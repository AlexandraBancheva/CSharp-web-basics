using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCards.Data
{
    public class DataConstants
    {
        public const int IdMaxLength = 40;

        public const int UsernameMinLength = 5;
        public const int DefaultMaxLength = 20;
        public const string UserEmailRegularExpression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        public const int PasswordMinLength = 6;

        public const int NameCardMaxLength = 15;
        public const int DescriptionMaxLength = 200;
        public const string ToughKeyword = "Tough";
        public const string ChallengerKeyword = "Challenger";
        public const string ElusiveKeyword = "Elusive";
        public const string OverwhelmKeyword = "Overwhelm";
        public const string LifestealKeyword = "Lifesteal";
        public const string EphemeralKeyword = "Ephemeral";
        public const string FearsomeKeyword = "Fearsome";
    }
}
