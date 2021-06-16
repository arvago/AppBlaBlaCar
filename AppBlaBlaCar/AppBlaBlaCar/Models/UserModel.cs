using System;
using System.Collections.Generic;
using System.Text;

namespace AppBlaBlaCar.Models
{
    public class UserModel
    {
        public int IDUser { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
