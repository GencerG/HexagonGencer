using HexagonGencer.Enums;
using HexagonGencer.Factory;
using HexagonGencer.Game.Core.Concrete;
using HexagonGencer.Game.Models.Abstract;
using HexagonGencer.Utils;
using UniRx;
using UnityEngine;

namespace HexicClone.Game.Core
{
    public class Hexagon : MonoBehaviour, IItem
    {
        #region Fields

        [SerializeField] private SpriteRenderer _spriteRenderer;

        public Cell Cell { get; set; }

        public IntReactiveProperty SortingOrder { get; set; } = new IntReactiveProperty(0);

        public Transform Transform { get; set; }

        public ItemColor ItemColor { get; set; }

        #endregion

        #region Unity

        private void OnEnable()
        {
            this.Transform = transform;

            transform.rotation = Quaternion.identity;

            SortingOrder.Subscribe(order =>
            {
                _spriteRenderer.sortingOrder = order;
            });
        }

        #endregion

        #region Custom Methods

        public void SetRandomColor()
        {
            var colorIndex = Random.Range(0, HexagonGencerUtils.GameSettings.NUMBER_OF_COLORS);

            ItemColor = (ItemColor)colorIndex;

            _spriteRenderer.color =
                ColorFactory.GetColor(ItemColor);
        }

        public void Execute()
        {

        }

        #endregion
    }
}