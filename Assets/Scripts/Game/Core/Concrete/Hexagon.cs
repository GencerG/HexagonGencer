using HexagonGencer.Game.Core.Abstract;
using UniRx;
using UnityEngine;

namespace HexagonGencer.Game.Core.Concrete
{
    public class Hexagon : Item
    {
        public override bool Execute()
        {
            return true;
        }

        #region Unity

        private void Start()
        {
            transform.rotation = Quaternion.identity;

            SortingOrder.Subscribe(order =>
            {
                spriteRenderer.sortingOrder = order;
            });
        }

        #endregion
    }
}