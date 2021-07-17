using HexagonGencer.Game.Core.Abstract;
using HexagonGencer.Game.Core.Concrete;
using HexagonGencer.Game.Models.Abstract;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonGencer.Game.Controller.Concrete
{
    public partial class GameSceneController
    {
        #region Custom Methods

        private void SetTupleParent(Tuple<Item, Item, Item> tuple, Transform parent)
        {
            if (tuple == null) { return; }

            tuple.Item1.transform.SetParent(parent);
            tuple.Item2.transform.SetParent(parent);
            tuple.Item3.transform.SetParent(parent);
        }

        private void SetTupleSortingOrder(Tuple<Item, Item, Item> tuple, int sortingOrder)
        {
            if (tuple == null) { return; }

            tuple.Item1.SortingOrder.Value = sortingOrder;
            tuple.Item2.SortingOrder.Value = sortingOrder;
            tuple.Item3.SortingOrder.Value = sortingOrder;
        }

        private Vector3 GetTuplePosition(Tuple<Item, Item, Item> tuple)
        {
            var position =
                (tuple.Item1.transform.position +
                 tuple.Item2.transform.position +
                 tuple.Item3.transform.position) / 3f;

            return position;
        }

        private List<Cell> CheckTuples(Cell startCell, ref List<Cell> matchables)
        {
            if (startCell == null) { return null; }

            var list = startCell.CheckTuples(ref matchables);

            foreach (Cell cell in _cellList)
            {
                cell.IsAdded = false;
            }

            return list;
        }

        private bool CheckNullItemInTuple(Tuple<Item, Item, Item> tuple)
        {
            return tuple.Item1 == null || tuple.Item2 == null || tuple.Item3 == null;
        }

        #endregion
    }
}
