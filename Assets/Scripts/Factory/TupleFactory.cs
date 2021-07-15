using HexagonGencer.Game.Models.Abstract;
using System;

namespace HexagonGencer.Factory
{
    public static class TupleFactory
    {
        public static Tuple<IItem, IItem, IItem> GetItemTuple(IItem firstPiece, int corner)
        {
            var neighbours = firstPiece.Cell.GetNeighbours();

            if (neighbours[corner] == null || neighbours[(corner + 1) % 6] == null)
                return null;

            IItem secondPiece = neighbours[corner].Item;
            IItem thirdPiece = neighbours[(corner + 1) % 6].Item;

            return Tuple.Create(firstPiece, secondPiece, thirdPiece);
        }
    }
}