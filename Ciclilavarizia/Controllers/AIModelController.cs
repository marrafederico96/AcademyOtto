using AdventureWorks.Models.AIData.Dtos;
using AdventureWorks.Services.AISentiment;
using Microsoft.AspNetCore.Mvc;

namespace AdventureWorks.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AIModelController(ISentimentService sentimentService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<AIResponse>> Get([FromBody] AIResponse reviewData)
        {
            var result = await sentimentService.AnalyzeSentenceSentiment(reviewData.Response);
            return Ok(result);
        }
    }
}
