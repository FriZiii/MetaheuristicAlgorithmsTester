using MediatR;
using MetaheuristicAlgorithmsTester.Application.Menagments.Algorithms.AddAlgorithm;
using Microsoft.AspNetCore.Mvc;

namespace MetaheuristicAlgorithmsTester.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlgorithmsController : ControllerBase
    {
        private readonly IMediator mediator;

        public AlgorithmsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost(Name = "PostAlgorithm")]
        public async Task<IActionResult> PostAgorithm(AddAlgorithm addAlgorithm)
        {
            var result = await mediator.Send(addAlgorithm);
            if (result.IsSuccesfull)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet(Name = "PostAlgorithm")]
        public async Task<IActionResult> GetAgorithmById(GetAlgorithm getAlgorithm)
        {
            var result = await mediator.Send(getAlgorithm);
            if (result.IsSuccesfull)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
