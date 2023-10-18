using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Repositories.ProductRepository;
using Entities.Concrete;
using Business.Aspects.Secured;
using Core.Aspects.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.ProductRepository;
using Entities.Dtos;
using DataAccess.Repositories.ProductImageRepository;
using Business.Repositories.ProductImageRepository;
using Business.Repositories.PriceListDetailRepository;

namespace Business.Repositories.ProductRepository
{
    public class ProductManager : IProductService
    {
        private readonly IProductDal _productDal;
        private readonly IProductImageService _productImageService;
        private readonly IPriceListDetailService _priceListDetailService;

      



        public ProductManager(IProductImageService productImageService, IPriceListDetailService priceListDetailService = null, IProductDal productDal = null)

        {
            _productImageService = productImageService;
            _priceListDetailService = priceListDetailService;
            _productDal = productDal;
        }

        [SecuredAspect("admin, product.add")]
       [RemoveCacheAspect("Business.Repositories.ProductRepository.IProductService.GetList")]
        public async Task<IResult> Add(Product product)
        {
            await _productDal.Add(product);
            return new SuccessResult("Kayıt Başarılı");
        }

        [SecuredAspect("admin, product.update")]
       [RemoveCacheAspect("Business.Repositories.ProductRepository.IProductService.GetList")]
        public async Task<IResult> Update(Product product)
        {
            await _productDal.Update(product);
            return new SuccessResult("Kayıt Güncellendi.");
        }

        [SecuredAspect("admin, product.delete")]
       [RemoveCacheAspect("Business.Repositories.ProductRepository.IProductService.GetList")]
        public async Task<IResult> Delete(Product product)
        {

            var images =  await _productImageService.GetListByProductId(product.Id);
            foreach (var image in images.Data)
            {
               await _productImageService.Delete(image);



            }

            var priceListProducts =await _productImageService.GetListByProductId(product.Id);
            foreach (var item in priceListProducts.Data)
            {
                await _productImageService.Delete(item);

            }


            await _productDal.Delete(product);
            return new SuccessResult("Kayıt Silindi");
        }

        [SecuredAspect("admin, product.get")]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<ProductListDto>>> GetList()
        {
            return new SuccessDataResult<List<ProductListDto>>(await _productDal.GetList());
        }
        //Task<IDataResult<List<ProductListDto>>> GetProductList(int CustomerId);

        //[SecuredAspect("admin, product.get")]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<ProductListDto>>> GetProductList(int CustomerId)
        {
            return new SuccessDataResult<List<ProductListDto>>(await _productDal.GetProductList(CustomerId));
        }





        [SecuredAspect("admin, product.get")]
        public async Task<IDataResult<Product>> GetById(int id)
        {
            return new SuccessDataResult<Product>(await _productDal.Get(p => p.Id == id));
        }

    }
}
