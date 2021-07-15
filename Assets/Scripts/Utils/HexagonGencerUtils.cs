using UnityEngine;

namespace HexagonGencer.Utils
{
    public static class HexagonGencerUtils
    {
        #region Constants

        public static int BOARD_WIDTH = 8;
        public static int BOARD_HEIGHT = 9;

        public const float x_MUL = 1.9f;
        public const float y_MUL = 2.2f;
        public const float y_OFFSET_EVEN_ROWS = 1.1F;

        #endregion

        #region Helper Methods

        public static Vector2 GetHexagonPosition(int i, int j)
        {
            if (j % 2 == 0)
            {
                return new Vector2(j * x_MUL, i * y_MUL + y_OFFSET_EVEN_ROWS);
            }

            else
            {
                return new Vector2(j * x_MUL, i * y_MUL);
            }
        }

        #endregion
    }
}
