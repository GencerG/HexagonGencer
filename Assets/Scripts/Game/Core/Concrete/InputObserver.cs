using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace HexagonGencer.Game.Core.Concrete
{
    public class InputObserver : ObservableEventTrigger
    {
        #region Fields

        [SerializeField] private float _minTime = 1;
        [SerializeField] private float _minDistance = 100.0f;

        private Vector2 _startPosition;
        private DateTime _startTime;

        private CompositeDisposable _disposables = new CompositeDisposable();

        #endregion

        #region Subjects

        private Subject<Vector2> _onSwipeLeft = new Subject<Vector2>();
        public IObservable<Vector2> OnSwipeLeft
        {
            get { return _onSwipeLeft; }
        }

        private Subject<Vector2> _onSwipeRight = new Subject<Vector2>();
        public IObservable<Vector2> OnSwipeRight
        {
            get { return _onSwipeRight; }
        }

        private Subject<Vector2> _onSwipeDown = new Subject<Vector2>();
        public IObservable<Vector2> OnSwipeDown
        {
            get { return _onSwipeDown; }
        }

        private Subject<Vector2> _onSwipeUp = new Subject<Vector2>();
        public IObservable<Vector2> OnSwipeUp
        {
            get { return _onSwipeUp; }
        }

        private Subject<Vector2> _onClick = new Subject<Vector2>();
        public IObservable<Vector2> OnClick
        {
            get { return _onClick; }
        }

        #endregion

        #region Unity

        private void OnEnable()
        {
            var beginDragStream = this
                .OnBeginDragAsObservable()
                .TakeUntilDisable(this)
                .Select(eventData => eventData.position)
                .Subscribe(position =>
                {
                    this._startPosition = position;
                    this._startTime = DateTime.Now;

                }).AddTo(_disposables);

            var endDragStream = this
                .OnEndDragAsObservable()
                .TakeUntilDisable(this)
                .Where(_ => (DateTime.Now - _startTime).TotalSeconds < _minTime)
                .Select(eventData => eventData.position)
                .Share();

            var pointerDownStream = this
                .OnPointerDownAsObservable()
                .TakeUntilDisable(this)
                .Select(eventData => eventData.position)
                .Subscribe(position =>
                {

                    this._startPosition = position;

                }).AddTo(_disposables);

            var pointerUpStream = this
                .OnPointerUpAsObservable()
                .TakeUntilDisable(this)
                .Select(eventData => eventData.position)
                .Share();

            endDragStream
                .Where(position =>
                {
                    var diffrence = (position - _startPosition).normalized;
                    return diffrence.y > -0.5 && diffrence.y < 0.5;
                })
                .Where(position => _startPosition.x > position.x)
                .Where(position => Mathf.Abs(_startPosition.x - position.x) >= _minDistance)
                .Subscribe(position =>
                {
                    _onSwipeLeft.OnNext(position);

                }).AddTo(_disposables);

            endDragStream
                .Where(position =>
                {
                    var diffrence = (position - _startPosition).normalized;
                    return diffrence.y > -0.5 && diffrence.y < 0.5;
                })
                .Where(position => position.x > _startPosition.x)
                .Where(position => Mathf.Abs(position.x - _startPosition.x) >= _minDistance)
                .Subscribe(position =>
                {
                    _onSwipeRight.OnNext(position);

                }).AddTo(_disposables);

            endDragStream
                .Where(position =>
                {
                    var diffrence = (position - _startPosition).normalized;
                    return diffrence.x > -0.5 && diffrence.x < 0.5;
                })
                .Where(position => _startPosition.y > position.y)
                .Where(position => Mathf.Abs(_startPosition.y - position.y) >= _minDistance)
                .Subscribe(position =>
                {
                    _onSwipeDown.OnNext(position);

                }).AddTo(_disposables);

            endDragStream
                .Where(position =>
                {
                    var diffrence = (position - _startPosition).normalized;
                    return diffrence.x > -0.5 && diffrence.x < 0.5;
                })
                .Where(position => position.y > _startPosition.y)
                .Where(position => Mathf.Abs(position.y - _startPosition.y) >= _minDistance)
                .Subscribe(position =>
                {
                    _onSwipeUp.OnNext(position);

                }).AddTo(_disposables);

            pointerUpStream
                .Where(position =>
                {
                    var diffrence = (position - _startPosition).magnitude;
                    return diffrence < 10f;
                })
                .Subscribe(position =>
                {
                    _onClick.OnNext(position);

                }).AddTo(_disposables);
        }

        private void OnDisable()
        {
            _disposables.Dispose();
        }

        #endregion

    }
}
