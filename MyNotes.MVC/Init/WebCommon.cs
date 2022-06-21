using MyNotes.BusinessLayer.Modals;
using MyNotes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyNotes.MVC.Init
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUserName()
        {
            if (CurrentSession.User != null)
            {
                var user=CurrentSession.User;
                return user.UserName;
            }
            return "system";
        }
        //sonra Global.asax.cs 'e git ve bir satır kod ekle
    }
}