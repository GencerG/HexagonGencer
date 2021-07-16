using UnityEngine;

namespace HexagonGencer.Game.Scripables
{
    [CreateAssetMenu(fileName = "NewSettings", menuName = "Settings/Create Game Settings")]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private int _boardWidth;
        [SerializeField] private int _boardHeight;
        [SerializeField] private int _numberOfColors;

        public int BOARD_WIDTH{ get { return _boardWidth; } set { } }
        public int BOARD_HEIGHT { get { return _boardHeight; } set { } }
        public int NUMBER_OF_COLORS { get { return _numberOfColors; } set { } }
    }
}