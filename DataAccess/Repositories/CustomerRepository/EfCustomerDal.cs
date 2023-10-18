using Core.DataAccess.EntityFramework;
using DataAccess.Context.EntityFramework;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.CustomerRepository
{
    public class EfCustomerDal : EfEntityRepositoryBase<Customer, SimpleContextDb>, ICustomerDal
    {
        public async Task<List<CustomerDto>> GetListDto()
        {
            using (var context = new SimpleContextDb())
            {
                var result = from customer in context.Customers
                             select new CustomerDto
                             {
                                 Id = customer.Id,
                                 Email = customer.Email,
                                 Name = customer.Name,
                                 Status=customer.Status,
                                 PasswordHash = customer.PasswordHash,
                                 PasswordSalt = customer.PasswordSalt,
                                 Discount =
                                 (context.CustomersRelationships.Where(p => p.CustomerId == customer.Id) != null
                                 ? context.CustomersRelationships.Where(p => p.CustomerId == customer.Id).Select(s => s.Discount).FirstOrDefault()
                                : 0),
                                 PriceListId =
                                 (context.CustomersRelationships.Where(p => p.CustomerId == customer.Id) != null
                                ? context.CustomersRelationships.Where(p => p.CustomerId == customer.Id).Select(s => s.PriceListId).FirstOrDefault()
                                : 0),
                                 PriceListName =
                                 (context.CustomersRelationships.Where(p => p.CustomerId == customer.Id) != null
                                 ? context.PriceLists.Where(p => p.Id == (context.CustomersRelationships.Where(p => p.CustomerId == customer.Id).Select(s => s.PriceListId).FirstOrDefault())).Select(s => s.Name).FirstOrDefault()
                                 : "")
                             };
                return await result.OrderBy(p => p.Name).ToListAsync();
            }
        }

        public async Task<CustomerDto> GetDto(int id)
        {
            using (var context = new SimpleContextDb())
            {
                var result = from customer in context.Customers.Where(p => p.Id == id)
                             select new CustomerDto
                             {
                                 Id = customer.Id,
                                 Email = customer.Email,
                                 Name = customer.Name,
                                 Status=customer.Status,
                                 PasswordHash = customer.PasswordHash,
                                 PasswordSalt = customer.PasswordSalt,
                                 Discount =
                                 (context.CustomersRelationships.Where(p => p.CustomerId == customer.Id) != null
                                 ? context.CustomersRelationships.Where(p => p.CustomerId == customer.Id).Select(s => s.Discount).FirstOrDefault()
                                : 0),
                                 PriceListId =
                                 (context.CustomersRelationships.Where(p => p.CustomerId == customer.Id) != null
                                ? context.CustomersRelationships.Where(p => p.CustomerId == customer.Id).Select(s => s.PriceListId).FirstOrDefault()
                                : 0),
                                 PriceListName =
                                 (context.CustomersRelationships.Where(p => p.CustomerId == customer.Id) != null
                                 ? context.PriceLists.Where(p => p.Id == (context.CustomersRelationships.Where(p => p.CustomerId == customer.Id).Select(s => s.PriceListId).FirstOrDefault())).Select(s => s.Name).FirstOrDefault()
                                 : "")
                             };
                return await result.FirstOrDefaultAsync();
            }
        }
    }
}
