using Entities;

namespace Shared
{
    public class AssetMapper
    {
        public static AssetMinimalDto? Map(Asset? asset)
        {
            if (asset == null)
            {
                return null;
            }

            return new AssetMinimalDto(asset.Id, asset.Name);
        }
    }
}
