using HexagonGencer.Utils;
using UniRx;
using UnityEngine;

namespace HexagonGencer.Game.Controller.Concrete
{
    public partial class GameSceneController
    {
        private void BindGridManager()
        {
            _onStartRotating.Subscribe(tuple =>
            {
                var cell1 = tuple.Item1.Cell;
                var cell2 = tuple.Item2.Cell;
                var cell3 = tuple.Item3.Cell;

                if (_rotationDirection == HexagonGencerUtils.CLOCK_WISE)
                {

                    cell1.UpdateHexagon(tuple.Item3);
                    cell2.UpdateHexagon(tuple.Item1);
                    cell3.UpdateHexagon(tuple.Item2);
                }

                else
                {
                    cell1.UpdateHexagon(tuple.Item2);
                    cell2.UpdateHexagon(tuple.Item3);
                    cell3.UpdateHexagon(tuple.Item1);
                }

                
                tuple.Item1.Transform.GetChild(0).GetComponent<TextMesh>().text = tuple.Item1.Cell.Index.ToString();
                tuple.Item2.Transform.GetChild(0).GetComponent<TextMesh>().text = tuple.Item2.Cell.Index.ToString();
                tuple.Item3.Transform.GetChild(0).GetComponent<TextMesh>().text = tuple.Item3.Cell.Index.ToString();
            });
        }
    }
}
