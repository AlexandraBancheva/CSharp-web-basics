using BattleCards.Models.Cards;
using BattleCards.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static BattleCards.Data.DataConstants;

namespace BattleCards.Service
{
    public class Validator : IValidator
    {
        public ICollection<string> ValidateCard(AddCardFormModel model)
        {
            var errors = new List<string>();

            if (model.Name.Length < UsernameMinLength || model.Name.Length > NameCardMaxLength)
            {
                errors.Add($"Name '{model.Name}' is not valid. It must be between {UsernameMinLength} and {NameCardMaxLength} characters long.");
            }

            if (model.Image == null || !Uri.IsWellFormedUriString(model.Image, UriKind.Absolute))
            {
                errors.Add($"Image '{model.Image}' is not valid. It must be a valid URL.");
            }

            if (model.Keyword == null || 
                (model.Keyword != ToughKeyword && model.Keyword != ChallengerKeyword && model.Keyword != ElusiveKeyword 
                && model.Keyword != OverwhelmKeyword && model.Keyword != LifestealKeyword && model.Keyword != EphemeralKeyword 
                && model.Keyword != FearsomeKeyword))
            {
                errors.Add($"Keyword '{model.Keyword}' is not valid.");
            }

            if (model.Attack < 0)
            {
                errors.Add("Attack cannot be negative.");
            }

            if (model.Health < 0)
            {
                errors.Add("Health cannot be negative.");
            }

            if (model.Description == null || model.Description.Length > DescriptionMaxLength)
            {
                errors.Add("Description is not valid.");
            }

            return errors;
        }

        public ICollection<string> ValidateUser(RegisterUserFormModel model)
        {
            var errors = new List<string>();

            if (model.Username.Length < UsernameMinLength || model.Username.Length > DefaultMaxLength)
            {
                errors.Add($"Username '{model.Username}' is not valid. It must be between {UsernameMinLength} and {DefaultMaxLength} characters long.");
            }

            if (!Regex.IsMatch(model.Email, UserEmailRegularExpression))
            {
                errors.Add($"Email {model.Email} is not a valid e-mail address.");
            }

            if (model.Password.Length < PasswordMinLength || model.Password.Length > DefaultMaxLength)
            {
                errors.Add($"The provided password is not valid. It must be between {PasswordMinLength} and {DefaultMaxLength} characters long.");
            }

            if (model.Password.Any(x => x == ' '))
            {
                errors.Add($"The provided password cannot contain whitespaces.");
            }

            if (model.Password != model.ConfirmPassword)
            {
                errors.Add($"Password and its confirmation are different.");
            }

            return errors;
        }
    }
}
