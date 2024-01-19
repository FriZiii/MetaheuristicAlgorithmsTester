using MediatR;
using MetaheuristicAlgorithmsTester.Application.Menagments.Reports.PdfReports.PdfReportOfMultipleAlgorithms;
using MetaheuristicAlgorithmsTester.Application.Menagments.Reports.PdfReports.PdfReportOfSingleAlgorithm;
using MetaheuristicAlgorithmsTester.Application.Menagments.Reports.TxtReports.TxtReportOfMultipleAlgorithms;
using MetaheuristicAlgorithmsTester.Application.Menagments.Reports.TxtReports.TxtReportOfSingleAlgorithm;
using Microsoft.AspNetCore.Mvc;

namespace MetaheuristicAlgorithmsTester.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportsController(IMediator mediator) : ControllerBase
    {
        [HttpGet("PDF/{id:int}")]
        public async Task<IActionResult> GetPdfReportOfSingleAlgorithm(int id)
        {
            var result = await mediator.Send(new PdfReportOfSingleAlgorithm() { ExecutedId = id });
            if (result.IsSuccesfull)
            {
                return File(result.FileContent, result.ContentType, result.FileName);
            }
            else
            {
                object response = new { IsSuccessful = result.IsSuccesfull, Message = result.Message };
                return BadRequest(response);
            }
        }

        [HttpGet("PDF/multiple")]
        public async Task<IActionResult> GetPdfReportOfMultipleAlgorithm([FromQuery] List<int> executedIds)
        {
            var result = await mediator.Send(new PdfReportOfMultipleAlgorithms() { ExecutedIds = executedIds });
            if (result.IsSuccesfull)
            {
                return File(result.FileContent, result.ContentType, result.FileName);
            }
            else
            {
                object response = new { IsSuccessful = result.IsSuccesfull, Message = result.Message };
                return BadRequest(response);
            }
        }

        [HttpGet("TXT/{id:int}")]
        public async Task<IActionResult> GetTxtReportOfSingleAlgorithm(int id)
        {
            var result = await mediator.Send(new TxtReportOfSingleAlgorithm() { ExecutedId = id});
            if (result.IsSuccesfull)
            {
                return File(result.FileContent, result.ContentType, result.FileName);
            }
            else
            {
                object response = new { IsSuccessful = result.IsSuccesfull, Message = result.Message };
                return BadRequest(response);
            }
        }

        [HttpGet("TXT/multiple")]
        public async Task<IActionResult> GetTxtReportOfSingleAlgorithm([FromQuery] List<int> algorithmIds)
        {
            var result = await mediator.Send(new TxtReportOfMultipleAlgorithms() { ExecutedIds = algorithmIds });
            if (result.IsSuccesfull)
            {
                return File(result.FileContent, result.ContentType, result.FileName);
            }
            else
            {
                object response = new { IsSuccessful = result.IsSuccesfull, Message = result.Message };
                return BadRequest(response);
            };
        }
    }
}
