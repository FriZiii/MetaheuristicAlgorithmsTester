using MediatR;
using MetaheuristicAlgorithmsTester.Application.Menagments.MultipleExecutedAlgorithms.DeleteMultipleExecutedAlgorithms;
using MetaheuristicAlgorithmsTester.Application.Menagments.MultipleExecutedAlgorithms.GetAllMultipleExecutedAlgorithms;
using Microsoft.AspNetCore.Mvc;

namespace MetaheuristicAlgorithmsTester.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExecutedMultipleAlgorithmsController(IMediator mediator) : ControllerBase
    {
        [HttpGet(Name = "GetExecutedMultipleAlgorithms")]
        public async Task<IActionResult> GetExecutedSingleAlgorithms()
        {
            try
            {
                var result = await mediator.Send(new GetAllMultipleExecutedAlgorithms());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMultipleExecutedById([FromRoute] string id)
        {
            var result = await mediator.Send(new DeleteMultipleExecutedAlgorithms() { Id = id });
            if (result)
            {
                return Ok($"Executed multiple algorithm with id {id} has been removed");
            }
            else
            {
                return BadRequest($"No executed multiple algorithm found with id {id}");
            }
        }
    }
}
