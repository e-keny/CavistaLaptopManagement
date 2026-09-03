using CavistaLaptopLifecycleManagement.Api.Features.Laptop.Models;
using CavistaLaptopLifecycleManagement.Api.Features.Laptop.Services;
using CavistaLaptopLifecycleManagement.Api.Features.Shared;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Mvc;

namespace CavistaLaptopLifecycleManagement.Api.Features.Laptop.Endpoints
{
    [Handler]
    [MapGet("")]
    [MapGroup<LaptopMapGroup>]
    public static partial class GetUserLaptops
    {
        public record Query([FromQuery] int? pageNumber, [FromQuery] int? pageSize);

        private async static ValueTask<PaginatedList<UserLaptop>> HandleAsync(
            Query request,
            UserLaptopService userLaptopService,
            CancellationToken token)
        {
            var userLaptop = await userLaptopService.GetUserLaptopsAsync(request.pageNumber ?? 1, request.pageSize ?? 10);

            return userLaptop;
        }
    }
}
