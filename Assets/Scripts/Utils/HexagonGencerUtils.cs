using HexagonGencer.Enums;
using HexagonGencer.Game.Core.Concrete;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonGencer.Utils
{
    public static class HexagonGencerUtils
    {
        #region Constants

        public static int BOARD_WIDTH = 8;
        public static int BOARD_HEIGHT = 9;
        public static int NUMBER_OF_COLORS = 7;
        public static readonly float CLOCK_WISE = -120f;
        public static readonly float COUNTER_CLOCK_WISE = 120f;
        public const float ROTATION_ANIMATON_DURATION = .2f;

        public const float x_MUL = 1.9f;
        public const float y_MUL = 2.2f;
        public const float y_OFFSET_EVEN_ROWS = 1.1F;

        public const string LAYER_MASK = "Hexagon";

        #endregion

        #region Helper Methods

        public static void MoveItemInList<T>(this List<T> list, int oldIndex, int newIndex)
        {
            var item = list[oldIndex];
            list[oldIndex] = list[newIndex];
            list[newIndex] = item;
        }

        public static Cell[] GetNeighboursFromCell(Cell cell, List<Cell> cellList)
        {
            var remainder = cell.Column % 2;
            var offset = BOARD_WIDTH * remainder;

            var neighbours = new Cell[6] {
                cell.Index < cellList.Count - BOARD_WIDTH ? cellList[cell.Index + BOARD_WIDTH] : null,
                cell.Index < cellList.Count - (BOARD_WIDTH + 1) + offset ? cellList[cell.Index + (BOARD_WIDTH + 1) - offset] : null,
                cell.Index >= -1 + offset && cell.Index != cellList.Count - 1 ? cellList[cell.Index + 1 - offset] : null,
                cell.Index >= BOARD_WIDTH ? cellList[cell.Index - BOARD_WIDTH] : null,
                cell.Index >= 1 + offset ? cellList[cell.Index - 1 - offset] : null,
                cell.Index < cellList.Count - (BOARD_WIDTH - 1) + offset ? cellList[cell.Index + (BOARD_WIDTH - 1) - offset] : null,
            };


            if (cell.Column == 0)
            {
                neighbours[4] = null;
                neighbours[5] = null;
            }

            if (cell.Column == BOARD_WIDTH - 1)
            {
                neighbours[1] = null;
                neighbours[2] = null;
            }

            return neighbours;
        }

        public static Vector2 GetItemPosition(int i, int j)
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

        public static ItemCorner GetHexCorner(Vector2 hexPosition, Vector2 hitPosition)
        {
            var direction = hitPosition - hexPosition;
            direction.Normalize();

            var angle = Vector2.SignedAngle(Vector2.right, direction);

            if (angle > -30f && angle <= 30f)
            {
                return ItemCorner.Right;
            }

            else if (angle > 30f && angle <= 90f)
            {
                return ItemCorner.TopRight;
            }

            else if (angle > 90f && angle <= 150f)
            {
                return ItemCorner.TopLeft;
            }

            else if (angle > 150f && angle <= -150f)
            {
                return ItemCorner.Left;
            }

            else if (angle > -150f && angle <= -90f)
            {
                return ItemCorner.BottomLeft;
            }

            else if (angle > -90f && angle <= -30f)
            {
                return ItemCorner.BottomRight;
            }

            else
            {
                return ItemCorner.Left;
            }
        }

        #endregion
    }
}
