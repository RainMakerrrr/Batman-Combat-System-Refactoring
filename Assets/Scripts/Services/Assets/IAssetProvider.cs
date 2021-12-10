using UnityEngine;

namespace Services.Assets
{
    public interface IAssetProvider
    {
        T LoadData<T>(string path) where T : Object;
    }
}