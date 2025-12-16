using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoUpload.WebService.ViewModels
{
    public class LicenseModel
    {
        public string Email { get; set; }

        public string LicenseKey { get; set; }

        public string MachineId { get; set; }

        public string Checksum { get; set; }
    }
}