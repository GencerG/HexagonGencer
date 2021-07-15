using HexagonGencer.Factory;
using HexagonGencer.Game.Controller.Abstract;
using HexagonGencer.Game.Core.Concrete;
using HexagonGencer.Game.Models.Concrete;
using HexagonGencer.Utils;
using System.Collections.Generic;
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

        private readonly List<Hexagon> _hexagonList = new List<Hexagon>();

        #endregion

        #region Scene Controller

        public override BoolReactiveProperty ShouldRenderNewScene { get; set; }
            = new BoolReactiveProperty(false);

        public override void InitializeScene()
        {
            InitializePool();
            InitializeHexagons();
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

        private void InitializeHexagons()
        {
            for (int i = 0; i < HexagonGencerUtils.BOARD_HEIGHT; ++i)
            {
                for (int j = 0; j < HexagonGencerUtils.BOARD_WIDTH; ++j)
                {
                    var hexPosition = HexagonGencerUtils.GetHexagonPosition(i, j);
                    var hexagonObjectInstance = _objectPool.GetFromPool();
                    hexagonObjectInstance.transform.position = hexPosition;

                    if (!hexagonObjectInstance.TryGetComponent<Hexagon>(out Hexagon hexagon)) return;
                    hexagon.SetRandomColor();
                    _hexagonList.Add(hexagon);
                }
            }
        }

        #endregion
    }
}
