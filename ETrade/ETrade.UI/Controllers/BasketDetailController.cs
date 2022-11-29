using ETrade.Entity.Concretes;
using ETrade.UI.Models;
using ETrade.Uw;
using Microsoft.AspNetCore.Mvc;

namespace ETrade.UI.Controllers
{
    public class BasketDetailController : Controller
    {
        IUow _uow;
        BasketDetail _basketDetail;
        BasketDetailModel _basketDetailModel;
        public BasketDetailController(IUow uow, BasketDetail basketDetail, BasketDetailModel basketDetailModel)
        {
            _uow = uow;
            _basketDetail = basketDetail;
            _basketDetailModel = basketDetailModel;
        }

        public IActionResult Add(int id)
        {
            _basketDetailModel.ProductsDTO = _uow._productsRep.GetProductsSelect();
            _basketDetailModel.BasketDetailDTO = _uow._basketDetailRep.BasketDetailDTOs(id);
            return View(_basketDetailModel);
        }
        [HttpPost]
        public IActionResult Add(BasketDetailModel m, int id)
        {
            Products products = _uow._productsRep.FindWithVat(m.ProductId);
            _basketDetail.Amount = m.Amount;
            _basketDetail.ProductId = m.ProductId;
            _basketDetail.Id = id;
            _basketDetail.UnitId = products.UnitId;
            _basketDetail.Ratio = products.Vat.Ratio;
            _basketDetail.UnitPrice = products.UnitPrice;
            _uow._basketDetailRep.Add(_basketDetail);
            _uow.Commit();
            return RedirectToAction("Add", new { id }); // Ekleme işlemi bittikten sonra tekrardan aynı Id üzerinden eklemeye devam edebilmek için return de Id yi de gönderiyoruz.
        }

        //---------------------------------------------------

        /*
        public IActionResult Add(int id)
        {
            _basketDetailModel.ProductsDTO = _uow._productsRep.GetProductsSelect();
            _basketDetailModel.BasketDetailDTO = _uow._basketDetailRep.BasketDetailDTOs(id);
            return View(_basketDetailModel);
        }
        [HttpPost]
        public IActionResult Add(BasketDetailModel m, int id)
        {
            Products products = _uow._productsRep.FindWithVat(m.ProductId);
            _basketDetail.Amount = m.Amount;
            _basketDetail.ProductId = m.ProductId;
            _basketDetail.Id = id;
            _basketDetail.UnitId = products.UnitId;
            _basketDetail.Ratio = products.Vat.Ratio;
            _basketDetail.UnitPrice = products.UnitPrice;
            _uow._basketDetailRep.Add(_basketDetail);
            _uow.Commit();
            return RedirectToAction("Add", new { id }); // Ekleme işlemi bittikten sonra tekrardan aynı Id üzerinden eklemeye devam edebilmek için return de Id yi de gönderiyoruz.
        }
        */

        public IActionResult Delete(int Id, int productId)
        {
            _uow._basketDetailRep.Delete(Id, productId);
            _uow.Commit();
            return RedirectToAction("Add", new { Id });
        }

        public IActionResult Update(int Id, int productId)
        {
            var selectedBDetail = _uow._basketDetailRep.Find(Id, productId);
            return View(selectedBDetail);
        }
        [HttpPost]
        public IActionResult Update(int Amount, int Id, int productId)
        {
            var selectedBDetail = _uow._basketDetailRep.Find(Id, productId);
            selectedBDetail.Amount = Amount;
            _uow._basketDetailRep.Update(selectedBDetail);
            _uow.Commit();
            return RedirectToAction("Add", new { Id });
        }
    }
}
