using UniRx;

namespace HexagonGencer.Game.Controller.Abstract
{
    public abstract class SceneController
    {
        #region Fields

        public abstract BoolReactiveProperty ShouldRenderNewScene { get; set; }
        public abstract BoolReactiveProperty ShouldRestartScene { get; set; }

        #endregion

        #region Virtual Methods

        public abstract void InitializeScene();

        #endregion
    }

}