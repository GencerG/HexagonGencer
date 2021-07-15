using HexagonGencer.Enums;
using HexagonGencer.Factory;
using UnityEngine;

namespace HexagonGencer.Game.Core.Concrete
{
    public class Hexagon : MonoBehaviour
    {
        #region Fields

        public HexagonColor HexagonColor;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        #endregion

        #region Unity

        #endregion

        #region Custom Methods

        public void SetRandomColor()
        {
            var colorIndex = Random.Range(0, 7);

            HexagonColor = (HexagonColor)colorIndex;

            _spriteRenderer.color =
                ColorFactory.GetColor(HexagonColor);
        }

        #endregion
    }
}
