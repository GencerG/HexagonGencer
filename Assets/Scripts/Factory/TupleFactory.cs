using HexagonGencer.Game.Core.Abstract;
using System;

namespace HexagonGencer.Factory
{
    public static class TupleFactory
    {
        public static Tuple<Item, Item, Item> GetItemTuple(Item firstPiece, int corner)
        {
            var neighbours = firstPiece.Cell.GetNeighbours();

            if (neighbours[corner] == null || neighbours[(corner + 1) % 6] == null)
                return null;

            Item secondPiece = neighbours[corner].Item;
            Item thirdPiece = neighbours[(corner + 1) % 6].Item;

            return Tuple.Create(firstPiece, secondPiece, thirdPiece);
        }
    }
}