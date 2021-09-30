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
    public class Blog
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Lütfen Başlık kısmını doldurunuz.")]
        public string Baslik { get; set; }
        [Required(ErrorMessage = "Lütfen Aciklama kısmını doldurunuz.")]
        public string Aciklama { get; set; }
        public string Icerik { get; set; }
        [Required(ErrorMessage = "Lütfen eklenme tarihini giriniz.")]
        public DateTime EklenmeTarihi { get; set; }
        public bool Onay { get; set; }
        public bool Anasayfa { get; set; }

        [DisplayName("Resim Adı")]
        public string Resim { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Lütfen Bloğa ait Resim yükleyiniz.")]
        [DisplayName("Resim Yükle")]
        public IFormFile ImageFile { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }


    }
}
