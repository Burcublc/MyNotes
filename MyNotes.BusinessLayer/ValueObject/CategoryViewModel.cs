﻿using MyNotes.EntityLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.BusinessLayer.ValueObject
{
    public class CategoryViewModel
    {
        public Category Category { get; set; }=new Category();
        //public int Id { get; set; }
        //[DisplayName("Başlık")]
        //public string Title { get; set; }
        //[DisplayName("Açıklama")]
        //public string Description { get; set; }
        //public DateTime? CreatedOn { get; set; }
        //public DateTime? ModifiedOn { get; set; }
        //[DisplayName("Kullanıcı Adı")]
        //public string ModifiedUserName { get; set; }
    }
}
