using MyNotes.BusinessLayer.Abstract;
using MyNotes.BusinessLayer.ValueObject;
using MyNotes.Common.Helper;
using MyNotes.EntityLayer;
using MyNotes.EntityLayer.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.BusinessLayer
{
    public class MyNotesUserManager:ManagerBase<MyNotesUser>
    {
        //kullanıcı username kontrolü yapmalıyım
        //kullanıcı email kontrolü yapmalıyım
        //kayıt işlemi gerçekleştirmeliyiz
        //aktivasyon eposta gönderimi
        private BusinessLayerResult<MyNotesUser> res = new BusinessLayerResult<MyNotesUser>();
        public BusinessLayerResult<MyNotesUser> LoginUser(LoginViewModel data) 
        {
            res.Result = Find(s => s.UserName == data.Username && s.Password == data.Password && s.IsDelete!=true);
            if (res.Result != null)
            {
                //böyle bir kayıt var null gelmedi
                if (!res.Result.IsActive)
                {
                    //IsActive false ise. Bunun false gelmesi bir hata döndürür
                    res.AddError(ErrorMessageCode.UserIsNotActive, "Kullanici Adi Aktifleştirilmemiş");
                    res.AddError(ErrorMessageCode.CheckYourEmail, "Lutfen mailinizi kontrol ediniz.");
                }
               
            }
            else
            {
                res.AddError(ErrorMessageCode.UsernameOrPassWrong, "Kullanici Adi veya Sifre Hatalı.Lutfen kontrol ediniz!");
            }
            return res;
        }
        public BusinessLayerResult<MyNotesUser> RegisterUser(RegisterViewModel data)
        {
            MyNotesUser user = Find(s => s.UserName == data.UserName || s.Email == data.Email);
            if (user != null)
            {
                if (user.UserName == data.UserName)
                {
                    res.AddError(ErrorMessageCode.UserNameAlreadyExist, "Bu kullanici adi daha once alinmistir.");
                }
                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "Bu Email Daha once kaydedilmistir.");
                }
            }
            else
            {
                int dbResult = base.Insert(new MyNotesUser()
                {
                    Name=data.Name,
                    LastName=data.LastName,
                    Email=data.Email,
                    UserName=data.UserName,
                    Password=data.Password,
                    IsActive=false,
                    IsAdmin=false,
                    ActivateGuid=Guid.NewGuid(),
                    ProfileImageFileName="user.jpg"
                });
                if (dbResult > 0)
                {
                    res.Result=Find(s=>s.Email==data.Email && s.UserName==data.UserName);
                    //Activasyon Emaili gönderilecek burada 

                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{res.Result.ActivateGuid}";
                    string body = $"Merhaba{res.Result.UserName};<br><br> Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'>Bu Linke Tiklayin</a>.";
                    MailHelper.SendMail(body, res.Result.Email, "MyNotes Aktivasyon Mail Hizmeti");

                }
            }
            return res;
        }
        public BusinessLayerResult<MyNotesUser> SendMail(LoginViewModel data)
        {
            res.Result = Find(s => s.Password == data.Password && s.UserName == data.Username);
            string siteUri = ConfigHelper.Get<string>("SiteRootUri");
            string activateUri = $"{siteUri}/Home/UserActivate/{res.Result.ActivateGuid}";
            string body =
                $"Merhaba {res.Result.UserName};<br><br> Hesabinizi aktiflestirmek icin <a href='{activateUri}' target='_blank'>bu linke tiklayin</a>.";
            MailHelper.SendMail(body, res.Result.Email, "MyNotes Aktivasyon mail hizmet");

            return res;

        }
        public BusinessLayerResult<MyNotesUser> ActivateUser(Guid id)
        {
            res.Result = Find(s => s.ActivateGuid == id);
            if (res.Result != null)
            {
                //resultım varsa
                if (res.Result.IsActive)
                {
                    //aktifse
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktif");
                    return res;
                }
                res.Result.IsActive = true;
                Update(res.Result);
            }
            else
            {
                //böyle bir kayit bulunamadıysa
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExist, "Boyle bir aktivasyon kodu yoktur");
            }
            return res;
        }
        public new BusinessLayerResult<MyNotesUser> Insert(MyNotesUser data)
        {
            MyNotesUser user=Find(s=>s.UserName==data.UserName ||s.Email==data.Email);
            res.Result = data;
            if (user != null)
            {
                if (user.UserName == data.UserName)
                {
                    res.AddError(ErrorMessageCode.UserNameAlreadyExist, "Bu Kullanici adi daha once alinmistir.");
                }
                if(user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "Bu email daha once alinmistir.");
                }
            }
            else
            {
                res.Result.ActivateGuid=Guid.NewGuid();
                if (base.Insert(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotInserted, "Kullanici Eklenemedi");
                }
            }
            return res;
        }
        public new BusinessLayerResult<MyNotesUser> Update(MyNotesUser data) //burda bu metodları newleyerek yeniden inşa ediyoruz yani managerbasedeki update ve insert yerine burdaki update ve inserti kullanıcak yani bir override işelmi yapıpyoruz
        {
            MyNotesUser user=Find(s=>s.Id!=data.Id && (s.UserName==data.UserName || s.Email==data.Email));
            if(user!= null && user.Id!=data.Id)
            {
                if (user.UserName == data.UserName)
                {
                    res.AddError(ErrorMessageCode.UserNameAlreadyExist, "Bu kullanici adini alamazsiniz");
                }
                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "Bu emaili alamazsiniz");
                }
                return res;
            }
            res.Result = Find(s => s.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name=data.Name;
            res.Result.LastName=data.LastName;
            res.Result.Password=data.Password;
            res.Result.UserName=data.UserName;
            res.Result.IsAdmin=data.IsAdmin;
            res.Result.IsActive=data.IsActive;
            res.Result.IsDelete = data.IsDelete;
            if (base.Update(res.Result) == 0) //base ile managerbasedeki Update'e gonderiyoruz
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Kullanici bilgileri guncelenemedi");
            }
            return res;
        }
        public BusinessLayerResult<MyNotesUser> GetUserById(int id)
        {
            res.Result = Find(s => s.Id == id);
            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanici bulanamadi.");
                
            }
            return res;
        }
        public BusinessLayerResult<MyNotesUser> UpdateProfile(MyNotesUser data)
        {
            MyNotesUser user = Find(s => s.Id != data.Id && (s.UserName == data.UserName || s.Email == data.Email));
            if (user != null && user.Id != data.Id)
            {
                if (user.UserName == data.UserName)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Bu kullanici adi daha once kaydedilmis.");
                }
                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "Bu email daha once kaydedilmis.");
                }
                return res;
            }
            res.Result = Find(s => s.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.LastName = data.LastName;
            res.Result.Password = data.Password;
            res.Result.UserName = data.UserName;
            if (!string.IsNullOrEmpty(data.ProfileImageFileName))
            {
                res.Result.ProfileImageFileName = data.ProfileImageFileName;
            }

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profil guncellenemedi.");
            }

            return res;
        }
        public BusinessLayerResult<MyNotesUser> RemoveUserById(int id)
        {
            res.Result = Find(s => s.Id == id);
            if(res.Result!=null)
            {        
                res.Result.IsDelete = true;
                if (base.Update(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove,"Kullanici Silinemedi");
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanici Bulunamadi");
            }
            return res;
        }
    }
}
