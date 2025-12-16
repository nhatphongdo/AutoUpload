using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AutoUpload.WebService.Models
{
    public class MockupTemplate
    {
        public long Id { get; set; }

        [MaxLength(512)]
        public string Name { get; set; }

        [MaxLength(512)]
        public string Category { get; set; }

        public string FilePath { get; set; }

        public string ThumbnailPath { get; set; }

        public string ValidPlatforms { get; set; }

        public User User { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }
    }
}
