using HexagonGencer.Factory;
using HexagonGencer.Game.Models.Abstract;
using HexagonGencer.Utils;
using System.Collections.Generic;
using UnityEngine;


namespace HexagonGencer.Game.Core.Concrete
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private Cell[] _neighbours = new Cell[6];
        [SerializeField] private Cell _cellBelow = null;
        public IItem Item { get; private set; }
        public int Row;
        public int Column;
        public bool IsAdded = false;
        public int Index => HexagonGencerUtils.BOARD_WIDTH * Row + Column;

        public void UpdateHexagon(IItem newItem)
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

        public void Execute()
        {
            Item.Execute();
        }
    }
}