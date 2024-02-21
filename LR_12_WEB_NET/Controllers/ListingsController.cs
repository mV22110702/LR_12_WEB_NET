using LR_12_WEB_NET.ApiClient;
using LR_12_WEB_NET.Dto;
using LR_12_WEB_NET.Enums;
using LR_12_WEB_NET.Services;
using Microsoft.AspNetCore.Mvc;

namespace LR_12_WEB_NET.Controllers;

[ApiController]
[Route("[controller]")]
public class ListingsController
{
    private readonly IListingService _listingService;

    public ListingsController(IListingService listingService)
    {
        _listingService = listingService;
    }

    [HttpGet("latest")]
    public async Task<GetLatestListingsResponse> GetLatestListings([FromQuery] string convert)
    {
        return await _listingService.GetLatestListings(new GetLatestListingsDto()
        {
            Convert = convert
        });
    }
}