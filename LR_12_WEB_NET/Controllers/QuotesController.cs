using LR_12_WEB_NET.ApiClient;
using LR_12_WEB_NET.Dto;
using LR_12_WEB_NET.Enums;
using LR_12_WEB_NET.Services;
using LR_12_WEB_NET.Services.QuoteService;
using LR6_WEB_NET.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Serilog;


namespace LR_12_WEB_NET.Controllers;

[ApiController]
[Route("[controller]")]
public class QuotesController : ControllerBase
{
    private readonly IQuoteService _quoteService;

    public QuotesController(IQuoteService quoteService)
    {
        _quoteService = quoteService;
    }

    [HttpPost("latest")]
    public async Task<ResponseDto<GetLatestQuoteResponse>> GetLatestQuotes([FromBody] GetLatestQuoteDto dto)
    {
        var response = await _quoteService.GetLatestQuote(dto);
        Response.StatusCode = StatusCodes.Status200OK;
        return new ResponseDto<GetLatestQuoteResponse>
        {
            StatusCode = StatusCodes.Status200OK,
            Values = new List<GetLatestQuoteResponse> { response },
            Description = "Success",
            TotalRecords = 1
        };
    }
}