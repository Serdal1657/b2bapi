using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Repositories.CustomersRelationshipRepository;
using DataAccess.Context.EntityFramework;

namespace DataAccess.Repositories.CustomersRelationshipRepository
{
    public class EfCustomersRelationshipDal : EfEntityRepositoryBase<CustomersRelationship, SimpleContextDb>, ICustomersRelationshipDal
    {
    }
}
