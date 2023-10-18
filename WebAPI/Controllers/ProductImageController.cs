using Business.Repositories.ProductImageRepository;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Entities.Dtos;
using System.Net;

namespace WebApi.Controllers
{
   [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageService _productImageService;

        public ProductImageController(IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add([FromForm] ProductImageAddDto productImageAddDto)
        {
            var result = await _productImageService.Add(productImageAddDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Update([FromForm] ProductImageUpdateDto productImageUpdateDto)
        {
            var result = await _productImageService.Update(productImageUpdateDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Delete(ProductImage productImage)
        {
            var result = await _productImageService.Delete(productImage);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetList()
        {
            var result = await _productImageService.GetList();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }


        [HttpGet("[action]/{productId}")]
        public async Task<IActionResult> GetListByProductId(int productId)
        {
            var result = await _productImageService.GetListByProductId(productId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _productImageService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }


        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> SetMainImage(int id)
        {
            var result = await _productImageService.SetMainImage(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

       

    }
}
