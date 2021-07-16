using UnityEngine.UI;
using HexagonGencer.Game.Models.Concrete;
using HexagonGencer.Game.Presenters;
using UnityEngine;

namespace HexagonGencer.Game.Controller.Concrete
{
    public partial class GameSceneController
    {
        #region Model

        private GameUIModel _gameUIModel
            = new GameUIModel();

        #endregion

        #region Bindging

        private void BindUIManager()
        {
            var presenter = new GameUIPresenter
            {
                ScoreText = GameObject.Find("ScoreText").GetComponent<Text>(),
                MovesText = GameObject.Find("MovesText").GetComponent<Text>(),
                Model = _gameUIModel,
            };

            presenter.BindView();
        }

        #endregion

    }
}