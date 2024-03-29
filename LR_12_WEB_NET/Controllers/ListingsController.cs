﻿using LR_12_WEB_NET.ApiClient;
using LR_12_WEB_NET.Dto;
using LR_12_WEB_NET.Enums;
using LR_12_WEB_NET.Services;
using LR6_WEB_NET.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Serilog;


namespace LR_12_WEB_NET.Controllers;

[ApiController]
[Route("[controller]")]
public class ListingsController : ControllerBase
{
    private readonly IListingService _listingService;

    public ListingsController(IListingService listingService)
    {
        _listingService = listingService;
    }

    [HttpPost("latest")]
    public async Task<ResponseDto<GetLatestListingsResponse>> GetLatestListings([FromBody] GetLatestListingsDto dto)
    {
        var response = await _listingService.GetLatestListings(dto);
        Response.StatusCode = StatusCodes.Status200OK;
        return new ResponseDto<GetLatestListingsResponse>
        {
            StatusCode = StatusCodes.Status200OK,
            Values = new List<GetLatestListingsResponse> { response },
            Description = "Success",
            TotalRecords = 1
        };
    }
}