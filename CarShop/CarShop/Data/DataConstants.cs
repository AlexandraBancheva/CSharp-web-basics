using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Data
{
    public class DataConstants
    {
        public const int IdMaxLength = 40;

        public const int DefaultMaxLength = 20;
        public const int UserMinLength = 4;
        public const string UserEmailRegularExpression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        public const int UserPasswordMinLength = 5;

        public const string UserTypeMechanic = "Mechanic";
        public const string UserTypeClient = "Client";


        public const int AddCarMinLength = 5;
        public const int CarPlateNumMaxLength = 10;
        public const string CarPlateNumExpression = @"[A-Z]{2}\s[0-9]{4}\s[A-Z]{2}";
        public const int MinCarYear = 1900;
        public const int MaxCarYear = 2100;
    }
}
