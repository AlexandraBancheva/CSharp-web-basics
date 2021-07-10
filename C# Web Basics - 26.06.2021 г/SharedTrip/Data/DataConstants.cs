namespace SharedTrip.Data
{
    public class DataConstants
    {
        public const int IdMaxLength = 40;

        public const int UserMinLength = 5;
        public const int UserPasswordMinLength = 6;
        public const int DefaultMaxLength = 20;
        public const string UserEmailRegularExpression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        public const int SeatsMinValue = 2;
        public const int SeatsMaxValue = 6;

        public const int DescriptionMaxLength = 80;
    }
}
