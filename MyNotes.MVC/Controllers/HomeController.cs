using MyNotes.BusinessLayer;
using MyNotes.BusinessLayer.Modals;
using MyNotes.BusinessLayer.ValueObject;
using MyNotes.EntityLayer;
using MyNotes.EntityLayer.Messages;
using MyNotes.MVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyNotes.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyNotesUserManager mum=new MyNotesUserManager();
        private BusinessLayerResult<MyNotesUser> res;
        private readonly NoteManager nm = new NoteManager();
        public ActionResult ByCategoryId(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Note> notes = nm.QList().Where(s => s.Category.Id == id && s.IsDraft == false).OrderByDescending(s => s.ModifiedOn).ToList();
            return View("Index",notes);
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model) 
        {
            if (ModelState.IsValid) //geçersizse. model LoginViewModel 'ın şartlarını gerçekleştirmemişse yani mesela password int bir değer ama ben gidip string bir değer atarsam false döndürecek
            {
                TempData["pass"] = model.Password;
                TempData["uname"] = model.Username;
                res = mum.LoginUser(model);
                if (res.Errors.Count > 0)
                {
                    if (res.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotActive) != null)
                    {
                        ViewBag.SetLink = "http://Home/UserActivate/1234-2345-3456789";
                    }
                    res.Errors.ForEach(s => ModelState.AddModelError("", s.Message));
                    return View(model);
                }
                //Session["Login"] = res.Result;//CurrentSession classına gönderek bu yapıyı orda yaptık
                CurrentSession.Set("Login", res.Result);//bunun sayesinde birden fazla objeyi session yapabilirim
                return RedirectToAction("Index");//Home'nın indeksine gider
            }
            return View(model);
        }
        public ActionResult Index()
        {
            return View(nm.QList().OrderByDescending(s=>s.ModifiedOn).ToList()); //tarihe göre en yeni tarihte olanı bana sıralasın
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult LogOut()
        {
            //Session.Clear();
            //artık aşağıdaki bu işi yapacak
            CurrentSession.Clear();
            return RedirectToAction("Login");
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                MyNotesUserManager mum = new MyNotesUserManager();
                BusinessLayerResult<MyNotesUser> res = mum.RegisterUser(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(s => ModelState.AddModelError("", s.Message));
                    return View(model);
                }
                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "Kayit Basarili",
                    RedirectingUrl = "/Home/Login"
                };
                notifyObj.Items.Add("Lütfen e posta adresinize gönderdiğimiz  aktivasyon linkine tıklayarak hesabınızı aktive ediniz! ");
                return View("Ok", notifyObj);
            }
            return View(model);
        }
        public ActionResult RegisterOk()
        {
            return View();
        }
        public ActionResult UserActivate(Guid id)
        {
            res = mum.ActivateUser(id);
            if(res.Errors.Count > 0)
            {
                TempData["errors"] = res.Errors;
                return RedirectToAction("UserActivateCancel");
            }
            return RedirectToAction("UserActivateOk");
        }
        public ActionResult UserActivateOk()
        {
            return View();
        }
        public ActionResult UserActivateCancel()
        {
            List<ErrorMessageObj> errors = null;
            if (TempData["errors"] != null)
            {
                errors=TempData["errors"] as List<ErrorMessageObj>; //temp datanın içerisindeki verileri errormessage objeye göre uyarlıyor
            }
            return View(errors);
        }
        public ActionResult ShowProfile()
        {
            if (CurrentSession.User is MyNotesUser currentUser) res = mum.GetUserById(currentUser.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Olustu",
                    Items = res.Errors
                };
                return View("Error", errorNotifyObj);
            }
            return View(res.Result);
        }
        public ActionResult EditProfile()
        {
            if(CurrentSession.User is MyNotesUser currentUser) res=mum.GetUserById(currentUser.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title="Hata Olustu",
                    Items=res.Errors
                };
                return View("Error", errorNotifyObj);
            }
            return View(res.Result);
        }
        [HttpPost]
        public ActionResult EditProfile(MyNotesUser model,HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUserName");
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            if (ModelState.IsValid)
            {
                if (ProfileImage != null && ProfileImage.ContentType=="image/jpeg" || ProfileImage.ContentType == "image/jpg" || ProfileImage.ContentType == "image/png")
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";
                    ProfileImage.SaveAs(Server.MapPath($"~/image/{filename}"));
                    model.ProfileImageFileName = filename;

                }
                res = mum.UpdateProfile(model);
                if(res.Errors.Count > 0)
                {
                    ErrorViewModel errNotifyObj = new ErrorViewModel()
                    {
                        Title = "Profil Guncellenemedi",
                        Items =res.Errors,
                        RedirectingUrl="/Home/EditProfile"
                    };
                    return View("Error", errNotifyObj);
                }
                CurrentSession.Set("Login", res.Result);//yeniden login olmak için
                return RedirectToAction("ShowProfile");
            }
            return View(model);
        }
        public ActionResult DeleteProfile()
        {
            if(CurrentSession.User is MyNotesUser currentUser)
            {
                res = mum.RemoveUserById(currentUser.Id);
            }
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errNotifyObj = new ErrorViewModel()
                {
                    Title = "Profil Silinemedi",
                    Items=res.Errors,
                    RedirectingUrl="/Home/ShowProfile"
                };
                return View("Error", errNotifyObj);
            }
            CurrentSession.Clear();
            return RedirectToAction("Index");
        }
        public ActionResult SendEmail(LoginViewModel model)
        {
            model.Password = TempData["pass"].ToString();
            model.Username = TempData["uname"].ToString();

            mum.SendMail(model);

            OkViewModel notifyObj = new OkViewModel()
            {
                Title = "Email Gönderildi",
                RedirectingUrl = "/Home/Login"
            };
            notifyObj.Items.Add("Lutfen e-mail adresinize gelen aktivasyon linkine tıklayın");
            return View("Ok",notifyObj);
        }
    }
}