using HexagonGencer.Factory;
using HexagonGencer.Game.Core.Abstract;
using HexagonGencer.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonGencer.Game.Core.Concrete
{
    public class Cell : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Cell[] _neighbours = new Cell[6];
        [SerializeField] private Cell _cellBelow = null;
        public Item Item { get; private set; }
        public int Row;
        public int Column;
        public bool IsAdded = false;
        public int Index => HexagonGencerUtils.GameSettings.BOARD_WIDTH * Row + Column;

        #endregion

        #region Custom Methods

        /// <summary>
        /// This function updates cell's item
        /// </summary>
        /// <param name="newItem">New Item</param>
        public void UpdateHexagon(Item newItem)
        {
            Item = newItem;

            if (newItem != null)
                Item.Cell = this;
        }

        public Cell[] GetNeighbours()
        {
            return _neighbours;
        }

        public void SetNeighbours(Cell[] neighbours)
        {
            _neighbours = neighbours;
            _cellBelow = neighbours[3];
        }

        /// <summary>
        /// This function calculates the target cell after explosion,
        /// loops until finds a cell whose item isn't null
        /// </summary>
        /// <returns>Target cell</returns>
        public Cell GetTargetCell()
        {
            Cell targetCell = this;

            while (targetCell._cellBelow != null && targetCell._cellBelow.Item == null)
            {
                targetCell = targetCell._cellBelow;
            }

            return targetCell;
        }

        /// <summary>
        /// This function checks for a color match for each cell in the list,
        /// loops through all possible corners and creates tuples
        /// </summary>
        /// <param name="matchables"></param>
        /// <returns></returns>
        public List<Cell> CheckTuples(ref List<Cell> matchables)
        {
            for (int i = 0; i < 6; ++i)
            {
                var tuple = TupleFactory.GetItemTuple(Item, i);

                if (tuple == null)
                    continue;

                var isMatching = tuple.Item1.ItemColor == tuple.Item2.ItemColor && tuple.Item1.ItemColor == tuple.Item3.ItemColor;

                if (isMatching)
                {
                    if (!tuple.Item1.Cell.IsAdded)
                    {
                        tuple.Item1.Cell.IsAdded = true;
                        matchables.Add(tuple.Item1.Cell);
                        tuple.Item1.Cell.CheckTuples(ref matchables);
                    }

                    if (!tuple.Item2.Cell.IsAdded)
                    {
                        tuple.Item2.Cell.IsAdded = true;
                        matchables.Add(tuple.Item2.Cell);
                        tuple.Item2.Cell.CheckTuples(ref matchables);
                    }

                    if (!tuple.Item3.Cell.IsAdded)
                    {
                        tuple.Item3.Cell.IsAdded = true;
                        matchables.Add(tuple.Item3.Cell);
                        tuple.Item3.Cell.CheckTuples(ref matchables);
                    }
                }
            }

            return matchables;
        }

        public void Execute()
        {
            Item.Execute();
        }

        #endregion

        #region Unity

        private void OnDestroy()
        {
            if (Item != null)
                Destroy(Item.gameObject);
        }

        #endregion
    }
}