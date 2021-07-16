using HexagonGencer.Factory;
using HexagonGencer.Game.Controller.Abstract;
using HexagonGencer.Game.Core.Concrete;
using HexagonGencer.Game.Models.Abstract;
using HexagonGencer.Game.Models.Concrete;
using HexagonGencer.Utils;
using System;
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

        private readonly List<Cell> _cellList 
            = new List<Cell>();

        private Dictionary<int, int> _explodeInfo 
            = new Dictionary<int, int>();

        #endregion

        #region Subjects

        private Subject<Tuple<IItem, IItem, IItem>> _onStartRotating 
            = new Subject<Tuple<IItem, IItem, IItem>>();

        private Subject<List<Cell>> _onMatch 
            = new Subject<List<Cell>>();

        #endregion

        #region Scene Controller

        public override BoolReactiveProperty ShouldRenderNewScene { get; set; }
            = new BoolReactiveProperty(false);

        public override void InitializeScene()
        {
            InitializePool();
            InitializeItems();
            InitializeOutline();
            InitializeDictionary();
            BindInputEvents();
            BindGridManager();
            BindUIManager();
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

        private void InitializeItems()
        {
            for (int i = 0; i < HexagonGencerUtils.BOARD_HEIGHT; ++i)
            {
                for (int j = 0; j < HexagonGencerUtils.BOARD_WIDTH; ++j)
                {
                    var itemPosition = HexagonGencerUtils.GetItemPosition(i, j);

                    var cellObjectInstance = GameObject.Instantiate(AssetFactory.GetAsset(GameObjectAssetModel.CellPrefab));
                    cellObjectInstance.transform.position = itemPosition;

                    if (!cellObjectInstance.TryGetComponent<Cell>(out Cell cellInstance)) return;
                    _cellList.Add(cellInstance);

                    var hexInstance = _objectPool.GetFromPool();
                    hexInstance.transform.position = cellObjectInstance.transform.position;

                    if (!hexInstance.TryGetComponent<IItem>(out IItem item)) return;
                    cellInstance.UpdateHexagon(item);
                    item.SetRandomColor();
                    cellInstance.Row = i;
                    cellInstance.Column = j;
                    cellInstance.transform.name = cellInstance.Index.ToString();
                    hexInstance.transform.GetChild(0).GetComponent<TextMesh>().text = cellInstance.Index.ToString();
                }
            }

            foreach (Cell cell in _cellList)
            {
                cell.SetNeighbours(HexagonGencerUtils.GetNeighboursFromCell(cell, _cellList));
            }
        }

        private void InitializeOutline()
        {
            _outline = GameObject.Instantiate(AssetFactory
                .GetAsset(GameObjectAssetModel.OutlinePrefab));

            _outline.SetActive(false);
        }

        private void InitializeDictionary()
        {
            for (int i = 0; i < HexagonGencerUtils.BOARD_WIDTH; ++i)
            {
                _explodeInfo.Add(i, 0);
            }
        }

        #endregion
    }
}
