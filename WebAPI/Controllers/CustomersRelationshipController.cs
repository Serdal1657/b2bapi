using Business.Repositories.CustomersRelationshipRepository;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
   [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class CustomersRelationshipController : ControllerBase
    {
        private readonly ICustomersRelationshipService _customersRelationshipService;

        public CustomersRelationshipController(ICustomersRelationshipService customersRelationshipService)
        {
            _customersRelationshipService = customersRelationshipService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(CustomersRelationship customersRelationship)
        {
            var result = await _customersRelationshipService.Add(customersRelationship);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Update(CustomersRelationship customerRelationship)
        {
            var result = await _customersRelationshipService.Update(customerRelationship);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Delete(CustomersRelationship customersRelationship)
        {
            var result = await _customersRelationshipService.Delete(customersRelationship);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetList()
        {
            var result = await _customersRelationshipService.GetList();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _customersRelationshipService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

    }
}
