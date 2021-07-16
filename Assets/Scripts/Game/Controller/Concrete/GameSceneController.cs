using DG.Tweening;
using HexagonGencer.Enums;
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

        private Camera _mainCam;

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
            DOTween.SetTweensCapacity(2000, 200);
            InitializePool();
            InitializeItems();
            InitializeOutline();
            InitializeDictionary();
            BindInputEvents();
            BindGridManager();
            BindUIManager();
            SetCameraBounds();
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
            for (int i = 0; i < HexagonGencerUtils.GameSettings.BOARD_HEIGHT; ++i)
            {
                for (int j = 0; j < HexagonGencerUtils.GameSettings.BOARD_WIDTH; ++j)
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

            CheckColorMatch();
        }

        private void InitializeOutline()
        {
            _outline = GameObject.Instantiate(AssetFactory
                .GetAsset(GameObjectAssetModel.OutlinePrefab));

            _outline.SetActive(false);
        }

        private void InitializeDictionary()
        {
            for (int i = 0; i < HexagonGencerUtils.GameSettings.BOARD_WIDTH; ++i)
            {
                _explodeInfo.Add(i, 0);
            }
        }

        private void CheckColorMatch()
        {
            foreach (Cell cell in _cellList)
            {
                var avaibleColors = new List<ItemColor>();

                for (int i = 0; i < 7; ++i)
                {
                    avaibleColors.Add((ItemColor)i);
                }

                var neighbours = cell.GetNeighbours();

                foreach (Cell neighbour in neighbours)
                {
                    if (neighbour == null)
                        continue;

                    if (cell.Item.ItemColor == neighbour.Item.ItemColor)
                    {
                        avaibleColors.Remove(cell.Item.ItemColor);
                        cell.Item.SetRandomColor();
                    }
                }
            }
        }

        private void SetCameraBounds()
        {
            _mainCam = Camera.main;

            var upperIndex = _cellList.Count - Mathf.CeilToInt(HexagonGencerUtils.GameSettings.BOARD_WIDTH / 2f);
            var upperCenter = (_cellList[upperIndex].transform.position + _cellList[upperIndex - 1].transform.position) / 2;
            var lowerIndex = HexagonGencerUtils.GameSettings.BOARD_WIDTH - (int)Mathf.Ceil(HexagonGencerUtils.GameSettings.BOARD_WIDTH / 2);
            var lowerCenter = (_cellList[lowerIndex].transform.position + _cellList[lowerIndex - 1].transform.position) / 2;

            var position = (upperCenter + lowerCenter) / 2;
            position.z = _mainCam.transform.position.z;

            _mainCam.transform.position = position;
            _mainCam.orthographicSize = upperCenter.y;
        }

        #endregion
    }
}
