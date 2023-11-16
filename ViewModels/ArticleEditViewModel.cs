using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationList13X.Models;

namespace WebApplicationList13X.ViewModels
{
    public class ArticleEditViewModel
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int CategoryId { get; set; }
    }
}
