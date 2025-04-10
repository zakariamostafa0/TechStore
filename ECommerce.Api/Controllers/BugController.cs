using ECommerce.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    public class BugController : BaseController
    {
        public BugController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        [HttpGet("not-found")]
        public async Task<IActionResult> GetNotFound()
        {
            var thing = await _unitOfWork.ProductRepository.GetByIdAsync(100);
            if (thing == null)
                return NotFound();
            return Ok(thing);
        }
        [HttpGet("server-error")]
        public async Task<IActionResult> GetServerError()
        {
            var thing = await _unitOfWork.ProductRepository.GetByIdAsync(100);
            thing.Name = "";
            return Ok(thing);
        }
        [HttpGet("bad-req>uest")]
        public async Task<IActionResult> GetBadRequest()
        {
            return Ok();
        }
        [HttpGet("bad-request/{id}")]
        public async Task<IActionResult> GetBadRequest(int id)
        {
            return Ok();
        }
    }
}
