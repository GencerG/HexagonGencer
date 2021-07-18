using HexagonGencer.Game.Models.Concrete;
using UniRx;
using UnityEngine.UI;

namespace HexagonGencer.Game.Presenters
{
    public class MainMenuPresenter
    {
        #region Model

        public MainMenuUIModel Model;

        #endregion

        #region Views 

        public Text WidthText;
        public Text HeightText;
        public Text ColorText;
        public Button PlayButton;
        public Button SettingsButton;
        public Button BackButton;
        public Slider WidthSlider;
        public Slider HeightSlider;
        public Slider ColorSlider;

        #endregion

        #region Constructer

        public MainMenuPresenter() { }

        public MainMenuPresenter(Text widthText, Text heightText, Text numberOfColorsText, Button playButton, Button settingsButton, Button backButton)
        {
            WidthText = widthText;
            HeightText = heightText;
            ColorText = numberOfColorsText;
            PlayButton = playButton;
            SettingsButton = settingsButton;
            BackButton = backButton;

            BindView();
        }

        #endregion

        #region Binding

        public void BindView()
        {
            Model.BoardHeight.Subscribe(height =>
            {
                HeightText.text = height.ToString();
                Model.OnHeightValueChanged.OnNext(height);
            });

            Model.BoardWidth.Subscribe(width =>
            {
                WidthText.text = width.ToString();
                Model.OnWidthValueChanged.OnNext(width);
            });

            Model.NumberOfColors.Subscribe(numColors =>
            {
                ColorText.text = numColors.ToString();
                Model.OnNumberOfColorsValueChanged.OnNext(numColors);
            });

            PlayButton.OnClickAsObservable().AsObservable().Subscribe(_ =>
            {
                Model.OnPlayButtonClicked.OnNext(Unit.Default);
            });

            SettingsButton.OnClickAsObservable().Subscribe(_ =>
            {
                Model.OnSettingsButtonClicked.OnNext(Unit.Default);
            });

            BackButton.OnClickAsObservable().Subscribe(_ =>
            {
                Model.OnBackButtonClicked.OnNext(Unit.Default);
            });

            WidthSlider.onValueChanged.AsObservable().Subscribe(value =>
            {
                Model.BoardWidth.Value = (int)value;
            });

            HeightSlider.onValueChanged.AsObservable().Subscribe(value =>
            {
                Model.BoardHeight.Value = (int)value;
            });

            ColorSlider.onValueChanged.AsObservable().Subscribe(value =>
            {
                Model.NumberOfColors.Value = (int)value;
            });
        }

        #endregion
    }
}
