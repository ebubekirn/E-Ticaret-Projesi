using ETrade.Entity.Concretes;
using ETrade.UI.Models;
using ETrade.Uw;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ETrade.UI.Controllers
{
    public class AuthController : Controller
    {
        IUow _uow;
        UsersModel _model;
        public AuthController(IUow uow, UsersModel model)
        {
            _uow = uow;
            _model = model;
        }

        public IActionResult Register()
        {
            _model.Users = new Users();
            _model.Counties = _uow._countyRep.List();
            return View(_model);
        }
        [HttpPost]
        public IActionResult Register(UsersModel model)
        {
            model.Users = _uow._usersRep.CreateUser(model.Users);
            if (model.Users.Error == true)
            {
                model.Counties = _uow._countyRep.List();
                model.Error = $"{model.Users.Mail} Kullanıcısı Mevcut !!!";
                return View(model);
                //return RedirectToAction("Error", "Home", new { Msg = $"{model.Users.Mail} Kullanıcısı Mevcut !!!" }); // Bu şekilde hata yeni pencerede görürüz. Yukarıdakş şekilde ise ekleme ekranında hata alırız.           
            }
            else
            {
                model.Users.Role = "User";
                _uow._usersRep.Add(model.Users);
                _uow.Commit();
                return RedirectToAction("Success", "Home", new {Msg = $"{model.Users.Mail} Adlı Kullanıcı Girişi Başarılı !"});
                // Sadece Success yazarsam bu sayfada index arar. Başka bir controllerdaki indexe gitmek istersem virgülden sonra gideceğim kontrollerin adını yazarız.
            }
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string Mail, string Password)
        {
            var user = _uow._usersRep.Login(Mail, Password);
            if (user.Error == false)
            {
                HttpContext.Session.SetString("User", JsonConvert.SerializeObject(user));
                if (user.Role == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Home");
        }
    }
}
