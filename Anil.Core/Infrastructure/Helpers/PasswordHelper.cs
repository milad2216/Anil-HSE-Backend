using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Anil.Core.Infrastructure.Helpers
{
    public class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            var result = string.Empty;
            using (var myHash = SHA256.Create())
            {
                var byteArrayResultOfRawData =
                      Encoding.UTF8.GetBytes(password);

                var byteArrayResult =
                     myHash.ComputeHash(byteArrayResultOfRawData);

                result =
                  string.Concat(Array.ConvertAll(byteArrayResult,
                                       h => h.ToString("X2")));
            }
            return result;
        }
    }
}
