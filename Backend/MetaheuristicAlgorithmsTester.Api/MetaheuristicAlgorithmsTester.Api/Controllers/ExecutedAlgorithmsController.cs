using MediatR;
using MetaheuristicAlgorithmsTester.Application.Menagments.ExecutedAlgotrithms.DeleteExecutedAlgorithmById;
using MetaheuristicAlgorithmsTester.Application.Menagments.ExecutedAlgotrithms.GetAllExecutedAlgorithms;
using Microsoft.AspNetCore.Mvc;

namespace MetaheuristicAlgorithmsTester.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExecutedAlgorithmsController(IMediator mediator) : ControllerBase
    {

        [HttpGet(Name = "GetExecutedAlgorithms")]
        public async Task<IActionResult> GetExecutedAlgorithms()
        {
            var result = await mediator.Send(new GetAllExecutedAlgorithms());
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
        public async Task<IActionResult> DeleteById(int id)
        {
            var result = await mediator.Send(new DeleteExecutedAlgorithmById() { Id = id });
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
