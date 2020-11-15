using System;
using System.ComponentModel.DataAnnotations;

namespace BlackListChecker.Data
{
    public class BlackListItem
    {
        [Key]
        public string Ip { set; get; }

        public long LastTime { set; get; }

        public bool Added { set; get; }
    }
}
