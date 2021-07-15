using HexagonGencer.Factory;
using HexagonGencer.Game.Controller.Abstract;
using HexagonGencer.Game.Models.Concrete;
using HexagonGencer.Utils;
using UniRx;
using UnityEngine;

namespace HexagonGencer.Game.Controller.Concrete
{
    public partial class GameSceneController : SceneController
    {
        #region Fields

        private ObjectPool _objectPool;

        private GameObject _poolContainer;
        private GameObject _outline;

        #endregion

        #region Scene Controller

        public override BoolReactiveProperty ShouldRenderNewScene { get; set; }
            = new BoolReactiveProperty(false);

        public override void InitializeScene()
        {

        }

        #endregion

        #region Custom Methods

        private void InitializePool()
        {
            var prefab = AssetFactory
                .GetAsset(GameObjectAssetModel.HexagonPrefab);

            _poolContainer = GameObject.Find("PoolContainer");
            _objectPool = new ObjectPool(prefab, _poolContainer.transform);
            _objectPool.InstantiatePool();
        }

        #endregion
    }
}
