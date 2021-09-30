using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Models
{
    public class Superuser
    {
        public int Id { get; set; }
        [Required]
        public string KullaniciAdi{ get; set; }
        [Required]
        public string Sifre { get; set; }
    }
}
