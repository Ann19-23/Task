using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceURL.Models
{
    public class Link
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Ссылка")]      
        public string LongUrl { get; set; }
        [Display(Name = "Сокращеная ссылка")]
        public string ShortUrl { get; set; }
        [Display(Name = "Дата создания")]
        public DateTime DateCreate { get; set; }
        [Display(Name = "Переходы")]
        public int NumberOfTransitions { get; set; }

    }
}
