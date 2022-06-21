using MyNotes.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.BusinessLayer
{
    public class Test
    {
        public Test()//constructor method
        {
            MyNotesContext db = new MyNotesContext();
            db.Categories.ToList();
        }
        
    }
}
