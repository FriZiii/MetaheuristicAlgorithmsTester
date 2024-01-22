using MediatR;
using MetaheuristicAlgorithmsTester.Application.Menagments.Instruction.GetDll;
using MetaheuristicAlgorithmsTester.Application.Menagments.Instruction.GetInstruction;
using Microsoft.AspNetCore.Mvc;

namespace MetaheuristicAlgorithmsTester.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InstructionController(IMediator mediator) : ControllerBase
    {
        [HttpGet("GetInstructionPDF")]
        public async Task<IActionResult> GetInstructionPDF()
        {
            try
            {
                var result = await mediator.Send(new GetInstruction());
                return File(result.FileContent, result.ContentType, result.FileName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetDLLFile")]
        public async Task<IActionResult> GetDLLFile()
        {
            try
            {
                var result = await mediator.Send(new GetDll());
                return File(result.FileContent, result.ContentType, result.FileName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
