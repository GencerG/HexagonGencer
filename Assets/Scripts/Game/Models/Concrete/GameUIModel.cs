using UniRx;

namespace HexagonGencer.Game.Models.Concrete
{
    public class GameUIModel
    {
        public IntReactiveProperty Score = new IntReactiveProperty(0);
        public IntReactiveProperty Moves = new IntReactiveProperty(0);
    }
}
