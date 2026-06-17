using Microsoft.AspNetCore.Mvc;
using Summary.API.Extensions;
using Summary.API.Models;
using Summary.Core.Interfaces;
using Summary.Core.Models;

namespace Summary.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SummarizeController : ControllerBase
{
    private readonly ISummarizeService _summarizeService;

    public SummarizeController(ISummarizeService summarizeService)
    {
        _summarizeService = summarizeService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(GeneralResponseT<SummarizeResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GeneralErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(GeneralErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SummarizeAsync(
        [FromBody] GeneralRequestT<SummarizeRequest> request,
        CancellationToken cancellationToken)
    {
        var body = request.GetNotNullBody();

        var result = await _summarizeService.SummarizeAsync(body, cancellationToken);

        return Ok(new GeneralResponseT<SummarizeResponse> { Data = result });
    }

    [HttpPost("/[controller]")]
    [ProducesResponseType(typeof(GeneralResponseT<SummarizeResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GeneralErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(GeneralErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SummarizeRawStringAsync(
        [FromBody] string inputText,
        CancellationToken cancellationToken)
    {
        var body = new SummarizeRequest { InputText = inputText };

        var result = await _summarizeService.SummarizeAsync(body, cancellationToken);

        return Ok(new GeneralResponseT<SummarizeResponse> { Data = result });
    }
}
