using BladeRazorMvcExamples.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BladeRazorMvcExamples.ViewModels
{
    public class BladeViewModel
    {
        public dynamic DynamicModel { get; set; } = null;
        public ICollection<dynamic> DynamicList { get; set; } = null;
        public IEnumerable<SelectListItem> SelectItems { get; set; } = null;
        public IDictionary<string, IEnumerable<SelectListItem>> SelectItemsDictionary { get; set; } = null;
        
        public BladeViewModel()
        {
        }

        public BladeViewModel(dynamic dynamicModel)
        {
            this.DynamicModel = dynamicModel;
        }
    }
}
