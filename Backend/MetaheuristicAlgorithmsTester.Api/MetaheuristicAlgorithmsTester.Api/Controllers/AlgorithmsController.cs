using MediatR;
using MetaheuristicAlgorithmsTester.Application.Menagments.Algorithms.AddAlgorithm;
using MetaheuristicAlgorithmsTester.Application.Menagments.Algorithms.GetAlgorithmById;
using MetaheuristicAlgorithmsTester.Application.Menagments.Algorithms.GetAllAlgorithms;
using Microsoft.AspNetCore.Mvc;

namespace MetaheuristicAlgorithmsTester.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlgorithmsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;

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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAgorithmById(int id)
        {
            var result = await mediator.Send(new GetAlgorithmById() { Id = id });
            if (result.IsSuccesfull)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet(Name = "GetAlgorithms")]
        public async Task<IActionResult> GetAllAlgorithms()
        {
            var result = await mediator.Send(new GetAllAlgorithms());
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
