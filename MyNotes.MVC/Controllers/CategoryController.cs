using MyNotes.BusinessLayer;
using MyNotes.BusinessLayer.Modals;
using MyNotes.BusinessLayer.ValueObject;
using MyNotes.EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyNotes.MVC.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        CategoryManager cm = new CategoryManager();
        
        //[Authorize]
        public ActionResult Index()
        {
            var cat = cm.IndexCat();
            return View(cat);

            //return View(cm.List());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            //var cat = cm.Find(s => s.Id == id);
            CategoryViewModel cvm = cm.FindCate(id);
            if (cvm == null)
            {
                return HttpNotFound();
            }
            //}
            //CategoryViewModel cvm = new CategoryViewModel();
            //cvm.Id = cat.Id;
            //cvm.Title = cat.Title;
            //cvm.Description = cat.Description;
            //cvm.ModifiedOn = cat.ModifiedOn;
            //cvm.CreatedOn = cat.CreatedOn;
            //cvm.ModifiedUserName = cat.ModifiedUserName;
            return View(cvm);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(CategoryViewModel cat)
        {
            ModelState.Remove("Category.CreatedOn");
            ModelState.Remove("Category.ModifiedOn");
            ModelState.Remove("Category.ModifiedUserName");
            if (ModelState.IsValid)
            {
                cm.InsertCat(cat);
                CacheHelper.RemoveCategoriesFromCache(); //sayfa tekrar yüklendiğinde komple kaldırsın
                return RedirectToAction("Index");
            }
            return View(cat);
        }
        public ActionResult Edit(int? id) 
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            //var cat = cm.Find(s => s.Id == id);
            CategoryViewModel cvm = cm.GetEditCat(id);
            if (cvm == null)
            {
                return HttpNotFound();
            }
            return View(cvm);
        }

        [HttpPost]
        public ActionResult Edit(CategoryViewModel cat)
        {
            //ModelState.Remove("Category.CreatedOn");
            //ModelState.Remove("Category.ModifiedOn");
            //ModelState.Remove("Category.ModifiedUserName");
            if (ModelState.IsValid)
            {
                //CategoryViewModel category = cm.GetEditCat(cat.Category.Id);
                //category.Category.Title = cat.Category.Title;
                //category.Category.Description = cat.Category.Description;
                //Category cat1 = new Category();
                //cat1.Title= cat.Category.Title;
                //cat1.Description= cat.Category.Description;
                cm.UpdateCat(cat);
                CacheHelper.RemoveCategoriesFromCache();
                return RedirectToAction("Index");
            }
            return View(cat);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            CategoryViewModel cvm = cm.GetEditCat(id);
            if (cvm == null)
            {
                return HttpNotFound();
            }
            return View(cvm);
        }
        [HttpPost,ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            cm.DeleteCat(id);
            CacheHelper.RemoveCategoriesFromCache(); //sayfa tekrar yüklendiğinde komple kaldırsın
            return RedirectToAction("Index");
        }


    }
}