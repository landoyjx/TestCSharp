using System;
namespace UserService.Data.Items
{
    public class RegisterVo
    {
        public string username { get; set; }
        public string password { get; set; }
        public string name { get; set; }

        public string IsValid()
        {
            if (string.IsNullOrEmpty(username))
            {
                return "invalid username";
            }
            if (string.IsNullOrEmpty(password))
            {
                return "invalid password";
            }
            return "";
        }
    }
}
