using HexagonGencer.Game.Models.Concrete;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace HexagonGencer.Game.Presenters
{

    public class GameUIPresenter
    {
        #region Model 

        public GameUIModel Model;

        #endregion

        #region Views

        public Text ScoreText;
        public Text MovesText;
        public Button RestartButton;
        public Button MainMenuButton;
        public Button BackToMenuButton;

        #endregion

        #region Constructer

        public GameUIPresenter() { }

        public GameUIPresenter(Text scoreText, Text movesText, Button restartButton, Button mainMenuButton, Button backToMenu)
        {
            ScoreText = scoreText;
            MovesText = movesText;
            RestartButton = restartButton;
            MainMenuButton = mainMenuButton;
            BackToMenuButton = backToMenu;
            BindView();
        }

        #endregion

        #region Binding

        public void BindView()
        {
            Model.Score.SubscribeToText(ScoreText);
            Model.Moves.SubscribeToText(MovesText);

            RestartButton.onClick.AsObservable().Subscribe(_ =>
            {
                Model.OnRestartButtonClicked.OnNext(Unit.Default);
            });

            MainMenuButton.onClick.AsObservable().Subscribe(_ =>
            {
                Model.OnMainMenuButtonClicked.OnNext(Unit.Default);
            });

            BackToMenuButton.onClick.AsObservable().Subscribe(_ =>
            {
                Model.OnBackToMenuButtonClicked.OnNext(Unit.Default);
            });
        }

        #endregion
    }
}
