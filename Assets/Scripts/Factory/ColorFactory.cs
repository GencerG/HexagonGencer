using HexagonGencer.Enums;
using UnityEngine;

namespace HexagonGencer.Factory
{
    public static class ColorFactory
    {
        public static Color GetColor(HexagonColor itemColor)
        {
            switch (itemColor)
            {
                case HexagonColor.Blue:
                    return Color.blue;

                case HexagonColor.Green:
                    return Color.green;

                case HexagonColor.Magenta:
                    return Color.magenta;

                case HexagonColor.Red:
                    return Color.red;

                case HexagonColor.Yellow:
                    return Color.yellow;

                case HexagonColor.Cyan:
                    return Color.cyan;

                case HexagonColor.Orange:
                    return new Color(1.0f, 0.64f, 0.0f);
            }

            return Color.white;
        }
    }
}