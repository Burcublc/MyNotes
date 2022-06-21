using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.EntityLayer.Messages
{
    public class ErrorMessageObj
    {
        //Hata Kodlarını getirmek için
        public ErrorMessageCode Code { get; set; } //kodumu
        public string Message { get; set; } //ve mesajımı getirecek
    }
}
