using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Repositories.ProductImageRepository;
using Entities.Concrete;
using Business.Aspects.Secured;
using Core.Aspects.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.ProductImageRepository;
using Business.Abstract;
using Entities.Dtos;
using Core.Utilities.Business;
using System.Runtime.InteropServices;
using Business.Repositories.PriceListDetailRepository;
using System.Net;
using System.IO;
using System.Net.Http.Headers;

namespace Business.Repositories.ProductImageRepository
{
    public class ProductImageManager : IProductImageService
    {
        private readonly IProductImageDal _productImageDal;
        private readonly IFileService _fileService;

       


        public ProductImageManager(IProductImageDal productImageDal, IFileService fileService, IPriceListDetailService priceListDetailService = null)
        {
            _productImageDal = productImageDal;
            this._fileService = fileService;
            
        }

        [SecuredAspect()]
        [RemoveCacheAspect("Business.Repositories.ProductImageRepository.IProductImageService.GetList")]
        public async Task<IResult> Add(ProductImageAddDto productImageAddDto)
        {
            foreach (var image in productImageAddDto.Images)
            {

            IResult result = BusinessRules.Run(
               
                CheckIfImageExtesionsAllow(image.FileName),
                CheckIfImageSizeIsLessThanOneMb(image.Length)
                );

            if (result == null)
            {
               // string filename = _fileService.FileSaveToServer(image, "C:/B2Bfrontend/src/assets/img/");
                string filename = _fileService.FileSaveToFtp(image);
                    


                    ProductImage productImage = new ProductImage()
            {
                Id = 0,
                ProductId = productImageAddDto.ProductId,
                ImageUrl = filename,
                IsMainImage = false
                
            };

            await _productImageDal.Add(productImage);

            }

            
            }


            return new SuccessResult("Kayıt Başarılı");
        }

       [SecuredAspect()]
       [RemoveCacheAspect("Business.Repositories.ProductImageRepository.IProductImageService.GetList")]
        public async Task<IResult> Update(ProductImageUpdateDto productImageUpdateDto)
        {
            IResult result = BusinessRules.Run(

               CheckIfImageExtesionsAllow(productImageUpdateDto.Image.FileName),
               CheckIfImageSizeIsLessThanOneMb(productImageUpdateDto.Image.Length)
               );

            if (result != null)
            {
                return result;
            }
            try
            {
                if (System.IO.File.Exists(@"C:/B2Bfrontend/src/assets/img/" + productImageUpdateDto.ImageUrl) ) 
                {
                    System.IO.File.Delete(@"C:/B2Bfrontend/src/assets/img/" + productImageUpdateDto.ImageUrl);
            
                }
            }
            catch (Exception)
            {

                
            }
            string path = @"C:/B2Bfrontend/src/assets/img/" + productImageUpdateDto.ImageUrl;

            _fileService.FileDeleteToServer(path);

           string filename = _fileService.FileSaveToServer(productImageUpdateDto.Image, "C:/B2Bfrontend/src/assets/img/");
           
            ProductImage productImage = new ProductImage()
            {
                Id = productImageUpdateDto.Id,
                ImageUrl = filename,
                ProductId = productImageUpdateDto.ProductId,
                IsMainImage=productImageUpdateDto.IsMainImage
                
            };


            await _productImageDal.Update(productImage);
            return new SuccessResult("Kayıt Güncellendi.");
        }

        [SecuredAspect()]
       [RemoveCacheAspect("Business.Repositories.ProductImageRepository.IProductImageService.GetList")]
        public async Task<IResult> Delete(ProductImage productImage)
        {
            //string path = @"C:/B2Bfrontend/src/assets/img" + productImage.ImageUrl;
            string path =  productImage.ImageUrl;

           // _fileService.FileDeleteToServer(path);
            _fileService.FileDeleteToFtp(path);


            await _productImageDal.Delete(productImage);
            return new SuccessResult("Kayıt Silindi");
        }

        [SecuredAspect()]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<ProductImage>>> GetList()
        {
            return new SuccessDataResult<List<ProductImage>>(await _productImageDal.GetAll());
        }

       [SecuredAspect()]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<ProductImage>>> GetListByProductId(int productId)
        {
            return new SuccessDataResult<List<ProductImage>>(await _productImageDal.GetAll(p => p.ProductId == productId));
        }



        [SecuredAspect()]
        public async Task<IDataResult<ProductImage>> GetById(int id)
        {
            return new SuccessDataResult<ProductImage>(await _productImageDal.Get(p => p.Id == id));
        }
        private IResult CheckIfImageSizeIsLessThanOneMb(long imgSize)
        {
            decimal imgMbSize = Convert.ToDecimal(imgSize * 0.000001);
            if (imgMbSize > 5)
            {
                return new ErrorResult("Yüklediğiniz resmi boyutu en fazla 1mb olmalıdır");
            }
            return new SuccessResult();
        }

        private IResult CheckIfImageExtesionsAllow(string fileName)
        {
            var ext = fileName.Substring(fileName.LastIndexOf('.'));
            var extension = ext.ToLower();
            List<string> AllowFileExtensions = new List<string> { ".jpg", ".jpeg", ".gif", ".png" };
            if (!AllowFileExtensions.Contains(extension))
            {
                return new ErrorResult("Eklediğiniz resim .jpg, .jpeg, .gif, .png türlerinden biri olmalıdır!");
            }
            return new SuccessResult();
        }

        [SecuredAspect()]
        [RemoveCacheAspect("IProductImageService.Get")]
        [RemoveCacheAspect("IProductService.Get")]
        public async Task<IResult> SetMainImage(int Id)
        {
            var ProductImage = await _productImageDal.Get(p => p.Id == Id);
            var ProductImages = await _productImageDal.GetAll(p => p.ProductId == ProductImage.ProductId);
            foreach (var item in ProductImages)
            {
                item.IsMainImage = false;
                await _productImageDal.Update(item);

            }
            ProductImage.IsMainImage = true;
            await _productImageDal.Update(ProductImage);
            return new SuccessResult("Ana Resim Başarıyla Güncellendi.");
        }
    }
}
