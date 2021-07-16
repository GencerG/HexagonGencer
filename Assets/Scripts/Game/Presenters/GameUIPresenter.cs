using HexagonGencer.Game.Models.Concrete;
using UniRx;
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

        #endregion

        #region Constructer

        public GameUIPresenter() { }

        public GameUIPresenter(Text scoreText, Text movesText)
        {
            ScoreText = scoreText;
            MovesText = movesText;
            BindView();
        }

        #endregion

        #region Bind

        public void BindView()
        {
            Model.Score.SubscribeToText(ScoreText);
            Model.Moves.SubscribeToText(MovesText);
        }

        #endregion
    }
}
