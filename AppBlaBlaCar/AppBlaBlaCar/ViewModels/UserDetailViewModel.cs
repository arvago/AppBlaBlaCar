using AppBlaBlaCar.Models;
using AppBlaBlaCar.Services;
using AppBlaBlaCar.Views;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppBlaBlaCar.ViewModels
{
    public class UserDetailViewModel : BaseViewModel
    {
        
        int _IDUser;
        string FileNameSelected = string.Empty;

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

        byte[] _ByteData;
        public byte[] ByteData
        {
            get => _ByteData;
            set => SetProperty(ref _ByteData, value);
        }

        bool _switchRole;
        public bool switchRole
        {
            get => _switchRole;
            set => SetProperty(ref _switchRole, value);
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

        object _ImgPicture;
        public object ImgPicture
        {
            get => _ImgPicture;
            set => SetProperty(ref _ImgPicture, value);
        }

        public UserDetailViewModel()
        {

        }

        public UserDetailViewModel(UserModel userSelected)
        {
            UserSelected = userSelected;
            _IDUser = userSelected.IDUser;
            Name = userSelected.Name;
            Email = userSelected.Email;
            Password = userSelected.Password;
            Picture = userSelected.Picture;
            Role = userSelected.Role;
            switchRole = (Role == "Driver") ? true : false;
            CargarImagen();
        }

        //METODO PARA SELECCIONAR UNA FOTO DEL DISPOSITIVO
        private async void SelectPictureAction()
        {
            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert("AppBlaBlaCar", "No es posible seleccionar fotografía en el dispositivo", "OK");
                    return;
                }

                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });

                if (file == null)
                    return;

                ByteData = await ConvertImageFilePathToByteArray(file.Path);
                ImgPicture = ImageSource.FromStream(() => new MemoryStream(ByteData));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("AppBlaBlaCar", $"Se generó un error al seleccionar la fotografía ({ex.Message})", "OK");
                throw;
            }            
        }

        private async void SaveAction()
        {
            ResponseModel response;
            try
            {
                Role = (switchRole == false) ? "User" : "Driver";

                UserModel user = new UserModel
                {
                    IDUser = _IDUser,
                    Name = Name,
                    Email = Email,
                    Password = Password,
                    Role = Role,
                    Picture = Picture
                };
                if (user.IDUser > 0)
                {
                    //Actualizar 
                    await new AzureService().DeleteFileAsync(AzureContainer.Image, FileNameSelected);
                    user.Picture = await new AzureService().UploadFileAsync(AzureContainer.Image, new MemoryStream(ByteData));
                    response = await new ApiService().PutDataAsync("User", user);
                    
                }
                else
                {
                    //Insertar
                    if (ImgPicture != null && ByteData.Length > 0)
                    {
                        user.Picture = await new AzureService().UploadFileAsync(AzureContainer.Image, new MemoryStream(ByteData));
                    }
                    else
                    {
                        await Task.Delay(5000);
                    }
                    response = await new ApiService().PostDataAsync("User", user);
                }

                if (response == null || !response.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert("AppBlaBlaCar", $"Error al procesar el usuario {response.Message}", "Ok");
                    return;
                }
                int id = (int)(long)response.Result;
                await Application.Current.MainPage.Navigation.PushAsync(new RidesView(id, user));
                
            }
            catch 
            {
                throw;
            }          
        }

        private async void DeleteAction()
        {
            ResponseModel response ;
            try
            {
                if (_IDUser > 0)
                {
                    response = await new ApiService().DeleteDataAsync("User", _IDUser);
                }
                await Application.Current.MainPage.Navigation.PopToRootAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<byte[]> ConvertImageFilePathToByteArray(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                FileStream stream = File.Open(filePath, FileMode.Open);
                byte[] bytes = new byte[stream.Length];
                await stream.ReadAsync(bytes, 0, (int)stream.Length);
                return bytes;
            }
            else
            {
                return null;
            }
        }

        private async void CargarImagen()
        {
            try
            {
                if (Picture != null)
                {
                    FileNameSelected = Picture;
                    ByteData = await new AzureService().GetFileAsync(AzureContainer.Image, FileNameSelected);
                    var image = ImageSource.FromStream(() => new MemoryStream(ByteData));
                    ImgPicture = image;
                }
            }
            catch (Exception ex)
            {
                await Task.Delay(5000);
                throw;
            }
        }

        
    }
}
