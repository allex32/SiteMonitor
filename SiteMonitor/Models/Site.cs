using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SiteMonitor.Models
{
    public class Site
    {
       
        public int SiteId { get; set; }

        [Display(Name="Название")]
        public string Name { get; set; }

        [DataType(DataType.Url)]
        [Display(Name = "Url")]
        public string UriString
        {
            get;set;
        }
       

        public Uri Uri
        {
            get
            {
                if (Uri.TryCreate(UriString, UriKind.Absolute, out var parsedUri))
                    return parsedUri;
                return null;
            }
        }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh\\:mm}")]
        [Display(Name = "Интервал проверки (hh:mm)")]
        public TimeSpan CheckOnlineTimespan { get; set; }


        [Display(Name = "Online")]
        public bool SiteAvailability { get; set; }

        [Display(Name = "Последняя проверка")]
        public DateTime LastCheck { get; set; }
    }
}
