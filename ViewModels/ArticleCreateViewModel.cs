using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebApplicationList13X.Models;

namespace WebApplicationList13X.ViewModels
{
    public class ArticleCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public IFormFile File { get; set; }
        public int Price { get; set; }
    }
}
