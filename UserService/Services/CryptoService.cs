using System;
using System.Security.Cryptography;
using System.Text;

namespace UserService.Services
{
    public class CryptoService
    {
        public CryptoService()
        {
        }

        public static string SHA1(string content)
        {
            return SHA1(content, Encoding.UTF8);
        }

        public static string SHA1(string content, Encoding encode)
        {
            try
            {
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                byte[] bytes_in = encode.GetBytes(content);
                byte[] bytes_out = sha1.ComputeHash(bytes_in);
                sha1.Dispose();
                string result = BitConverter.ToString(bytes_out);
                result = result.Replace("-", "");
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("sha1 error：" + ex.Message);
            }
        }
    }
}
