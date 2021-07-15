using HexagonGencer.Game.Controller.Abstract;
using UniRx;
using UnityEngine;

namespace HexagonGencer.Game.Controller.Concrete
{
    public partial class GameSceneController : SceneController
    {
        #region Scene Controller

        public override BoolReactiveProperty ShouldRenderNewScene { get; set; }
            = new BoolReactiveProperty(false);

        public override void InitializeScene()
        {

        }

        #endregion
    }
}
