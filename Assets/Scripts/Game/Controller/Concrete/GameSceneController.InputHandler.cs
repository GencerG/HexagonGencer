using HexagonGencer.Enums;
using HexagonGencer.Factory;
using HexagonGencer.Game.Core.Concrete;
using HexagonGencer.Game.Models.Abstract;
using HexagonGencer.Utils;
using System;
using UniRx;
using UnityEngine;

namespace HexagonGencer.Game.Controller.Concrete
{
    public partial class GameSceneController
    {
        #region Fields

        private bool _isInteractable = true;
        private Tuple<IItem, IItem, IItem> _previousTuple, _currentTuple = null;

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
            if (!_isInteractable) { return; }

            SetTupleParent(_previousTuple, _poolContainer.transform);
            SetTupleSortingOrder(_previousTuple, 0);

            var hit = HexagonGencerUtils.RayCast2D(mousePosition);

            if (hit.collider == null) { return; }

            var hexInstance = hit.transform;

            if (!hexInstance.TryGetComponent<IItem>(out IItem item)) { return; }

            var corner = HexagonGencerUtils.GetHexCorner(hexInstance.position, hit.point);

            _currentTuple = TupleFactory.GetItemTuple(item, (int)corner);

            if (_currentTuple == null)
                return;

            var tuplePosition = GetTuplePosition(_currentTuple);
            var eulerAngles = GetOutlineAngles(corner);

            MoveOutline(tuplePosition, eulerAngles);
            _outline.SetActive(true);
            SetTupleParent(_currentTuple, _outline.transform);
            SetTupleSortingOrder(_currentTuple, 1);

            _previousTuple = _currentTuple;
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


        private Vector3 GetOutlineAngles(ItemCorner corner)
        {
            if (corner == ItemCorner.BottomLeft ||
                corner == ItemCorner.TopLeft ||
                corner == ItemCorner.Right)
                return new Vector3(0f, 0f, 60f);

            else
                return Vector3.zero;
        }

        #endregion
    }
}
