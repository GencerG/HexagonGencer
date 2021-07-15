using HexagonGencer.Enums;
using HexagonGencer.Game.Core.Concrete;
using HexagonGencer.Utils;
using UniRx;
using UnityEngine;

namespace HexagonGencer.Game.Controller.Concrete
{
    public partial class GameSceneController
    {
        #region Fields

        #endregion

        #region Bindings

        private void BindInputEvents()
        {
            var inputObserver = GameObject.FindObjectOfType<InputObserver>();

            inputObserver
                .OnClick
                .Subscribe(HandleOnClick);

            inputObserver
                .OnSwipeDown
                .Subscribe(HandleOnSwipeDown);

            inputObserver
                .OnSwipeLeft
                .Subscribe(HandleOnSwipeLeft);

            inputObserver
                .OnSwipeRight
                .Subscribe(HandleOnSwipeRight);

            inputObserver
                .OnSwipeUp
                .Subscribe(HandleOnSwipeUp);
        }

        #endregion

        #region Handles

        private void HandleOnClick(Vector2 mousePosition)
        {
            var hit = HexagonGencerUtils.RayCast2D(mousePosition);

            if (hit.collider == null) { return; }

            var hexInstance = hit.transform;

            if (!hexInstance.TryGetComponent<Hexagon>(out Hexagon hexagon)) { return; }

            Debug.Log(hexagon.HexagonColor);

            var corner = HexagonGencerUtils.GetHexCorner(hexInstance.position, hit.point);
            var eulerAngles = GetOutlineAngles(corner);

            Debug.Log(corner);
            _outline.SetActive(true);
            MoveOutline(hexInstance.position, eulerAngles);
        }

        private void HandleOnSwipeDown(Unit unit)
        {

        }

        private void HandleOnSwipeLeft(Unit unit)
        {

        }

        private void HandleOnSwipeRight(Unit unit)
        {

        }

        private void HandleOnSwipeUp(Unit unit)
        {

        }

        #endregion

        #region Helper Methods

        private void MoveOutline(Vector3 position, Vector3 eulerAngles)
        {
            _outline.transform.position = position;
            _outline.transform.localEulerAngles = eulerAngles;
        }


        private Vector3 GetOutlineAngles(HexagonCorner corner)
        {
            if (corner == HexagonCorner.BottomLeft ||
                corner == HexagonCorner.TopLeft ||
                corner == HexagonCorner.Right)
                return new Vector3(0f, 0f, 60f);

            else
                return Vector3.zero;
        }

        #endregion
    }
}
