using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Repositories.CustomersRelationshipRepository;
using Entities.Concrete;
using Business.Aspects.Secured;
using Core.Aspects.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.CustomersRelationshipRepository;

namespace Business.Repositories.CustomersRelationshipRepository
{
    public class CustomersRelationshipManager : ICustomersRelationshipService
    {
        private readonly ICustomersRelationshipDal _customersRelationshipDal;
        

        public CustomersRelationshipManager(ICustomersRelationshipDal customersRelationshipDal)
        {
            _customersRelationshipDal = customersRelationshipDal;
        }

        [SecuredAspect()]
       [RemoveCacheAspect("Business.Repositories.CustomersRelationshipRepository.ICustomersRelationshipService.GetList")]
        public async Task<IResult> Add(CustomersRelationship customersRelationship)
        {
            await _customersRelationshipDal.Add(customersRelationship);
            return new SuccessResult("Kayıt Başarılı");
        }


        [SecuredAspect()]
       // [ValidationAspect(typeof(CustomerRelationshipValidator))]
        [RemoveCacheAspect("ICustomerRelationshipService.Get")]
        [RemoveCacheAspect("ICustomerService.Get")]

        public async Task<IResult> Update(CustomersRelationship customerRelationship)
        {
            var result = await _customersRelationshipDal.Get(p => p.CustomerId == customerRelationship.CustomerId);
            if (result != null)
            {
                customerRelationship.Id = result.Id;
                await _customersRelationshipDal.Update(customerRelationship);
            }
            else
            {
                await _customersRelationshipDal.Add(customerRelationship);
            }

            return new SuccessResult("KAYIT GÜNCELLENDİ...");
        }


        [SecuredAspect()]
       [RemoveCacheAspect("Business.Repositories.CustomersRelationshipRepository.ICustomersRelationshipService.GetList")]
        public async Task<IResult> Delete(CustomersRelationship customersRelationship)
        {
            await _customersRelationshipDal.Delete(customersRelationship);
            return new SuccessResult("Kayıt Silindi");
        }

        [SecuredAspect()]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<CustomersRelationship>>> GetList()
        {
            return new SuccessDataResult<List<CustomersRelationship>>(await _customersRelationshipDal.GetAll());
        }

        [SecuredAspect()]
        public async Task<IDataResult<CustomersRelationship>> GetById(int id)
        {
            return new SuccessDataResult<CustomersRelationship>(await _customersRelationshipDal.Get(p => p.Id == id));
        }

        public async Task<IDataResult<CustomersRelationship>> GetByCustomerId(int customerId)
        {

            return new SuccessDataResult<CustomersRelationship>(await _customersRelationshipDal.Get(p => p.CustomerId == customerId));
        }
    }
}
