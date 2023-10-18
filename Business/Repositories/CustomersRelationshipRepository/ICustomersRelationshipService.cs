using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using Core.Utilities.Result.Abstract;

namespace Business.Repositories.CustomersRelationshipRepository
{
    public interface ICustomersRelationshipService
    {
        Task<IResult> Add(CustomersRelationship customersRelationship);
        Task<IResult> Update(CustomersRelationship customersRelationship);
        Task<IResult> Delete(CustomersRelationship customersRelationship);
        Task<IDataResult<List<CustomersRelationship>>> GetList();
        Task<IDataResult<CustomersRelationship>> GetById(int id);

        Task<IDataResult<CustomersRelationship>> GetByCustomerId(int customerId);
    }
}
