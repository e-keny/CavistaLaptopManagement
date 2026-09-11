using Immediate.Injections.Shared;

namespace CavistaLaptopLifecycleManagement.Api.Features.Shared.Services
{
    public static class UtilityService
    {
        public static string GenerateHybridId(string prefix)
        {
            string randomPart = Guid.NewGuid().ToString("N").Substring(0, 6);
            string shortTime = DateTime.UtcNow.Ticks.ToString().Substring(10, 6);
            return $"{prefix}_{shortTime}{randomPart}";
        }
    }
}
