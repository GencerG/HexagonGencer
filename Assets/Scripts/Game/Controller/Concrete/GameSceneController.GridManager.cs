using DG.Tweening;
using HexagonGencer.Game.Core.Concrete;
using HexagonGencer.Game.Models.Abstract;
using HexagonGencer.Utils;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace HexagonGencer.Game.Controller.Concrete
{
    public partial class GameSceneController
    {
        #region Binding

        private void BindGridManager()
        {
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

                
                tuple.Item1.Transform.GetChild(0).GetComponent<TextMesh>().text = tuple.Item1.Cell.Index.ToString();
                tuple.Item2.Transform.GetChild(0).GetComponent<TextMesh>().text = tuple.Item2.Cell.Index.ToString();
                tuple.Item3.Transform.GetChild(0).GetComponent<TextMesh>().text = tuple.Item3.Cell.Index.ToString();
            });

            _onMatch.Subscribe(matchables =>
            {
                foreach (Cell cell in matchables)
                {
                    var hexagon = cell.Item;

                    if (hexagon == null)
                        continue;

                    _objectPool.AddToPool(hexagon.Transform.gameObject);
                    cell.UpdateHexagon(null);

                    _explodeInfo[cell.Column]++;
                }

                FallHexes();
                SpawnNewHexes();
                ResetAfterMove();
                ResetOutline();

                matchables.Clear();
                _isInteractable = true;
            });
        }

        #endregion

        #region Custom Methods

        private void FallHexes()
        {
            foreach (KeyValuePair<int, int> entry in _explodeInfo)
            {
                if (entry.Value == 0)
                    continue;

                for (int i = 1; i < HexagonGencerUtils.BOARD_HEIGHT; ++i)
                {
                    var index = i * HexagonGencerUtils.BOARD_WIDTH + entry.Key;
                    var cell = _cellList[index];

                    if (cell.Item != null)
                    {
                        var targetCell = cell.GetTargetCell();

                        if (targetCell != null && targetCell != cell)
                        {
                            targetCell.UpdateHexagon(cell.Item);
                            cell.Item.Transform.DOMoveY(targetCell.transform.position.y, 1f).SetEase(Ease.OutBounce).OnComplete(() =>
                            {
                                List<Cell> matchables = new List<Cell>();
                                CheckTuples(targetCell, ref matchables);

                                foreach (Cell cell in matchables)
                                {
                                    cell.Execute();
                                }

                                if (matchables.Count >= 3)
                                {
                                    _onMatch.OnNext(matchables);
                                }
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
                    var hexagonInstance = _objectPool.GetFromPool();
                    hexagonInstance.transform.position = topCell.transform.position + new Vector3(0f, 7f, 0f);

                    var targetCell = topCell.GetTargetCell();
                    var item = hexagonInstance.GetComponent<IItem>();
                    item.SetRandomColor();
                    targetCell.UpdateHexagon(item);
                    hexagonInstance.transform.DOMoveY(targetCell.transform.position.y, 1f).SetEase(Ease.OutBounce).OnComplete(() =>
                    {
                        List<Cell> matchables = new List<Cell>();
                        CheckTuples(targetCell, ref matchables);

                        foreach (Cell cell in matchables)
                        {
                            cell.Execute();
                        }

                        if (matchables.Count >= 3)
                        {
                            _onMatch.OnNext(matchables);
                        }
                    });
                }
            }
        }

        private void ResetOutline()
        {
            SetTupleParent(_currentTuple, _poolContainer.transform);
            SetTupleSortingOrder(_currentTuple, 0);

            _outline.SetActive(false);
        }

        private void ResetAfterMove()
        {
            for (int i = 0; i < HexagonGencerUtils.BOARD_WIDTH; ++i)
            {
                _explodeInfo[i] = 0;
            }
        }

        #endregion
    }
}
