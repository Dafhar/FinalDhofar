using System.Security.Cryptography;
using System.Text;

namespace DhofarAppApi.Helpers
{
    public class RandomStringGenerator
    {
        public static string GenerateRandomString(int length)
        {
            const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_-+=<>?";

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] randomBytes = new byte[length];
                rng.GetBytes(randomBytes);

                StringBuilder result = new StringBuilder(length);

                foreach (byte b in randomBytes)
                {
                    result.Append(allowedChars[b % allowedChars.Length]);
                }

                return result.ToString();
            }
        }
    }
}