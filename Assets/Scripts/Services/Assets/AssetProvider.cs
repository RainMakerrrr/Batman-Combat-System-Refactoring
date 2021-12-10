using UnityEngine;

namespace Services.Assets
{
    public class AssetProvider : IAssetProvider
    {
        public T LoadData<T>(string path) where T : Object => Resources.Load<T>(path);
    }
}