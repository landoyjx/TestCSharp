using System;
namespace UserService.Data.Items
{
    public class UserInfoVo
    {
        public UserInfoVo(User user)
        {
            userId = user.Uid;
            name = user.Name;
        }

        public string userId { set; get; }
        public string name { set; get; }
    }
}
