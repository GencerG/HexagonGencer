using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using HexagonGencer.Game.Controller.Concrete;

namespace HexagonGencer.Game.Manager
{
    public class GameManager : MonoBehaviour
    {
        #region Fields

        private GameSceneController _gameSceneController 
            = new GameSceneController();

        private MainMenuSceneController _mainMenuSceneController 
            = new MainMenuSceneController();

        #endregion

        #region Unity

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        #endregion

        #region Scene Management

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case 0:
                    _mainMenuSceneController.InitializeScene();
                    _mainMenuSceneController.ShouldRenderNewScene.Where(newScene => newScene == true)
                        .Subscribe(_ =>
                        {
                            SceneManager.LoadScene(1);
                        });
                    break;

                case 1:
                    _gameSceneController.InitializeScene();
                    _gameSceneController.ShouldRenderNewScene.Where(newScene => newScene == true)
                        .Subscribe(_ =>
                        {
                            SceneManager.LoadScene(0);
                        });
                    break;
            }
        }

        #endregion
    }
}