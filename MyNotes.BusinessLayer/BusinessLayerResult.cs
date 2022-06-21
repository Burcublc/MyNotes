using MyNotes.EntityLayer.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.BusinessLayer
{
    public class BusinessLayerResult<T> where T : class
    {
        /**kullanıcı girişi için kullanıcam bu kısmı mesala kullanıcı hatalı bir giriş yaptı bende hata mesajı göndermem lazım bu kısmıda entity layerdan döndürücem*/
        public BusinessLayerResult()
        {
            Errors = new List<ErrorMessageObj>();
        }

        public List<ErrorMessageObj> Errors { get; set; }
        public T Result { get; set; }
        public void AddError(ErrorMessageCode code,string message)
        {
            Errors.Add(new ErrorMessageObj{ Code= code, Message = message });
        }
    }
}
