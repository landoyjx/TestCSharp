using System;
namespace UserService.Data.Items
{
    public class LoginVo
    {
        public string username { set; get; }
        public string password { set; get; }

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
