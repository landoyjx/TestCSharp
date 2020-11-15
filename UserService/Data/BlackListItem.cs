using System;
using System.ComponentModel.DataAnnotations;

namespace UserService.Data
{
    public class BlackListItem
    {
        public BlackListItem(string ip)
        {
            Ip = ip;
            LastTime = UnixTime();
            Added = false;
        }

        public static long UnixTime()
        {
            return (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        [Key]
        public string Ip { set; get; }

        public long LastTime { set; get; }

        public bool Added { set; get; }
    }
}
