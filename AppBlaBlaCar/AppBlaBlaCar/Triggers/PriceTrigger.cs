using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppBlaBlaCar.Triggers
{
    //ESTE TRIGGER NOS AYUDARA A VALIDAR LOS VALORES QUE SE INGRESAN EN NUESTROS ENTRIES
    public class PriceTrigger : TriggerAction<Entry>
    {
        protected override void Invoke(Entry sender)
        {
            double n;
            bool isNumeric = double.TryParse(sender.Text, out n);
            if (string.IsNullOrWhiteSpace(sender.Text) || !isNumeric)
            {
                sender.Text = "";
            }
            else
            {
                if (n < 0)
                {
                    sender.Text = "0"; //Si es menor a 0 lo seteamos como 0
                }
            }
        }
    }
}
