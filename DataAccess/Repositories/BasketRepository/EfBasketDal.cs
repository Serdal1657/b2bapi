using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Repositories.BasketRepository;
using DataAccess.Context.EntityFramework;
using Entities.Dtos;
using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace DataAccess.Repositories.BasketRepository
{
    public class EfBasketDal : EfEntityRepositoryBase<Basket, SimpleContextDb>, IBasketDal

    {
        public async Task <List<basketListDto>> GetListByCustomerId(int CustomerId)
        {
            using (var contex = new SimpleContextDb())
            {
                var result = from Basket in contex.Baskets.Where(p => p.CustomerId == CustomerId)
                             join Product in contex.Products on Basket.ProductId equals Product.Id
                             select new basketListDto
                             {
                                 Id = Basket.Id,
                                 CustomerId = Basket.CustomerId,
                                 ProductId = Basket.ProductId,
                                 ProductName = Product.Name,
                                 Price = Basket.Price,
                                 Quantity = Basket.Quantity,
                                 Total = Basket.Price * Basket.Quantity
                             };

                return await result.ToListAsync();


            }
        }


        public async Task<basketListDto> GetProductCustomerId(int customerId, int productId)
        {
            using (var context = new SimpleContextDb())
            {
                var result = from CustomersRelationship in context.CustomersRelationships.Where(p => p.CustomerId == customerId)
                             join priceListDetail in context.PriceListDetails on CustomersRelationship.PriceListId equals priceListDetail.PriceListId
                             select new basketListDto
                             {
                                 Id = CustomersRelationship.Id,
                                 CustomerId=CustomersRelationship.CustomerId,
                                 ProductId=productId,
                                  
                                 Discount = CustomersRelationship.Discount,
                                 //Price = priceListDetail.Price,
                                 Price = context.PriceListDetails.Where(p => p.PriceListId == CustomersRelationship.PriceListId && p.ProductId == productId).Select(s => s.Price).FirstOrDefault(),
                                Total = context.PriceListDetails.Where(p => p.PriceListId == CustomersRelationship.PriceListId && p.ProductId == productId).Select(s => s.Price).FirstOrDefault()
                                     -
                                     context.PriceListDetails.Where(p => p.PriceListId == CustomersRelationship.PriceListId && p.ProductId == productId).Select(s => s.Price).FirstOrDefault()
                                        * CustomersRelationship.Discount/100
                             };

                return await result.FirstOrDefaultAsync();
            }
        }

        
    }
}
