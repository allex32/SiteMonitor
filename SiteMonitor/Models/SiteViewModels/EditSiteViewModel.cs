using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteMonitor.Models.SiteViewModels
{
    public class EditSiteViewModel
    {
        public int SiteId { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = @"hh\:mm")]
        [Display(Name = "Интервал проверки (hh:mm)")]
        public TimeSpan CheckOnlineTimespan { get; set; }

        //to-do: заменить на automapper
        public static implicit operator EditSiteViewModel(Site site)
        {
            return new EditSiteViewModel
            {
                SiteId = site.SiteId,
                Name = site.Name,
                CheckOnlineTimespan = site.CheckOnlineTimespan
            };
        }

        public static implicit operator Site(EditSiteViewModel siteViewModel)
        {
            return new Site()
            {
                SiteId = siteViewModel.SiteId,
                Name = siteViewModel.Name,
                CheckOnlineTimespan = siteViewModel.CheckOnlineTimespan
            };

        }
    }
}
