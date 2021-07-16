using UniRx;
using UnityEngine;

namespace HexagonGencer.Game.Models.Concrete
{
    public class MainMenuUIModel
    {
        public IntReactiveProperty BoardWidth = new IntReactiveProperty(8);
        public IntReactiveProperty BoardHeight = new IntReactiveProperty(9);
        public IntReactiveProperty NumberOfColors = new IntReactiveProperty(7);

        public Subject<Unit> OnPlayButtonClicked = new Subject<Unit>();
        public Subject<Unit> OnSettingsButtonClicked = new Subject<Unit>();
        public Subject<Unit> OnBackButtonClicked = new Subject<Unit>();
        public Subject<int> OnWidthValueChanged = new Subject<int>();
        public Subject<int> OnHeightValueChanged = new Subject<int>();
        public Subject<int> OnNumberOfColorsValueChanged = new Subject<int>();
    }
}
