using HexagonGencer.Enums;
using HexagonGencer.Game.Core.Concrete;
using UniRx;
using UnityEngine;

namespace HexagonGencer.Game.Models.Abstract
{
    public interface IItem
    {
        ItemColor ItemColor { get; set; }

        Cell Cell { get; set; }

        IntReactiveProperty SortingOrder { get; set; }

        Transform Transform { get; set; }

        void SetRandomColor();

        void Execute();
    }
}