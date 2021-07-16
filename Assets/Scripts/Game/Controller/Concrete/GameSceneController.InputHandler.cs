using DG.Tweening;
using HexagonGencer.Enums;
using HexagonGencer.Factory;
using HexagonGencer.Game.Core.Concrete;
using HexagonGencer.Game.Models.Abstract;
using HexagonGencer.Utils;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace HexagonGencer.Game.Controller.Concrete
{
    public partial class GameSceneController
    {
        #region Fields

        private bool _isInteractable = true;
        private Tuple<IItem, IItem, IItem> _previousTuple, _currentTuple = null;
        private float _rotationDirection;

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

            var hit = HexagonGencerUtils.RayCast2D(mousePosition);

            if (hit.collider == null) { return; }

            var hexInstance = hit.transform;

            if (!hexInstance.TryGetComponent<IItem>(out IItem item)) { return; }

            var corner = HexagonGencerUtils.GetHexCorner(hexInstance.position, hit.point);

            _currentTuple = TupleFactory.GetItemTuple(item, (int)corner);

            if (_currentTuple == null) 
            {
                _currentTuple = _previousTuple;
                return;
            }

            SetTupleParent(_previousTuple, _poolContainer.transform);
            SetTupleSortingOrder(_previousTuple, 0);

            var tuplePosition = GetTuplePosition(_currentTuple);
            var eulerAngles = GetOutlineAngles(corner);

            MoveOutline(tuplePosition, eulerAngles);

            SetTupleParent(_currentTuple, _outline.transform);
            SetTupleSortingOrder(_currentTuple, 1);

            _previousTuple = _currentTuple;
        }

        private void HandleOnSwipeDown(Vector2 mousePosition)
        {
            if (!_isInteractable) { return; }

            if (!_outline.activeInHierarchy) { return; }

            _isInteractable = false;

            var worldPosition = _mainCam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f));

            _rotationDirection = worldPosition.x < _outline.transform.position.x ?
                HexagonGencerUtils.COUNTER_CLOCK_WISE : HexagonGencerUtils.CLOCK_WISE;

            RotationTween(_outline.transform, _rotationDirection);
        }

        private void HandleOnSwipeLeft(Vector2 mousePosition)
        {
            if (!_isInteractable) { return; }

            if (!_outline.activeInHierarchy) { return; }

            _isInteractable = false;

            var worldPosition = _mainCam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f));

            _rotationDirection = worldPosition.y > _outline.transform.position.y ?
                HexagonGencerUtils.COUNTER_CLOCK_WISE : HexagonGencerUtils.CLOCK_WISE;

            RotationTween(_outline.transform, _rotationDirection);
        }

        private void HandleOnSwipeRight(Vector2 mousePosition)
        {
            if (!_isInteractable) { return; }

            if (!_outline.activeInHierarchy) { return; }

            _isInteractable = false;

            var worldPosition = _mainCam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f));

            _rotationDirection = worldPosition.y < _outline.transform.position.y ?
                HexagonGencerUtils.COUNTER_CLOCK_WISE : HexagonGencerUtils.CLOCK_WISE;

            RotationTween(_outline.transform, _rotationDirection);
        }

        private void HandleOnSwipeUp(Vector2 mousePosition)
        {
            if (!_isInteractable) { return; }

            if (!_outline.activeInHierarchy) { return; }

            _isInteractable = false;

            var worldPosition = _mainCam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f));

            _rotationDirection = worldPosition.x > _outline.transform.position.x ?
                HexagonGencerUtils.COUNTER_CLOCK_WISE : HexagonGencerUtils.CLOCK_WISE;

            RotationTween(_outline.transform, _rotationDirection);
        }

        #endregion

        #region Custom Methods

        private void MoveOutline(Vector3 position, Vector3 eulerAngles)
        {
            _outline.SetActive(true);
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

        private void RotationTween(Transform objectToRotate, float angle)
        {
            var eulerAngles = objectToRotate.transform.localEulerAngles;

            var rotationSequence = DOTween.Sequence();

            rotationSequence.AppendInterval(0.01f).Append(objectToRotate.transform.DORotate(
               new Vector3(0f, 0f, angle + eulerAngles.z),
               HexagonGencerUtils.ROTATION_ANIMATON_DURATION,
               RotateMode.Fast).SetEase(Ease.Linear)
               .OnStart(() => _onStartRotating.OnNext(_currentTuple))
                .OnComplete(() =>
                {
                    List<Cell> matchables = new List<Cell>();
                    CheckTuples(_currentTuple.Item1.Cell, ref matchables);
                    CheckTuples(_currentTuple.Item2.Cell, ref matchables);
                    CheckTuples(_currentTuple.Item3.Cell, ref matchables);

                    if (matchables.Count >= 3)
                    {
                        rotationSequence.Kill();
                        _onMatch.OnNext(matchables);
                        _gameUIModel.Moves.Value++;
                    }
                }));

            rotationSequence.AppendInterval(0.01f).Append(objectToRotate.transform.DORotate(
               new Vector3(0f, 0f, angle * 2 + eulerAngles.z),
               HexagonGencerUtils.ROTATION_ANIMATON_DURATION,
               RotateMode.Fast).SetEase(Ease.Linear).
               OnStart(() => _onStartRotating.OnNext(_currentTuple))
                .OnComplete(() =>
                {
                    List<Cell> matchables = new List<Cell>();
                    CheckTuples(_currentTuple.Item1.Cell, ref matchables);
                    CheckTuples(_currentTuple.Item2.Cell, ref matchables);
                    CheckTuples(_currentTuple.Item3.Cell, ref matchables);

                    if (matchables.Count >= 3)
                    {
                        rotationSequence.Kill();
                        _onMatch.OnNext(matchables);
                        _gameUIModel.Moves.Value++;
                    }

                }));

            rotationSequence.AppendInterval(0.01f).Append(objectToRotate.transform.DORotate(
               new Vector3(0f, 0f, angle * 3 + eulerAngles.z),
               HexagonGencerUtils.ROTATION_ANIMATON_DURATION,
               RotateMode.Fast).SetEase(Ease.Linear)
               .OnStart(() => _onStartRotating.OnNext(_currentTuple)));

            rotationSequence.OnComplete(() => _isInteractable = true);
        }

        #endregion
    }
}
