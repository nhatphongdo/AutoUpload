using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoUpload.WebService.HashIds;
using AutoUpload.WebService.Models;
using AutoUpload.WebService.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace AutoUpload.WebService.Controllers
{
    [Route("api/[controller]")]
    public class SalesController : Controller
    {
        private readonly AutoUploadContext _autoUploadContext;
        private readonly ILogger<SalesController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfigurationRoot _configuration;

        public SalesController(AutoUploadContext autoUploadContext, ILogger<SalesController> logger, IHttpContextAccessor httpContextAccessor, IConfigurationRoot configuration)
        {
            _autoUploadContext = autoUploadContext;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        
        [HttpPost("sales")]
        public async Task<dynamic> Sales([FromBody] DateRangeModel model)
        {
            if (model.ApiKey != _configuration["Settings:ManagementApiKey"])
            {
                return NotFound();
            }

            var ignoreUsers = (_configuration["Settings:IgnoreUsers"] ?? "").Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
            var licenses = await _autoUploadContext.Licenses
                                                   .Where(x => !x.IsDeleted && !x.IsTrial && !ignoreUsers.Contains(x.UserId.ToString())
                                                               && ((x.ValidFrom >= model.FromDate && x.ValidFrom <= model.ToDate)
                                                                   || (x.ValidTo >= model.FromDate && x.ValidTo <= model.ToDate)
                                                                   || (x.ValidFrom <= model.FromDate && x.ValidTo >= model.ToDate)))
                                                   .ToListAsync();

            var sales = new Dictionary<DateTime, dynamic>();
            for (var date = new DateTime(model.FromDate.Year, model.FromDate.Month, 1);
                 date <= new DateTime(model.ToDate.Year, model.ToDate.Month, 1);
                 date = date.AddMonths(1))
            {
                sales.Add(date,
                          new
                          {
                              TotalLicenses = licenses.Count(x => ((x.ValidTo.Year * 12 + x.ValidTo.Month) - (x.ValidFrom.Year * 12 + x.ValidFrom.Month))
                                                                 > ((date.Year * 12 + date.Month) - (x.ValidFrom.Year * 12 + x.ValidFrom.Month))),
                              TotalPlatforms = licenses.Where(x => ((x.ValidTo.Year * 12 + x.ValidTo.Month) - (x.ValidFrom.Year * 12 + x.ValidFrom.Month))
                                                                   > ((date.Year * 12 + date.Month) - (x.ValidFrom.Year * 12 + x.ValidFrom.Month)))
                                                       .SelectMany(x => (x.ValidPlatforms ?? "").Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries))
                                                       .Count(),
                              TotalSingles = licenses.Count(x => ((x.ValidTo.Year * 12 + x.ValidTo.Month) - (x.ValidFrom.Year * 12 + x.ValidFrom.Month))
                                                                 > ((date.Year * 12 + date.Month) - (x.ValidFrom.Year * 12 + x.ValidFrom.Month))
                                                                 && (x.ValidPlatforms ?? "").Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Length == 1),
                              TotalBundlesOf2 = licenses.Count(x => ((x.ValidTo.Year * 12 + x.ValidTo.Month) - (x.ValidFrom.Year * 12 + x.ValidFrom.Month))
                                                                    > ((date.Year * 12 + date.Month) - (x.ValidFrom.Year * 12 + x.ValidFrom.Month))
                                                                    && (x.ValidPlatforms ?? "").Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Length == 2),
                              TotalBundlesOf3 = licenses.Count(x => ((x.ValidTo.Year * 12 + x.ValidTo.Month) - (x.ValidFrom.Year * 12 + x.ValidFrom.Month))
                                                                    > ((date.Year * 12 + date.Month) - (x.ValidFrom.Year * 12 + x.ValidFrom.Month))
                                                                    && (x.ValidPlatforms ?? "").Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Length == 3),
                              TotalBundlesOf4 = licenses.Count(x => ((x.ValidTo.Year * 12 + x.ValidTo.Month) - (x.ValidFrom.Year * 12 + x.ValidFrom.Month))
                                                                    > ((date.Year * 12 + date.Month) - (x.ValidFrom.Year * 12 + x.ValidFrom.Month))
                                                                    && (x.ValidPlatforms ?? "").Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Length == 4),
                              TotalBundlesOf5 = licenses.Count(x => ((x.ValidTo.Year * 12 + x.ValidTo.Month) - (x.ValidFrom.Year * 12 + x.ValidFrom.Month))
                                                                    > ((date.Year * 12 + date.Month) - (x.ValidFrom.Year * 12 + x.ValidFrom.Month))
                                                                    && (x.ValidPlatforms ?? "").Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Length == 5),
                              TotalBundlesOf6 = licenses.Count(x => ((x.ValidTo.Year * 12 + x.ValidTo.Month) - (x.ValidFrom.Year * 12 + x.ValidFrom.Month))
                                                                    > ((date.Year * 12 + date.Month) - (x.ValidFrom.Year * 12 + x.ValidFrom.Month))
                                                                    && (x.ValidPlatforms ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length == 6)
                          });
            }

            return new
            {
                Sales = sales
            };
        }
    }
}
