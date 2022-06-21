using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.EntityLayer
{
    [Table("tblMyNotesUsers")]
    public class MyNotesUser:BaseEntity
    {
        [StringLength(30)]
        public string Name { get; set; }

        [StringLength(30)]
        public string LastName { get; set; }

        [StringLength(30),Required]
        public string UserName { get; set; }

        [StringLength(100),Required]
        public string Email { get; set; }

        [StringLength(100),Required]
        public string Password { get; set; }

        public bool IsActive { get; set; }

        public string ProfileImageFileName { get; set; }

        [Required]
        public Guid ActivateGuid { get; set; } = Guid.NewGuid(); //Benzersiz bir id oluşturuyor
        public bool IsDelete { get; set; } = false;

        public bool IsAdmin { get; set; }

        public virtual List<Note> Notes { get; set; } = new List<Note>(); //ICollection,IList,IQuerable,lİST olarakta verebiliriz

        //virtual;eager loading ve lazyloading adında iki tane yükleme(doldur) eylemi,loading oluşturacağımız nesnenin alt bağlamlar var ise
        //hepsini getir.lazing loading ise oluşturulan nesne getirilir ilgili bağlamlar yüklenmez,
        //ögrencier ve sınıflar tablom var bunların arasındada foreign key bağlantım var
        //ben ogrenciler listesi aldığımda eğer eager loading kullanrak alıyorsam liste şöyle olur;
        /*
         ad 
         soyad
         sınıf[
            {
                sinifadi,
                kat,
                egitmen,
            }
        ]
         */
        //virtual kullandığımız için bu lazy loading oluyo;lazy loadingte sadece istenilen tablonun sadece idsi getirilir
        //yoksa eage loading olur

        public virtual List<Comment> Comments { get; set; }= new List<Comment>();
        public virtual List<Liked> Likes { get; set; }=new List<Liked>();
    }
}
