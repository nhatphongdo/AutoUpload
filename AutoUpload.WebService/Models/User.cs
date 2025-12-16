using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoUpload.WebService.Models
{
    public class User
    {
        public long Id { get; set; }

        [MaxLength(512)]
        public string Email { get; set; }

        public string MachineId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get;set; }

        public bool IsDeleted { get; set; }

        public List<License> Licenses { get; set; }

        public List<Log> Logs { get; set; }

        public List<MockupTemplate> MockupTemplates { get; set; }
    }
}
