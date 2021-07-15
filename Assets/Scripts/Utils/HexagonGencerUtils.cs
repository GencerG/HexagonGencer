using HexagonGencer.Enums;
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

        public const string LAYER_MASK = "Hexagon";

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

        public static RaycastHit2D RayCast2D(Vector2 screenPoisition)
        {
            var ray = Camera.main.ScreenPointToRay(screenPoisition);
            var layerMask = LayerMask.GetMask(LAYER_MASK);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, layerMask);
            return hit;
        }

        public static HexagonCorner GetHexCorner(Vector2 hexPosition, Vector2 hitPosition)
        {
            var direction = hitPosition - hexPosition;
            direction.Normalize();

            var angle = Vector2.SignedAngle(Vector2.right, direction);

            if (angle > -30f && angle <= 30f)
            {
                return HexagonCorner.Right;
            }

            else if (angle > 30f && angle <= 90f)
            {
                return HexagonCorner.TopRight;
            }

            else if (angle > 90f && angle <= 150f)
            {
                return HexagonCorner.TopLeft;
            }

            else if (angle > 150f && angle <= -150f)
            {
                return HexagonCorner.Left;
            }

            else if (angle > -150f && angle <= -90f)
            {
                return HexagonCorner.BottomLeft;
            }

            else if (angle > -90f && angle <= -30f)
            {
                return HexagonCorner.BottomRight;
            }

            else
            {
                return HexagonCorner.Left;
            }
        }

        #endregion
    }
}
