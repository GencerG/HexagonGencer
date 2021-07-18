using HexagonGencer.Game.Models.Concrete;
using HexagonGencer.Game.Presenters;
using UnityEngine;
using UniRx;
using HexagonGencer.Utils;
using DG.Tweening;

namespace HexagonGencer.Game.Controller.Concrete
{
    public partial class GameSceneController
    {
        #region Model

        private GameUIModel _gameUIModel
            = new GameUIModel();

        private MainMenuUIModel _mainMenuUIModel
            = new MainMenuUIModel();

        #endregion

        #region Fields

        private GameObject _gameOverPanel;
        private GameObject _mainMenuPanel;
        private GameObject _settingsPanel;
        private GameObject _backToMenuButton;

        #endregion

        #region Binding

        public void BindUIManager()
        {
            var componentHolderInstance = GameObject.FindWithTag(HexagonGencerUtils.COMPONENT_HOLDER_TAG)
                .GetComponent<ComponentHolder>();

            var mainMenupresenter = new MainMenuPresenter
            {
                WidthText = componentHolderInstance.WidthText,
                HeightText = componentHolderInstance.HeightText,
                ColorText = componentHolderInstance.ColorText,
                BackButton = componentHolderInstance.BackButton,
                PlayButton = componentHolderInstance.PlayButton,
                SettingsButton = componentHolderInstance.SettingsButton,
                WidthSlider = componentHolderInstance.WidthSlider,
                HeightSlider = componentHolderInstance.HeightSlider,
                ColorSlider = componentHolderInstance.ColorSlider,
                Model = _mainMenuUIModel
            };

            mainMenupresenter.BindView();

            _mainMenuUIModel.OnPlayButtonClicked.Subscribe(HandleOnPlayButtonClicked);
            _mainMenuUIModel.OnSettingsButtonClicked.Subscribe(HandleOnSettingsButtonClick);
            _mainMenuUIModel.OnBackButtonClicked.Subscribe(HandleOnBackButtonClicked);
            _mainMenuUIModel.OnHeightValueChanged.Subscribe(HandleOnHeightChanged);
            _mainMenuUIModel.OnWidthValueChanged.Subscribe(HandleOnWidthChanged);
            _mainMenuUIModel.OnNumberOfColorsValueChanged.Subscribe(HandleOnNumberOfColorsChanged);

            mainMenupresenter.ColorSlider.value = HexagonGencerUtils.GameSettings.NUMBER_OF_COLORS;
            mainMenupresenter.WidthSlider.value = HexagonGencerUtils.GameSettings.BOARD_WIDTH;
            mainMenupresenter.HeightSlider.value = HexagonGencerUtils.GameSettings.BOARD_HEIGHT;

            var gameUIPresenter = new GameUIPresenter
            {
                ScoreText = componentHolderInstance.ScoreText,
                MovesText = componentHolderInstance.MovesText,
                RestartButton = componentHolderInstance.RestartButton,
                MainMenuButton = componentHolderInstance.MainMenuButton,
                BackToMenuButton = componentHolderInstance.BackToMenuButton,
                Model = _gameUIModel,
            };

            gameUIPresenter.BindView();

            _gameUIModel.OnMainMenuButtonClicked.Subscribe(HandleOnMainMenuButtonClicked);
            _gameUIModel.OnRestartButtonClicked.Subscribe(HandleOnRestartButtonClicked);
            _gameUIModel.OnBackToMenuButtonClicked.Subscribe(HandleOnMainMenuButtonClicked);

            _settingsPanel = componentHolderInstance.SettingsPanel;
            _mainMenuPanel = componentHolderInstance.MainMenuPanel;
            _gameOverPanel = componentHolderInstance.GameOverPanel;
            _backToMenuButton = componentHolderInstance.BackToMenuButton.gameObject;
        }

        #endregion

        #region Handles

        private void HandleOnSettingsButtonClick(Unit unit)
        {
            _settingsPanel.transform.DOScale(1f, .3f).SetEase(Ease.OutBack);
        }

        private void HandleOnBackButtonClicked(Unit unit)
        {
            _settingsPanel.transform.localScale = Vector3.zero;
        }

        private void HandleOnPlayButtonClicked(Unit unit)
        {
            InitializeBoard();
            SetActivePanel(_backToMenuButton.transform.name);
        }

        private void HandleOnWidthChanged(int width)
        {
            HexagonGencerUtils.GameSettings.BOARD_WIDTH = width;
        }

        private void HandleOnHeightChanged(int height)
        {
            HexagonGencerUtils.GameSettings.BOARD_HEIGHT = height;
        }

        private void HandleOnNumberOfColorsChanged(int numColor)
        {
            HexagonGencerUtils.GameSettings.NUMBER_OF_COLORS = numColor;
        }

        private void HandleOnRestartButtonClicked(Unit unit)
        {
            RestartLevel();
        }

        private void HandleOnMainMenuButtonClicked(Unit unit)
        {
            BackToMainMenu();
        }

        #endregion

        #region Custom Methods

        private void RestartLevel()
        {
            DestroyBoard();
            ObjectPool.Clear();
            InitializeBoard();
            ResetScore();
            SetActivePanel(_backToMenuButton.transform.name);
        }

        private void BackToMainMenu()
        {
            DestroyBoard();
            ObjectPool.Clear();
            ResetScore();
            SetActivePanel(_mainMenuPanel.transform.name);
        }

        private void SetActivePanel(string activePanel)
        {
            _gameOverPanel.SetActive(activePanel.Equals(_gameOverPanel.name));
            _mainMenuPanel.SetActive(activePanel.Equals(_mainMenuPanel.name));
            _backToMenuButton.SetActive(activePanel.Equals(_backToMenuButton.name));
        }

        private void ResetScore()
        {
            _gameUIModel.Score.Value = 0;
            _gameUIModel.Moves.Value = 0;
        }

        #endregion

    }
}
