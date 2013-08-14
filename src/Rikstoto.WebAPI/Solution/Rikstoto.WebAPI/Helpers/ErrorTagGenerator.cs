using System;

namespace Rikstoto.WebAPI.Helpers
{
    public class ErrorTagGenerator
    {
        private static readonly Random RandomNumberGenerator = new Random();
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        /// <summary>
        /// Returns a semi-unique incident tag
        /// </summary>
        public static string NewErrorTag()
        {
            return RandomString(6);
        }

        private static string RandomString(int size)
        {
            var buffer = new char[size];

            for (int i = 0; i < size; i++)
            {
                buffer[i] = Chars[RandomNumberGenerator.Next(Chars.Length)];
            }

            return new string(buffer);
        }
    }
}