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

        private Subject<Unit> _onSwipeLeft = new Subject<Unit>();
        public IObservable<Unit> OnSwipeLeft
        {
            get { return _onSwipeLeft; }
        }

        private Subject<Unit> _onSwipeRight = new Subject<Unit>();
        public IObservable<Unit> OnSwipeRight
        {
            get { return _onSwipeRight; }
        }

        private Subject<Unit> _onSwipeDown = new Subject<Unit>();
        public IObservable<Unit> OnSwipeDown
        {
            get { return _onSwipeDown; }
        }

        private Subject<Unit> _onSwipeUp = new Subject<Unit>();
        public IObservable<Unit> OnSwipeUp
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
                .Subscribe(_ =>
                {
                    Debug.Log("Swipe Left");
                    _onSwipeLeft.OnNext(Unit.Default);

                }).AddTo(_disposables);

            endDragStream
                .Where(position =>
                {
                    var diffrence = (position - _startPosition).normalized;
                    return diffrence.y > -0.5 && diffrence.y < 0.5;
                })
                .Where(position => position.x > _startPosition.x)
                .Where(position => Mathf.Abs(position.x - _startPosition.x) >= _minDistance)
                .Subscribe(_ =>
                {
                    Debug.Log("Swipe Right");
                    _onSwipeRight.OnNext(Unit.Default);

                }).AddTo(_disposables);

            endDragStream
                .Where(position =>
                {
                    var diffrence = (position - _startPosition).normalized;
                    return diffrence.x > -0.5 && diffrence.x < 0.5;
                })
                .Where(position => _startPosition.y > position.y)
                .Where(position => Mathf.Abs(_startPosition.y - position.y) >= _minDistance)
                .Subscribe(_ =>
                {
                    Debug.Log("Swipe Down");
                    _onSwipeDown.OnNext(Unit.Default);

                }).AddTo(_disposables);

            endDragStream
                .Where(position =>
                {
                    var diffrence = (position - _startPosition).normalized;
                    return diffrence.x > -0.5 && diffrence.x < 0.5;
                })
                .Where(position => position.y > _startPosition.y)
                .Where(position => Mathf.Abs(position.y - _startPosition.y) >= _minDistance)
                .Subscribe(_ =>
                {
                    Debug.Log("Swipe Up");
                    _onSwipeUp.OnNext(Unit.Default);

                }).AddTo(_disposables);

            pointerUpStream
                .Where(position =>
                {
                    var diffrence = (position - _startPosition).magnitude;
                    return diffrence < 10f;
                })
                .Subscribe(position =>
                {
                    Debug.Log("Click");
                    _onClick.OnNext(position);

                }).AddTo(_disposables);
        }

        #endregion

        private void OnDisable()
        {
            _disposables.Dispose();
        }
    }
}
