using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Lütfen KategoriAdi kısmını doldurunuz.")]
        [MaxLength(50)]
        public string KategoriAdi { get; set; }
        public List<Blog> Bloglar { get; set; }
    }
}
