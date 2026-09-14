using Immediate.Injections.Shared;
using System.IO.Compression;

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

        public static string CompressToBase64(string input)
        {
            byte[] raw = System.Text.Encoding.UTF8.GetBytes(input);

            using (var ms = new MemoryStream())
            {
                using (var gzip = new GZipStream(ms, CompressionMode.Compress))
                {
                    gzip.Write(raw, 0, raw.Length);
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }   
    }
}
