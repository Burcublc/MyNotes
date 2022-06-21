using MyNotes.EntityLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.DataAccessLayer
{
    public class MyInitializer:CreateDatabaseIfNotExists<MyNotesContext> //setIntilizer burayı tetikler
    {
        protected override void Seed(MyNotesContext context)
        {
            MyNotesUser admin = new MyNotesUser()
            {
                Name = "Burcu",
                LastName = "Balci",
                Email = "burcubalc7@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                UserName="burcubalci",
                Password="123456",
                ProfileImageFileName = "user.jpg",
                CreatedOn =DateTime.Now,
                ModifiedOn=DateTime.Now,
                ModifiedUserName="system",
            };

            MyNotesUser stdUser = new MyNotesUser()
            {
                Name = "Ozan",
                LastName = "Balci",
                Email = "ozibalc90@gmail.com",
                ActivateGuid=Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                UserName = "ozanbalci",
                Password = "789123",
                ProfileImageFileName = "user.jpg",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUserName = "system",
            };

            context.MyNotesUser.Add(admin);
            context.MyNotesUser.Add(stdUser);

            for(int i = 0; i < 8; i++)
            {
                MyNotesUser user = new MyNotesUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    LastName = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ActivateGuid=Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    UserName = $"user-{i}",
                    Password = "1234",
                    ProfileImageFileName = "user.jpg",
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1),DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUserName = $"user-{i}",
                };
                context.MyNotesUser.Add(user);
            }
            context.SaveChanges();//bunu yazmadanda ekliyor table 'a


            List<MyNotesUser> userList = context.MyNotesUser.ToList();
            for(int i = 0; i < 10; i++)
            {
                Category cat = new Category()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUserName = "system"
                };
                context.Categories.Add(cat);

                for(int j = 0; j < FakeData.NumberData.GetNumber(5, 9); j++) //herbir kayıt için 5 ile 9 arasında kayıt oluşturacak
                {
                    MyNotesUser owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                    Note note = new Note()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 20)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        IsDraft = false,
                        LikeCount = FakeData.NumberData.GetNumber(1, 9),
                        Owner = owner,
                        CreatedOn= FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn= FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUserName=owner.UserName
                    };
                    cat.Notes.Add(note);
                    for(int k = 0; k < FakeData.NumberData.GetNumber(3, 5); k++)
                    {
                        MyNotesUser comment_owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                        Comment comment = new Comment()
                        { 
                            Text=FakeData.TextData.GetSentence(),
                            Owner=comment_owner,
                            CreatedOn= FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn= FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUserName=comment_owner.UserName

                        };
                        note.Comments.Add(comment);
                    }
                    for(int k = 0; k < note.LikeCount; k++)
                    {
                        Liked liked = new Liked()
                        {
                            LikedUser = userList[k],

                        };
                        note.Likes.Add(liked);
                    }
                }

            }
            
            context.SaveChanges();


            //base.Seed(context);//databasei oluşturduktan sonra devreye girer ,context sınıfından seed doldurulacak


            //buranın çıkması için nugetten fake data katmana ekle sonra gelip bir sınıf oluştur
        }
    }
}
