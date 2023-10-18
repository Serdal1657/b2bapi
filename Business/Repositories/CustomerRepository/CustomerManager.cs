using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Repositories.CustomerRepository;
using Entities.Concrete;
using Business.Aspects.Secured;
using Core.Aspects.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.CustomerRepository;
using Entities.Dtos;
using Core.Utilities.Hashing;
using Business.Repositories.UserRepository;
using Core.Utilities.Business;
using Business.Repositories.CustomersRelationshipRepository;
using Castle.Core.Resource;
using Business.Repositories.OrderRepository;

namespace Business.Repositories.CustomerRepository
{
    public class CustomerManager : ICustomerService
    {
        private readonly ICustomerDal _customerDal;
        private readonly ICustomersRelationshipService _customerRelationshipService;
        private readonly IOrderService _orderService;

        public CustomerManager(ICustomerDal customerDal, ICustomersRelationshipService customerRelationshipService, IOrderService orderService)
        {
            _customerDal = customerDal;
            _customerRelationshipService = customerRelationshipService;
            _orderService = orderService;
        }

        [SecuredAspect()]
       [RemoveCacheAspect("Business.Repositories.CustomerRepository.ICustomerService.GetList")]
        public async Task<IResult> Add(CustomerRegisterDto customerRegisterDto)
        {
            IResult result = BusinessRules.Run(
               await CheckIfEmailExists(customerRegisterDto.Email)
               
               );

            if (result != null)
            {
                return result;
            }

            byte[] PasswordHash, PasswordSalt;
            HashingHelper.CreatePassword(customerRegisterDto.Password, out PasswordHash, out PasswordSalt);
            Customer customer = new Customer()
            {
                Id = 0,
                Email = customerRegisterDto.Email,
                Name = customerRegisterDto.Name,
                PasswordHash = PasswordHash,
                PasswordSalt = PasswordSalt,
                Status=customerRegisterDto.Status,


            };


            await _customerDal.Add(customer);
            return new SuccessResult("Kayıt Başarılı");
        }

        [SecuredAspect()]
       [RemoveCacheAspect("Business.Repositories.CustomerRepository.ICustomerService.GetList")]
        public async Task<IResult> Update(Customer customer)
        {
            await _customerDal.Update(customer);
            return new SuccessResult("Kayıt Güncellendi.");
        }
        [SecuredAspect()]
        [RemoveCacheAspect("ICustomerService.Get")]

        public async Task<IResult> Delete(Customer customer)
        {
            IResult result = BusinessRules.Run(
                await CheckIfCustomerOrderExist(customer.Id));

            if (result != null)
            {
                return result;
            }

            var customerRelationship = await _customerRelationshipService.GetByCustomerId(customer.Id);
            if (customerRelationship.Data != null)
            {
                await _customerRelationshipService.Delete(customerRelationship.Data);
            }

            await _customerDal.Delete(customer);
            return new SuccessResult("Müşteri kaydı silindi! ");
        }

        [SecuredAspect()]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<CustomerDto>>> GetList()
        {
            return new SuccessDataResult<List<CustomerDto>>(await _customerDal.GetListDto());
        }

        [SecuredAspect()]
        public async Task<IDataResult<Customer>> GetById(int id)
        {
            return new SuccessDataResult<Customer>(await _customerDal.Get(p => p.Id == id));
        }
        public async Task<Customer> GetByEmail(string email)
        {
            var result = await _customerDal.Get(p => p.Email == email);
            return result;
        }
        [SecuredAspect()]
        public async Task<IDataResult<CustomerDto>> GetDtoById(int id)
        {
            return new SuccessDataResult<CustomerDto>(await _customerDal.GetDto(id));
        }


        private async Task<IResult> CheckIfEmailExists(string email)
        {
            var list = await GetByEmail(email);
            if (list != null)
            {
                return new ErrorResult("Bu mail adresi daha önce kullanılmış");
            }
            return new SuccessResult();
        }

        public async Task<IResult> CheckIfCustomerOrderExist(int customerId)
        {
            var result = await _orderService.GetListByCustomerId (customerId);
            if (result.Data.Count > 0)
            {
                return new ErrorResult("Sipariş Kaydı bulunan müşteriyi silemezsiniz!");
            }
            return new SuccessResult();
        }

        [SecuredAspect()]
        public async Task<IResult> ChangePasswordByAdminPanel(CustomerChangePassworByAdminPanelDto customerDto)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePassword(customerDto.Password, out passwordHash, out passwordSalt);
            var customer = await _customerDal.Get(p => p.Id == customerDto.Id);
            customer.PasswordHash = passwordHash;
            customer.PasswordSalt = passwordSalt;

            await _customerDal.Update(customer);
            return new SuccessResult("Şifre Değiştirildi.");
        }


    }
}
