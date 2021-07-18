using DG.Tweening;
using HexagonGencer.Enums;
using HexagonGencer.Factory;
using HexagonGencer.Game.Core.Abstract;
using HexagonGencer.Game.Core.Concrete;
using HexagonGencer.Game.Models.Concrete;
using HexagonGencer.Utils;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HexagonGencer.Game.Controller.Concrete
{
    public partial class GameSceneController : MonoBehaviour
    {
        #region Fields

        private GameObject _hexagonPrefab;
        private GameObject _bombPrefab;
        private GameObject _poolContainer;
        private GameObject _outline;

        private Camera _mainCam;

        private readonly List<Cell> _cellList
            = new List<Cell>();

        private readonly List<GameObject> _bombList
            = new List<GameObject>();

        private Dictionary<int, int> _explodeInfo
            = new Dictionary<int, int>();

        private CompositeDisposable _disposables = new CompositeDisposable();


        #endregion

        #region Subjects

        private Subject<Tuple<Item, Item, Item>> _onStartRotating
            = new Subject<Tuple<Item, Item, Item>>();

        private Subject<List<Cell>> _onMatch
            = new Subject<List<Cell>>();

        #endregion

        #region Unity

        private void Start()
        {
            DOTween.SetTweensCapacity(2000, 200);
            BindInputEvents();
            BindGridManager();
            BindUIManager();
        }

        private void OnDestroy()
        {
            DisposeAll();
        }

        #endregion

        #region Custom Methods

        private void InitializeBoard()
        {
            InitializePool();
            InitializeItems();
            InitializeOutline();
            InitializeDictionary();
            SetCameraBounds();
        }

        public void InitializePool()
        {
            _hexagonPrefab = AssetFactory
                .GetAsset(GameObjectAssetModel.HexagonPrefab);

            _bombPrefab = AssetFactory
                .GetAsset(GameObjectAssetModel.HexagonBomb);

            _poolContainer = GameObject.Find("PoolContainer");

            ObjectPool.PreLoadInstances(_hexagonPrefab, 500, _poolContainer.transform);
            ObjectPool.PreLoadInstances(_bombPrefab, 10, _poolContainer.transform);
        }

        /// <summary>
        /// This function initializes cells and hexagons
        /// </summary>
        public void InitializeItems()
        {
            // Spawn cells and hexagons
            for (int i = 0; i < HexagonGencerUtils.GameSettings.BOARD_HEIGHT; ++i)
            {
                for (int j = 0; j < HexagonGencerUtils.GameSettings.BOARD_WIDTH; ++j)
                {
                    var itemPosition = HexagonGencerUtils.GetItemPosition(i, j);

                    var cellObjectInstance = GameObject.Instantiate(AssetFactory.GetAsset(GameObjectAssetModel.CellPrefab));
                    cellObjectInstance.transform.position = itemPosition;

                    if (!cellObjectInstance.TryGetComponent<Cell>(out Cell cellInstance)) return;
                    _cellList.Add(cellInstance);

                    var hexInstance = ObjectPool.GetInstance(_hexagonPrefab);
                    hexInstance.transform.position = cellObjectInstance.transform.position;

                    if (!hexInstance.TryGetComponent<Item>(out Item item)) return;
                    cellInstance.UpdateHexagon(item);
                    item.SetRandomColor();
                    cellInstance.Row = i;
                    cellInstance.Column = j;
                    cellInstance.transform.name = cellInstance.Index.ToString();
                }
            }

            // Assign nighbours
            foreach (Cell cell in _cellList)
            {
                cell.SetNeighbours(HexagonGencerUtils.GetNeighboursFromCell(cell, _cellList));
            }

            // Check if color match occured
            CheckColorMatch();

            // If there is no move availabe, restart
            if (!CheckAvailableMoves())
            {
                RestartLevel();
            }
        }

        public void InitializeOutline()
        {
            if (_outline != null)
                Destroy(_outline);

            _outline = GameObject.Instantiate(AssetFactory
                .GetAsset(GameObjectAssetModel.OutlinePrefab));

            _outline.SetActive(false);
        }

        /// <summary>
        /// Initialize info dictionary which contains explosion data in specific column
        /// </summary>
        public void InitializeDictionary()
        {
            for (int i = 0; i < HexagonGencerUtils.GameSettings.BOARD_WIDTH; ++i)
            {
                if (!_explodeInfo.ContainsKey(i))
                    _explodeInfo.Add(i, 0);

                else
                    _explodeInfo[i] = 0;
            }
        }

        /// <summary>
        /// This function creates available color list,
        /// checks all tuples on a cell, if there is color match,
        /// removes that color from list,
        /// assigns new color
        /// </summary>
        private void CheckColorMatch()
        {
            foreach (Cell cell in _cellList)
            {
                var availableColorIndices = new List<int>();

                for (int i = 0; i < HexagonGencerUtils.GameSettings.NUMBER_OF_COLORS; ++i)
                {
                    availableColorIndices.Add(i);
                }

                for (int i = 0; i < 6; ++i)
                {
                    var tuple = TupleFactory.GetItemTuple(cell.Item, i);

                    if (tuple == null) { continue; }

                    if (tuple.Item2.ItemColor == tuple.Item3.ItemColor)
                    {
                        availableColorIndices.Remove((int)tuple.Item2.ItemColor);
                        
                        if (availableColorIndices.Count == 0)
                        {
                            RestartLevel();
                            Debug.Log("restarting");
                            return;
                        }
                    }
                }

                var index = availableColorIndices[Random.Range(0, availableColorIndices.Count)];
                cell.Item.SetColor((ItemColor)index);
            }
        }

        /// <summary>
        /// This function sets camera properties according to board size,
        /// finds top-middle and bottom middle cells,
        /// calculates to average position,
        /// puts camera on that posiiton
        /// </summary>
        public void SetCameraBounds()
        {
            _mainCam = Camera.main;

            var upperIndex = _cellList.Count - Mathf.CeilToInt(HexagonGencerUtils.GameSettings.BOARD_WIDTH / 2f);
            var upperCenter = (_cellList[upperIndex].transform.position + _cellList[upperIndex - 1].transform.position) / 2;
            var lowerIndex = HexagonGencerUtils.GameSettings.BOARD_WIDTH - (int)Mathf.Ceil(HexagonGencerUtils.GameSettings.BOARD_WIDTH / 2);
            var lowerCenter = (_cellList[lowerIndex].transform.position + _cellList[lowerIndex - 1].transform.position) / 2;

            var position = (upperCenter + lowerCenter) / 2;
            position.z = _mainCam.transform.position.z;

            _mainCam.transform.position = position;

            if (HexagonGencerUtils.GameSettings.BOARD_HEIGHT > HexagonGencerUtils.GameSettings.BOARD_WIDTH)
                _mainCam.orthographicSize = upperCenter.y;

            else
                _mainCam.orthographicSize = position.x + (1.5f * HexagonGencerUtils.GameSettings.BOARD_WIDTH);
        }

        /// <summary>
        /// Dispose all diposables
        /// </summary>
        public void DisposeAll()
        {
            _disposables.Dispose();
        }

        #endregion
    }
}
