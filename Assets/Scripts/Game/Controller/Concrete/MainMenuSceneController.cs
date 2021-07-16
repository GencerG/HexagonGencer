using UniRx;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using HexagonGencer.Game.Controller.Abstract;
using HexagonGencer.Game.Models.Concrete;
using HexagonGencer.Game.Presenters;
using HexagonGencer.Utils;

public class MainMenuSceneController : SceneController
{
    private MainMenuUIModel _mainMenuUIModel = new MainMenuUIModel();
    private GameObject _settingsPanel;
    public override BoolReactiveProperty ShouldRenderNewScene { get; set; } = new BoolReactiveProperty(false);

    public override void InitializeScene()
    {
        GetReferences();
        InitializeUI();
    }

    private void InitializeUI()
    {
        var presenter = new MainMenuPresenter
        {
            WidthText = GameObject.Find("WidthText").GetComponent<Text>(),
            HeightText = GameObject.Find("HeightText").GetComponent<Text>(),
            ColorText = GameObject.Find("ColorText").GetComponent<Text>(),
            BackButton = GameObject.Find("BackButton").GetComponent<Button>(),
            PlayButton = GameObject.Find("PlayButton").GetComponent<Button>(),
            SettingsButton = GameObject.Find("SettingsButton").GetComponent<Button>(),
            WidthSlider = GameObject.Find("WidthSlider").GetComponent<Slider>(),
            HeightSlider = GameObject.Find("HeightSlider").GetComponent<Slider>(),
            ColorSlider = GameObject.Find("ColorSlider").GetComponent<Slider>(),
            Model = _mainMenuUIModel
        };

        presenter.BindView();

        _mainMenuUIModel.OnPlayButtonClicked.Subscribe(_ =>
        {
            ShouldRenderNewScene.Value = true;
        });

        _mainMenuUIModel.OnSettingsButtonClicked.Subscribe(HandleOnSettingsButtonClick);

        _mainMenuUIModel.OnBackButtonClicked.Subscribe(HandleOnBackButtonClicked);

        _mainMenuUIModel.OnHeightValueChanged.Subscribe(HandleOnHeightChanged);

        _mainMenuUIModel.OnWidthValueChanged.Subscribe(HandleOnWidthChanaged);

        _mainMenuUIModel.OnNumberOfColorsValueChanged.Subscribe(HandleOnNumberOfColorsChanged);

        presenter.ColorSlider.value = HexagonGencerUtils.GameSettings.NUMBER_OF_COLORS;
        presenter.WidthSlider.value = HexagonGencerUtils.GameSettings.BOARD_WIDTH;
        presenter.HeightSlider.value = HexagonGencerUtils.GameSettings.BOARD_HEIGHT;
    }

    private void HandleOnSettingsButtonClick(Unit unit)
    {
        _settingsPanel.transform.DOScale(1f, .3f).SetEase(Ease.OutBack);
    }

    private void HandleOnBackButtonClicked(Unit unit)
    {
        _settingsPanel.transform.localScale = Vector3.zero;
    }

    private void HandleOnWidthChanaged(int width)
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

    private void GetReferences()
    {
        _settingsPanel = GameObject.Find("SettingsPanel");
    }

}