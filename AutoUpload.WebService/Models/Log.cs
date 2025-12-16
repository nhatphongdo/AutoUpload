using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoUpload.WebService.Models
{
    public class Log
    {
        public long Id { get; set; }

        public long? UserId { get; set; }

        public User User { get; set; }

        public long? LicenseId { get; set; }

        public License License { get; set; }

        [MaxLength(2048)]
        public string Code { get; set; }

        [MaxLength(128)]
        public string IpAddress { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}