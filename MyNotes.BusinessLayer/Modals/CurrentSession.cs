using MyNotes.EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyNotes.BusinessLayer.Modals
{
    public class CurrentSession
    {
        public static MyNotesUser User {
            get
            {
                return Get<MyNotesUser>("Login");
            //    if (HttpContext.Current.Session["Login"] != null)
            //    {
            //        return HttpContext.Current.Session["Login"] as MyNotesUser;
            //    }
            //    return null;
            }
        }
        //Örneğin iki sonra nt ile giriş yapmak istediğimde
        //public static Note note
        //{
        //    get
        //    {
        //        return Get<Note>("nt");
        //    }
        //}
        public static T Get<T>(string key)
        {
            //Session lkısmında biz sadece login verdik ama biz iki gün sonra mogin ile giriş yapmak istediğimde yukarıdaki sadece tek iş yaptığı için bu işlemi generic hala getirerek birden fazla işi yaptırıyorum
            if(HttpContext.Current.Session[key] != null)
            {
                return (T)HttpContext.Current.Session[key];
            }
            return default(T);
        }

        public static void Set<T>(string key,T obj)
        {
            HttpContext.Current.Session[key] = obj;
        }
        //sessionı komple kökten kaldırma (bu ise komple kaldırıyor)
        public static void Remove(string key)
        {
            if (HttpContext.Current.Session[key] != null)
            {
                HttpContext.Current.Session.Remove(key);
            }
        }
        //sessionı temizleme (cookiesteki logini temizliyor)
        public static void Clear()
        {
            //bütün sessionları temizliyor
            HttpContext.Current.Session.Clear();
        }
    }
}
