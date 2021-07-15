using HexagonGencer.Game.Core.Concrete;
using HexagonGencer.Game.Models.Abstract;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonGencer.Game.Controller.Concrete
{
    public partial class GameSceneController
    {
        private void SetTupleParent(Tuple<IItem, IItem, IItem> tuple, Transform parent)
        {
            if (tuple == null) { return; }

            tuple.Item1.Transform.SetParent(parent);
            tuple.Item2.Transform.SetParent(parent);
            tuple.Item3.Transform.SetParent(parent);
        }

        private void SetTupleSortingOrder(Tuple<IItem, IItem, IItem> tuple, int sortingOrder)
        {
            if (tuple == null) { return; }

            tuple.Item1.SortingOrder.Value = sortingOrder;
            tuple.Item2.SortingOrder.Value = sortingOrder;
            tuple.Item3.SortingOrder.Value = sortingOrder;
        }

        private Vector3 GetTuplePosition(Tuple<IItem, IItem, IItem> tuple)
        {
            var position =
                (tuple.Item1.Transform.position +
                 tuple.Item2.Transform.position +
                 tuple.Item3.Transform.position) / 3f;

            return position;
        }

        private bool CheckNullItemInTuple(Tuple<IItem, IItem, IItem> tuple)
        {
            return tuple.Item1 == null || tuple.Item2 == null || tuple.Item3 == null;
        }
    }

}