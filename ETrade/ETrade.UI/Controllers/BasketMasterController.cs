using ETrade.Dto;
using ETrade.Entity.Concretes;
using ETrade.Uw;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ETrade.UI.Controllers
{
    public class BasketMasterController : Controller
    {
        IUow _uow;
        BasketMaster _basketMaster;
        public BasketMasterController(IUow uow, BasketMaster basketMaster)
        {
            _uow = uow;
            _basketMaster = basketMaster;
        }

        public IActionResult Create()
        {
            var user = JsonConvert.DeserializeObject<UserDTO>(HttpContext.Session.GetString("User"));
            // Hangi kullanıcını ekleme yaptığını anlamak için get session kullanıyoruz ve bu sayede de Id'yi kullanmış oluyoruz.
            var incompletedMaster = _uow._basketMasterRep.Set().FirstOrDefault(x => x.Completed == false && x.EntityId == user.Id);
            // Burada hali hazırda oluşturulmuş bir sepet var mı diye kontrol gerçekleştiriyoruz. Eğer tamamlanmamış bir sepet varsa eklemeyi onun üzerine yapacak. Eğer hazırda bir sepet yok ise de yeni sepet ekleyip, ürün eklemesini oraya yapacağız.
            if (incompletedMaster != null)
            {
                return RedirectToAction("Add", "BasketDetail", new { id = incompletedMaster.Id});
            }
            else
            {
                _basketMaster.OrderDate = DateTime.Now;
                _basketMaster.EntityId = user.Id;
                _uow._basketMasterRep.Add(_basketMaster);
                return RedirectToAction("Add", "BasketDetail", new { id = _basketMaster.Id });
                _uow.Commit();
            }
        }
    }
}
