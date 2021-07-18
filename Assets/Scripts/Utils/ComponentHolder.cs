using UnityEngine.UI;
using UnityEngine;

public class ComponentHolder : MonoBehaviour
{
    [Header("Main Menu Scene Views")]
    public Text WidthText;
    public Text HeightText;
    public Text ColorText;
    [Space(10)]
    public Button BackButton;
    public Button PlayButton;
    public Button SettingsButton;
    public Button BackToMenuButton;
    [Space(10)]
    public Slider WidthSlider;
    public Slider HeightSlider;
    public Slider ColorSlider;
    [Space(10)]
    public GameObject MainMenuPanel;
    public GameObject SettingsPanel;

    [Header("Game Scene Views ")]
    public Text ScoreText;
    public Text MovesText;
    [Space(10)]
    public Button RestartButton;
    public Button MainMenuButton;
    [Space(10)]
    public GameObject GameOverPanel;
}
