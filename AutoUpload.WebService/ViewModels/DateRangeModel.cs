using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoUpload.WebService.ViewModels
{
    public class DateRangeModel
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get;set; }

        public string ApiKey { get; set; }
    }
}
