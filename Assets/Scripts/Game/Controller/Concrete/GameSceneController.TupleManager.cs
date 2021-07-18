using HexagonGencer.Game.Core.Abstract;
using HexagonGencer.Game.Core.Concrete;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonGencer.Game.Controller.Concrete
{
    public partial class GameSceneController
    {
        #region Custom Methods

        /// <summary>
        /// This function sets item's parent in a tuple
        /// </summary>
        /// <param name="tuple">Item Tuple</param>
        /// <param name="parent">new parent object</param>
        private void SetTupleParent(Tuple<Item, Item, Item> tuple, Transform parent)
        {
            if (tuple == null) { return; }

            tuple.Item1.transform.SetParent(parent);
            tuple.Item2.transform.SetParent(parent);
            tuple.Item3.transform.SetParent(parent);
        }

        /// <summary>
        /// This function sets item's sorting order in a tuple
        /// </summary>
        /// <param name="tuple">Item Tuple</param>
        /// <param name="sortingOrder">new sorting order</param>
        private void SetTupleSortingOrder(Tuple<Item, Item, Item> tuple, int sortingOrder)
        {
            if (tuple == null) { return; }

            tuple.Item1.SortingOrder.Value = sortingOrder;
            tuple.Item2.SortingOrder.Value = sortingOrder;
            tuple.Item3.SortingOrder.Value = sortingOrder;
        }

        /// <summary>
        /// This function calculates center of a tuple
        /// </summary>
        /// <param name="tuple"></param>
        /// <returns>Center position</returns>
        private Vector3 GetTuplePosition(Tuple<Item, Item, Item> tuple)
        {
            var position =
                (tuple.Item1.transform.position +
                 tuple.Item2.transform.position +
                 tuple.Item3.transform.position) / 3f;

            return position;
        }

        /// <summary>
        /// This functions resets 'IsAdded' paramater for all cells
        /// </summary>
        /// <param name="startCell"></param>
        /// <param name="matchables"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This function checks for a null item in a tuple
        /// </summary>
        /// <param name="tuple"></param>
        /// <returns>True, if there is any null item</returns>
        private bool CheckNullItemInTuple(Tuple<Item, Item, Item> tuple)
        {
            return tuple.Item1 == null || tuple.Item2 == null || tuple.Item3 == null;
        }

        #endregion
    }
}
