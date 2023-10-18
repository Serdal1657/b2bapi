using Business.Repositories.BasketRepository;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
   [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(Basket basket)
        {
            var result = await _basketService.Add(basket);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Update(Basket basket)
        {
            var result = await _basketService.Update(basket);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Delete(Basket basket)
        {
            var result = await _basketService.Delete(basket);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetList()
        {
            var result = await _basketService.GetList();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("[action]/CustomerId")]
        public async Task<IActionResult> GetListByCustomerId(int CustomerId)
        {
            var result = await _basketService.GetListByCustomerId(CustomerId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("[action]/CustomerId,ProductId")]
        public async Task<IActionResult> GetProductCustomerId(int CustomerId, int ProductId)
        {
            var result = await _basketService.GetProductCustomerId(CustomerId,ProductId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _basketService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

    }
}
