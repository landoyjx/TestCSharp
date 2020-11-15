using System;
using System.ComponentModel.DataAnnotations;
using UserService.Data.Items;

using UserService.Services;

namespace UserService.Data
{
    public class User
    {
        [MaxLength(256)]
        public string Uid { get; set; }
        [Key]
        [MaxLength(256)]
        public string Username { get; set; }
        [MaxLength(256)]
        public string Name { get; set; } // display name
        [MaxLength(256)]
        public string Password { get; set; }

        public User()
        {
        }

        public User(RegisterVo item)
        {
            this.Uid = System.Guid.NewGuid().ToString();
            this.Username = item.username;
            this.Name = item.name;
            this.Password = CryptoService.SHA1(item.password);
        }
    }
}
