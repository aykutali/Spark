using System.Diagnostics;

namespace SparkApp.Common
{
    public static class EntityValidationConstants
    {
        public static class Game
        {
            public const int TitleMinLength = 2;
            public const int TitleMaxLength = 100;
            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLength = 255;
            public const string ReleasedDateFormat = "dd/MM/yyyy";

        }

        public static class Genre
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;
            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLength = 255;
        }

        public static class Director
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 100;
            public const int AboutMinLength = 10;
            public const int AboutMaxLength = 255;
        }

        public static class Developer
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 100;
        }
    }
}
