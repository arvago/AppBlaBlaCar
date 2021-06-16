using AppBlaBlaCar.Models;
using AppBlaBlaCar.Services;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppBlaBlaCar.ViewModels
{
    public class UserDetailViewModel : BaseViewModel
    {
        Command saveCommand;
        public Command SaveCommand => saveCommand ?? (saveCommand = new Command(SaveAction));
        Command deleteCommand;
        public Command DeleteCommand => deleteCommand ?? (deleteCommand = new Command(DeleteAction));
        Command _SelectPictureCommand;
        public Command SelectPictureCommand => _SelectPictureCommand ?? (_SelectPictureCommand = new Command(SelectPictureAction));

        UserModel userSelected;
        public UserModel UserSelected
        {
            get => userSelected;
            set => SetProperty(ref userSelected, value);
        }

        int _IDUser;
        public int IDUser
        {
            get => _IDUser;
            set => SetProperty(ref _IDUser, value);
        }
        string _Name;
        public string Name
        {
            get => _Name;
            set => SetProperty(ref _Name, value);
        }

        string _Email;
        public string Email
        {
            get => _Email;
            set => SetProperty(ref _Email, value);
        }

        string _Password;
        public string Password
        {
            get => _Password;
            set => SetProperty(ref _Password, value);
        }

        string _Role;
        public string Role
        {
            get => _Role;
            set => SetProperty(ref _Role, value);
        }
        string _Picture;
        public string Picture
        {
            get => _Picture;
            set => SetProperty(ref _Picture, value);
        }

        public UserDetailViewModel(UserModel userSelected)
        {
            UserSelected = userSelected;
            Name = userSelected.Picture;
            Email = userSelected.Email;
            Password = userSelected.Password;
            Picture = userSelected.Picture;
            Role = userSelected.Role;
        }

        //METODO PARA SELECCIONAR UNA FOTO DEL DISPOSITIVO
        private async void SelectPictureAction()
        {
            try
            {
                await CrossMedia.Current.Initialize();
                //VALIDA LOS PERMISOS PARA SELECCIONAR FOTO
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert("AppBlaBlaCar", "No es posible seleccionar fotografías en el dispositivo", "Ok");
                    return;
                }

                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });

                if (file == null)
                    return;
                //ASIGNA LA FOTO DESPUES DE CONVERTIRLA A BASE 64
                UserSelected.Picture = file.Path;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("AppBlaBlaCar", $"Se generó un error al cargar la fotografía ({ex.Message})", "Ok");
            }
        }

        private async void SaveAction()
        {
            ResponseModel response;
            try
            {
                UserModel user = new UserModel
                {
                    IDUser = _IDUser,
                    Name = _Name,
                    Email = _Email,
                    Password = _Password,
                    Role = _Role,
                    Picture = _Picture

                };

                if (user.IDUser > 0)
                {
                    response = await new ApiService().PutDataAsync("User", user);
                }
                else
                {
                    response = await new ApiService().PostDataAsync("User", user);
                }
                if (response == null || !response.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert("AppBlaBlaCar", $"Error al procesar el usuario {response.Message}", "Ok");
                    return;
                }
                Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async void DeleteAction()
        {
            ResponseModel response = null;
            try
            {
                UserModel user = new UserModel
                {
                    IDUser = _IDUser,
                    Name = _Name,
                    Email = _Email,
                    Password = _Password,
                    Picture = _Picture,
                    Role = _Role
                };
                if (user.IDUser > 0)
                {
                    response = await new ApiService().DeleteDataAsync("User", user.IDUser);
                }
                if (response == null || !response.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert("AppPets", $"Error al eliminar la mascota {response.Message}", "Ok");
                    return;
                }
                Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
