using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppBlaBlaCar.Triggers
{
    public class PassengersTrigger : TriggerAction<Entry>
    {
        protected override void Invoke(Entry sender)
        {
            int n;
            bool isNumeric = int.TryParse(sender.Text, out n);
            if (string.IsNullOrWhiteSpace(sender.Text) || !isNumeric)
            {
                sender.Text = "1"; //Texto anterior
            }
            else
            {
                if (n < 0)
                {
                    sender.Text = "1";
                }
                else if (n > 20)
                {
                    sender.Text = "20";
                }
            }
        }
    }
}
