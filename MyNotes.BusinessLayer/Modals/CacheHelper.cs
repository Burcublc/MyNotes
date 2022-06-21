using MyNotes.EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace MyNotes.BusinessLayer.Modals
{
    public class CacheHelper
    {
        //her tarayıcının bir cache 'i var
        public static List<Category> GetCategoriesFromCache()
        {
            var result = WebCache.Get("categories");
            if(result == null)
            {
                CategoryManager cm=new CategoryManager();
                result = cm.List();
                WebCache.Set("category-cache",result,30,true);//içindeki hafızasına aldığını 30dakika içerisinde siliyor
            }
            return result;
        } 
        public static void RemoveCategoriesFromCache()
        {
            WebCache.Remove("category-cache");
        }
        public static void Remove(string key)
        {
            WebCache.Remove(key);
        }
    }
}
