using MetaheuristicAlgorithmsTester.Application.Menagments.Algorithms.AddAlgorithm;
using Microsoft.AspNetCore.Mvc;

namespace MetaheuristicAlgorithmsTester.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlgorithmsController : ControllerBase
    {
        public AlgorithmsController()
        { }

        [HttpPost(Name = "PostAlgorithm")]
        public IActionResult PostAgorithm(AddAlgorithm addAlgorithm)
        {
            return Ok();
        }
    }
}
