using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.Common.Helper
{
    public class ConfigHelper
    {
        //önce git web confige add keyleri ekle son buraya gel
        //public static string Get(string key)
        //{
        //    return ConfigurationManager.AppSettings[key];
        //}

        public static T Get<T>(string key)//generic olarak çözümledik
        {
            return (T)Convert.ChangeType(ConfigurationManager.AppSettings[key], typeof(T)); //objeyi İçerisindekin değere dönüştürüyor
        }
    }
}
