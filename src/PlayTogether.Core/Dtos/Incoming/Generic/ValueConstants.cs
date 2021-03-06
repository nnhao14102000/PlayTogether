using System;
namespace PlayTogether.Core.Dtos.Incoming.Generic
{
    public class ValueConstants
    {
        public const float PricePerHourMinValue = 10000;
        public const float MaxHourHireMinValue = 1;
        public const float MaxHourHireMaxValue = 5;
        public const string DefaultAvatar = "https://firebasestorage.googleapis.com/v0/b/play-together-flutter.appspot.com/o/avatar%2Fdefault-profile-picture.jpg?alt=media&token=79641b44-454b-43e0-8c57-85d1431fcfce";

        public const string BaseUrl = "play-together.azurewebsites.net/api/play-together";

        public const int OrderProcessExpireTime = 5; // minute
        public const int HourActiveMoney = 6; // hour
        public const int HourActiveFeedbackReport = 3; // hour

        public const int OrderProcessExpireTimeForTest = 2; // minute
        public const int HourActiveMoneyForTest = 10; // minute
        public const int HourActiveFeedbackReportForTest = 5; // minute
    }
}