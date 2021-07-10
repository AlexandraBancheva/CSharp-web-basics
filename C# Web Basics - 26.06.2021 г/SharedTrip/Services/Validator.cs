namespace SharedTrip.Services
{
    using SharedTrip.ViewModels.Trips;
    using SharedTrip.ViewModels.Users;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using static SharedTrip.Data.DataConstants;

    public class Validator : IValidator
    {
        public ICollection<string> ValidateUser(RegisterUserFormModel model)
        {
            var errors = new List<string>();

            if (model.Username.Length < UserMinLength || model.Username.Length > DefaultMaxLength)
            {
                errors.Add($"Username '{model.Username}' is not valid. It must be between {UserMinLength} and {DefaultMaxLength} characters long.");
            }

            if (!Regex.IsMatch(model.Email, UserEmailRegularExpression))
            {
                errors.Add($"Email {model.Email} is not a valid e-mail address.");
            }

            if (model.Password.Length < UserPasswordMinLength || model.Password.Length > DefaultMaxLength)
            {
                errors.Add($"The provided password is not valid. It must be between {UserPasswordMinLength} and {DefaultMaxLength} characters long.");
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

        public ICollection<string> ValidateTrip(AddTripFormModel model)
        {
            var errors = new List<string>();

            if (model.StartPoint == null || string.IsNullOrWhiteSpace(model.StartPoint))
            {
                errors.Add($"Start point '{model.StartPoint}' is not valid.");
            }

            if (model.EndPoint == null || string.IsNullOrWhiteSpace(model.EndPoint))
            {
                errors.Add($"End point '{model.EndPoint}' is not valid.");
            }

            if (model.DepartureTime == null || string.IsNullOrWhiteSpace(model.DepartureTime))
            {
                errors.Add($"Departure time '{model.DepartureTime}' is not valid.");
            }

            if (model.ImagePath == null)
            {
                errors.Add($"Image URL '{model.ImagePath}' is not valid.");
            }

            if (model.Seats < SeatsMinValue || model.Seats > SeatsMaxValue)
            {
                errors.Add($"Seats '{model.Seats}' are not valid. They must be between {SeatsMinValue} and {SeatsMaxValue}");
            }

            return errors;
        }
    }
}
