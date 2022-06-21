using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyNotes.BusinessLayer;
using MyNotes.BusinessLayer.Modals;
using MyNotes.DataAccessLayer;
using MyNotes.EntityLayer;

namespace MyNotes.MVC.Controllers
{
    public class CommentController : Controller
    {
        //private MyNotesContext db = new MyNotesContext();
        private NoteManager nm = new NoteManager();
        private CommandManager cm = new CommandManager();


        public ActionResult Index()
        {
            return View(cm.List());
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = cm.Find(x => x.Id == id); //find bizden bir lamda expression istiyor
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Comment comment,int? noteId) //int? bir value typetıt bu null geldiğinde sistem hataya düşer buna engel olmak için sen int in kontrolünü bana bırak
        {
            ModelState.Remove("CreatedOn"); //CreatedOn ifadesini ModelState'ten çıkartmak istiyorum
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");
            if (ModelState.IsValid)
            {
                if (noteId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Note note = nm.Find(s => s.Id == noteId);
                if (note == null)
                {
                    return new HttpNotFoundResult();
                }
                comment.Note = note;
                comment.Owner = CurrentSession.User;

                if (cm.Insert(comment) > 0)//burda insert işlemi yapar
                {
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }
                //db.Comments.Add(comment);
                //db.SaveChanges();
                //return RedirectToAction("Index");
            }

            //return View(comment);
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowNoteComments(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Note note = nm.QList().Include("Comments").FirstOrDefault(s => s.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }

            return PartialView("_PartialComments", note.Comments);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment =cm.Find(s=>s.Id== id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        
        [HttpPost]
        public ActionResult Edit(int? id,string text)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = cm.Find(s => s.Id == id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            comment.Text= text;
            if (cm.Update(comment) > 0) //burda eğer comment sıfırdan büyükse yani biz burda zaten tek bir işlem yaptığımız için bir dönecek
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet); 
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            //if (ModelState.IsValid)
            //{
            //    //db.Entry(comment).State = EntityState.Modified;
            //    //db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //return View(comment);

        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = cm.Find(s => s.Id == id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            if (cm.Delete(comment) > 0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

       
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
