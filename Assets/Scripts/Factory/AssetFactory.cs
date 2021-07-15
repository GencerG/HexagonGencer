using HexagonGencer.Game.Models.Abstract;
using UnityEngine;

namespace HexagonGencer.Factory
{
    public static class AssetFactory
    {
        public static GameObject GetAsset(IAssetModel assetModel)
        {
            return Resources.Load(assetModel.Path) as GameObject;
        }
    }
}
