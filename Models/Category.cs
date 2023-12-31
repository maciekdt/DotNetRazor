﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationList13X.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Article> Articles { get; set; }
    }
}
