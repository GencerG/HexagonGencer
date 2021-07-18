using HexagonGencer.Enums;
using HexagonGencer.Game.Core.Concrete;
using HexagonGencer.Game.Scripables;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonGencer.Utils
{
    public static class HexagonGencerUtils
    {
        #region Settings

        public static GameSettings GameSettings 
            = Resources.Load("ScriptableObjects/GameSettings") as GameSettings;

        #endregion

        #region Constants

        public const int BOMB_SPAWN_PERIOD = 1000;
        public const int BOMB_HEALTH = 7;
        public const int SCORE_MULTIPLIER = 5;

        public const float CLOCK_WISE = -120f;
        public const float COUNTER_CLOCK_WISE = 120f;
        public const float ROTATION_ANIMATON_DURATION = .2f;
        public const float x_MUL = 1.9f;
        public const float y_MUL = 2.2f;
        public const float y_OFFSET_EVEN_ROWS = 1.1f;

        public const string LAYER_MASK = "Hexagon";
        public const string COMPONENT_HOLDER_TAG = "ComponentHolder";

        #endregion

        #region Helper Methods

        public static void MoveItemInList<T>(this List<T> list, int oldIndex, int newIndex)
        {
            var item = list[oldIndex];
            list[oldIndex] = list[newIndex];
            list[newIndex] = item;
        }

        /// <summary>
        /// This function callculates all possible neighbours for a specified cell
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="cellList"></param>
        /// <returns>Neighbour array</returns>
        public static Cell[] GetNeighboursFromCell(Cell cell, List<Cell> cellList)
        {
            var remainder = cell.Column % 2;
            var offset = GameSettings.BOARD_WIDTH * remainder;

            // neighbour pattern
            var neighbours = new Cell[6] {
                cell.Index < cellList.Count - GameSettings.BOARD_WIDTH ? cellList[cell.Index + GameSettings.BOARD_WIDTH] : null,
                cell.Index < cellList.Count - (GameSettings.BOARD_WIDTH + 1) + offset ? cellList[cell.Index + (GameSettings.BOARD_WIDTH + 1) - offset] : null,
                cell.Index >= -1 + offset && cell.Index != cellList.Count - 1 ? cellList[cell.Index + 1 - offset] : null,
                cell.Index >= GameSettings.BOARD_WIDTH ? cellList[cell.Index - GameSettings.BOARD_WIDTH] : null,
                cell.Index >= 1 + offset ? cellList[cell.Index - 1 - offset] : null,
                cell.Index < cellList.Count - (GameSettings.BOARD_WIDTH - 1) + offset ? cellList[cell.Index + (GameSettings.BOARD_WIDTH - 1) - offset] : null,
            };

            // run from edges
            if (cell.Column == 0)
            {
                neighbours[4] = null;
                neighbours[5] = null;
            }

            if (cell.Column == GameSettings.BOARD_WIDTH - 1)
            {
                neighbours[1] = null;
                neighbours[2] = null;
            }

            return neighbours;
        }

        /// <summary>
        /// This function calculates position with offsets, when spawning hexagons
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This function calculates which corner is touched,
        /// creates a direction vector between touch position and item position,
        /// calculates the angle between direction vector and X-axis
        /// and decides which corner is touched
        /// </summary>
        /// <param name="hexPosition"></param>
        /// <param name="hitPosition"></param>
        /// <returns>Item Corner</returns>
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

        /// <summary>
        /// This function finds highes cell possible in a specified column
        /// </summary>
        /// <param name="column"></param>
        /// <param name="cellList"></param>
        /// <returns></returns>
        public static Cell GetTopCell(int column, List<Cell> cellList)
        {
            var index = column + (GameSettings.BOARD_WIDTH * (GameSettings.BOARD_HEIGHT - 1));
            return cellList[index];
        }

        #endregion
    }
}
