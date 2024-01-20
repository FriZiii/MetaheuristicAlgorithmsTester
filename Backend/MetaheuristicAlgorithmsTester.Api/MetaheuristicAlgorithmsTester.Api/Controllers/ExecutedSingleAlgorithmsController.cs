using MediatR;
using MetaheuristicAlgorithmsTester.Application.Menagments.SingleExecutedAlgotrithms.DeleteExecutedAlgorithmById;
using MetaheuristicAlgorithmsTester.Application.Menagments.SingleExecutedAlgotrithms.GetAllExecutedAlgorithms;
using Microsoft.AspNetCore.Mvc;

namespace MetaheuristicAlgorithmsTester.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExecutedSingleAlgorithmsController(IMediator mediator) : ControllerBase
    {

        [HttpGet(Name = "GetExecutedSingleAlgorithms")]
        public async Task<IActionResult> GetExecutedSingleAlgorithms()
        {
            var result = await mediator.Send(new GetAllSingleExecutedAlgorithms());
            if (result.IsSuccesfull)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteSingleExecutedById(int id)
        {
            var result = await mediator.Send(new DeleteSingleExecutedAlgorithmById() { Id = id });
            if (result)
            {
                return Ok($"Executed algorithm with id {id} has been removed");
            }
            else
            {
                return BadRequest($"No executed algorithm found with id {id}");
            }
        }
    }
}
