using DG.Tweening;
using HexagonGencer.Factory;
using HexagonGencer.Game.Core.Concrete;
using HexagonGencer.Utils;
using System.Collections.Generic;
using System.Collections;
using UniRx;
using UnityEngine;
using HexagonGencer.Game.Core.Abstract;

namespace HexagonGencer.Game.Controller.Concrete
{
    public partial class GameSceneController
    {
        #region Fields

        private int _nextBomb;
        private int _moveCounter;

        #endregion

        #region Binding

        public void BindGridManager()
        {
            _nextBomb += HexagonGencerUtils.BOMB_SPAWN_RATE;

            _onStartRotating.Subscribe(tuple =>
            {
                var cell1 = tuple.Item1.Cell;
                var cell2 = tuple.Item2.Cell;
                var cell3 = tuple.Item3.Cell;

                if (_rotationDirection == HexagonGencerUtils.CLOCK_WISE)
                {
                    cell1.UpdateHexagon(tuple.Item3);
                    cell2.UpdateHexagon(tuple.Item1);
                    cell3.UpdateHexagon(tuple.Item2);
                }

                else
                {
                    cell1.UpdateHexagon(tuple.Item2);
                    cell2.UpdateHexagon(tuple.Item3);
                    cell3.UpdateHexagon(tuple.Item1);
                }
            }).AddTo(_disposables);

            _onMatch.Subscribe(matchables =>
            {
                foreach (Cell cell in matchables)
                {
                    var hexagon = cell.Item;

                    if (hexagon == null)
                        continue;

                    if (_bombList.Contains(cell.Item.gameObject))
                    {
                        _bombList.Remove(cell.Item.gameObject);
                    }

                    ObjectPool.StoreInstance(hexagon.gameObject);
                    cell.UpdateHexagon(null);

                    _explodeInfo[cell.Column]++;
                }

                FallHexes();
                SpawnNewHexes();
                ResetAfterMove();
                ResetOutline();

                _gameUIModel.Score.Value += matchables.Count * 10;

                matchables.Clear();
            }).AddTo(_disposables);
        }

        #endregion

        #region Custom Methods

        private void FallHexes()
        {
            foreach (KeyValuePair<int, int> entry in _explodeInfo)
            {
                if (entry.Value == 0)
                    continue;

                for (int i = 1; i < HexagonGencerUtils.GameSettings.BOARD_HEIGHT; ++i)
                {
                    var index = i * HexagonGencerUtils.GameSettings.BOARD_WIDTH + entry.Key;
                    var cell = _cellList[index];

                    if (cell.Item != null)
                    {
                        var targetCell = cell.GetTargetCell();

                        if (targetCell != null && targetCell != cell)
                        {
                            targetCell.UpdateHexagon(cell.Item);
                            _moveCounter++;
                            cell.Item.transform.DOMoveY(targetCell.transform.position.y, 1f).SetEase(Ease.OutBounce).OnComplete(() =>
                            {
                                List<Cell> matchables = new List<Cell>();
                                CheckTuples(targetCell, ref matchables);

                                if (matchables.Count >= 3)
                                {
                                    _onMatch.OnNext(matchables);
                                }
                                _moveCounter--;
                            });
                            cell.UpdateHexagon(null);
                        }
                    }
                }
            }
        }

        private void SpawnNewHexes()
        {
            foreach (KeyValuePair<int, int> entry in _explodeInfo)
            {
                for (int i = 0; i < entry.Value; ++i)
                {
                    var topCell = HexagonGencerUtils.GetTopCell(entry.Key, _cellList);
                    var targetCell = topCell.GetTargetCell();

                    GameObject hexagonInstance;

                    if (_gameUIModel.Score.Value >= _nextBomb)
                    {
                        _nextBomb += HexagonGencerUtils.BOMB_SPAWN_RATE;
                        hexagonInstance = ObjectPool.GetInstance(_bombPrefab);
                        _bombList.Add(hexagonInstance);
                    }
                    else
                        hexagonInstance = ObjectPool.GetInstance(_hexagonPrefab);

                    hexagonInstance.transform.position = topCell.transform.position + new Vector3(0f, 7f, 0f);

                    var item = hexagonInstance.GetComponent<Item>();
                    item.SetRandomColor();
                    targetCell.UpdateHexagon(item);
                    _moveCounter++;
                    hexagonInstance.transform.DOMoveY(targetCell.transform.position.y, 1f).SetEase(Ease.OutBounce).OnComplete(() =>
                    {
                        List<Cell> matchables = new List<Cell>();
                        CheckTuples(targetCell, ref matchables);

                        if (matchables.Count >= 3)
                        {
                            _onMatch.OnNext(matchables);
                        }
                        _moveCounter--;
                    });
                }
            }
        }

        private IEnumerator ChainRoutine()
        {
            while(_moveCounter > 0)
            {
                yield return new WaitForEndOfFrame();
            }

            Debug.Log("End of Chain..");

            _isInteractable = true;
            _gameUIModel.Moves.Value++;

            foreach(GameObject bomb in _bombList)
            {
                if (!bomb.GetComponent<Item>().Execute())
                    _gameOverPanel.SetActive(true);
            }

            if(!CheckAvailableMoves())
            {
                _gameOverPanel.SetActive(true);
            }

            yield return null;
        }

        private bool CheckAvailableMoves()
        {
            foreach (Cell cell in _cellList)
            {
                for (int i = 0; i < 6; ++i)
                {
                    var tuple = TupleFactory.GetItemTuple(cell.Item, i);
                    
                    if (tuple == null) { continue; }

                    var cell1 = tuple.Item1.Cell;
                    var cell2 = tuple.Item2.Cell;
                    var cell3 = tuple.Item3.Cell;

                    var item1 = cell1.Item;
                    var item2 = cell2.Item;
                    var item3 = cell3.Item;

                    for (int j = 0; j < 3; j++)
                    {
                        cell1.UpdateHexagon(tuple.Item3);
                        cell2.UpdateHexagon(tuple.Item1);
                        cell3.UpdateHexagon(tuple.Item2);

                        List<Cell> matchables = new List<Cell>();
                        CheckTuples(cell1, ref matchables);
                        CheckTuples(cell2, ref matchables);
                        CheckTuples(cell3, ref matchables);

                        if (matchables.Count >= 3)
                        {
                            cell1.UpdateHexagon(item1);
                            cell2.UpdateHexagon(item2);
                            cell3.UpdateHexagon(item3);
                            return true;
                        }
                    }

                    cell1.UpdateHexagon(item1);
                    cell2.UpdateHexagon(item2);
                    cell3.UpdateHexagon(item3);
                }
            }

            return false;
        }

        private void ResetOutline()
        {
            SetTupleParent(_currentTuple, _poolContainer.transform);
            SetTupleSortingOrder(_currentTuple, 0);

            _outline.SetActive(false);
        }

        private void ResetAfterMove()
        {
            for (int i = 0; i < HexagonGencerUtils.GameSettings.BOARD_WIDTH; ++i)
            {
                _explodeInfo[i] = 0;
            }
        }

        private void DestroyBoard()
        {
            foreach (Cell cell in _cellList)
            {
                Destroy(cell.gameObject);
            }

            _cellList.Clear();
            _bombList.Clear();
            _previousTuple = null;
            _currentTuple = null;

            Destroy(_outline);
        }

        #endregion
    }
}
