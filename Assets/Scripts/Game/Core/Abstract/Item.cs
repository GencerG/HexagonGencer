using HexagonGencer.Enums;
using HexagonGencer.Factory;
using HexagonGencer.Game.Core.Concrete;
using HexagonGencer.Utils;
using UniRx;
using UnityEngine;

namespace HexagonGencer.Game.Core.Abstract
{
    public abstract class Item : MonoBehaviour
    {
        #region Fields

        [SerializeField] protected SpriteRenderer spriteRenderer;

        public ItemColor ItemColor { get; set; }

        public Cell Cell { get; set; }

        public IntReactiveProperty SortingOrder { get; set; } = new IntReactiveProperty(0);

        #endregion

        #region Custom Methods

        public abstract bool Execute();

        public virtual void SetRandomColor()
        {
            var colorIndex = Random.Range(0, HexagonGencerUtils.GameSettings.NUMBER_OF_COLORS);

            ItemColor = (ItemColor)colorIndex;

            spriteRenderer.color =
                ColorFactory.GetColor(ItemColor);
        }

        public virtual void SetColor(ItemColor color)
        {
            var itemColor = ColorFactory.GetColor(color);

            ItemColor = color;

            spriteRenderer.color = itemColor;
        }

        #endregion
    }
}
