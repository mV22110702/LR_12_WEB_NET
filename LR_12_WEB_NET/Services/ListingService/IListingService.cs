using LR_12_WEB_NET.ApiClient;
using LR_12_WEB_NET.Dto;

namespace LR_12_WEB_NET.Services;

public interface IListingService
{
    public Task<GetLatestListingsResponse> GetLatestListings(GetLatestListingsDto options);
}