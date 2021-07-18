using HexagonGencer.Game.Core.Abstract;
using HexagonGencer.Utils;
using UniRx;
using UnityEngine;

namespace HexagonGencer.Game.Core.Concrete
{
    public class HexagonBomb : Item
    {
        #region Fields

        [SerializeField] private TextMesh _countDownText;

        public int Health;
        private bool _isJustSpawned = true;

        #endregion                                 

        public override bool Execute()
        {
            if (!_isJustSpawned)
            {
                Health--;
            }
            else
            {
                _isJustSpawned = false;
            }

            SetText();

            return Health > 0;
        }

        #region Unity

        private void OnEnable()
        {
            _isJustSpawned = true;
            transform.rotation = Quaternion.identity;
        }

        private void Start()
        {
            Health = HexagonGencerUtils.BOMB_HEALTH;
            SetText();

            SortingOrder.Subscribe(order =>
            {
                spriteRenderer.sortingOrder = order;
            });
        }

        private void SetText()
        {
            _countDownText.text = Health.ToString();
        }

        #endregion
    }
}