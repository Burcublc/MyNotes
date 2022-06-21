using MyNotes.EntityLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.DataAccessLayer
{
    public class MyNotesContext:DbContext
    {
        public MyNotesContext() : base("SqlConDb") //bunsuzda kurar database 'imizi
        {
            Database.SetInitializer(new MyInitializer()); //Initializer yüklemek için tetikledik burda
        }
        public DbSet<MyNotesUser> MyNotesUser { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Liked> Likes { get; set; }

        
    }
}
