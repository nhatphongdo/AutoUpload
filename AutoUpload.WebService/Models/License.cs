using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoUpload.WebService.Models
{
    public class License
    {
        public long Id { get; set; }

        [MaxLength(256)]
        public string LicenseKey { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }

        public string ValidPlatforms { get; set; }

        public long? UserId { get; set; }

        public User User { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        public bool IsTrial { get; set; }

        public List<Log> Logs { get; set; }
    }
}