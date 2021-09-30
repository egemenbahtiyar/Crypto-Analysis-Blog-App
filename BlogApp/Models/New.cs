using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Models
{
    public class New
    {
        [Key]
        public int NewId { get; set; }
        [Required(ErrorMessage = "Lütfen Başlık kısmını doldurunuz.")]
        public string HaberBaslik { get; set; }
        [Required(ErrorMessage = "Lütfen Icerik kısmını doldurunuz.")]
        public string HaberIcerik { get; set; }
        [Required(ErrorMessage = "Lütfen eklenme tarihini giriniz.")]
        public DateTime HaberEklenmeTarihi { get; set; }
        [DisplayName("Resim Adı")]
        public string Resim { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Lütfen Bloğa ait Resim yükleyiniz.")]
        [DisplayName("Resim Yükle")]
        public IFormFile ImageFile { get; set; }
    }
}
