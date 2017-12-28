using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteMonitor.Models.SiteViewModels
{
    public class NewSiteViewModel
    {
        [Required]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Url")]
        [DataType(DataType.Url)]
        public string Uri { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = @"hh\:mm")]
        [Display(Name = "Интервал проверки (hh:mm)")]
        public TimeSpan CheckOnlineTimespan { get; set; }

        //to-do: заменить на automapper
        public static implicit operator NewSiteViewModel(Site site)
        {
            return new NewSiteViewModel
            {
                Name = site.Name,
                Uri = site.UriString,
                CheckOnlineTimespan = site.CheckOnlineTimespan
            };
        }

        public static implicit operator Site(NewSiteViewModel siteViewModel)
        {
            return new Site()
            {
                Name = siteViewModel.Name,
                UriString = siteViewModel.Uri,
                CheckOnlineTimespan = siteViewModel.CheckOnlineTimespan
            };

        }
    }
}
