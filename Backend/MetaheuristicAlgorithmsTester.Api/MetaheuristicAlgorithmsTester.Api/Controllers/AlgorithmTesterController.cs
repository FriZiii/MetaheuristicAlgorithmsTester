using MediatR;
using MetaheuristicAlgorithmsTester.Application.Menagments.AlgorithmsTests.TestMultipleAlgorithms;
using MetaheuristicAlgorithmsTester.Application.Menagments.AlgorithmsTests.TestSingleAlgorithm;
using Microsoft.AspNetCore.Mvc;

namespace MetaheuristicAlgorithmsTester.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlgorithmTesterController(IMediator mediator) : ControllerBase
    {
        [HttpPost("TestSingleAlgorithm")]
        public async Task<IActionResult> TestSingleAlgorithm(TestSingleAlgorithm testSingleAlgorithm)
        {
            var result = await mediator.Send(testSingleAlgorithm);
            if (result.IsSuccesfull)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("TestMultipleAlgorithms")]
        public async Task<IActionResult> TestMultipleAlgorithms(TestMultipleAlgorithms testMultipleAlgorithms)
        {
            var result = await mediator.Send(testMultipleAlgorithms);
            if (result.All(x => x.IsSuccesfull))
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
