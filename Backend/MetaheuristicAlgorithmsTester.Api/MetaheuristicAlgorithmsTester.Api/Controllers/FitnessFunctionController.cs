using MediatR;
using MetaheuristicAlgorithmsTester.Application.Menagments.FitnessFunctions.AddFitnessFunction;
using MetaheuristicAlgorithmsTester.Application.Menagments.FitnessFunctions.GetAllFitnessFunctions;
using MetaheuristicAlgorithmsTester.Application.Menagments.FitnessFunctions.GetFitnessFunctionById;
using Microsoft.AspNetCore.Mvc;

namespace MetaheuristicAlgorithmsTester.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FitnessFunctionController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;

        [HttpPost(Name = "PostFitnessFunction")]
        public async Task<IActionResult> PostFitnessFunction(AddFitnessFunction addFitnessFunction)
        {
            var result = await mediator.Send(addFitnessFunction);
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
        public async Task<IActionResult> GetFitnessFunctionById(int id)
        {
            var result = await mediator.Send(new GetFitnessFunctionById() { Id = id });
            if (result.IsSuccesfull)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet(Name = "GetFitnessFunctions")]
        public async Task<IActionResult> GetAllFitnessFunctions()
        {
            var result = await mediator.Send(new GetAllFitnessFunctions());
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
