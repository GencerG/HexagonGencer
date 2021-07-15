using HexagonGencer.Enums;
using UnityEngine;

namespace HexagonGencer.Factory
{
    public static class ColorFactory
    {
        public static Color GetColor(ItemColor itemColor)
        {
            switch (itemColor)
            {
                case ItemColor.Blue:
                    return Color.blue;

                case ItemColor.Green:
                    return Color.green;

                case ItemColor.Magenta:
                    return Color.magenta;

                case ItemColor.Red:
                    return Color.red;

                case ItemColor.Yellow:
                    return Color.yellow;

                case ItemColor.Cyan:
                    return Color.cyan;

                case ItemColor.Orange:
                    return new Color(1.0f, 0.64f, 0.0f);
            }

            return Color.white;
        }
    }
}