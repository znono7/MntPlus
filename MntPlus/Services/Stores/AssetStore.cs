using Shared;

namespace MntPlus.WPF
{
    public class AssetStore
    {
        public event Action<AssetDto?>? AssetCreated;
        public event Action<AssetDto?>? AssetUpdated;
        public void CreateAsset(AssetDto? equipment)
        {
            AssetCreated?.Invoke(equipment);
        }

        public void UpdateAsset(AssetDto? equipment)
        {
            AssetUpdated?.Invoke(equipment);
        }
    }
}
