using ETrade.Core;
using ETrade.Dal;
using ETrade.Dto;
using ETrade.Entity.Concretes;
using ETrade.Repos.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETrade.Repos.Concretes
{
    public class BasketDetailRep<T> : BaseRepository<BasketDetail>, IBasketDetailRep where T : class
    {
        public BasketDetailRep(TradeContext db) : base(db)
        {
        }

        public List<BasketDetailDTO> BasketDetailDTOs(int MasterId)
        {
            return Set().Where(x => x.Id == MasterId).Select(x => new BasketDetailDTO
            {
                ProdcuctName = x.Products.ProductName,
                ProductId = x.ProductId,
                Id = x.Id,
                UnitName = x.Unit.Description,
                Amount = x.Amount,
                UnitPrice = x.UnitPrice,
                Vat = x.Ratio,
                Total = (x.UnitPrice * x.Amount) * ((x.Ratio/100) + 1)
            }).ToList();
        }
    }
}
