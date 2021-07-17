using HexagonGencer.Game.Models.Abstract;

namespace HexagonGencer.Game.Models.Concrete
{
    public class GameObjectAssetModel : IAssetModel
    {
        public string Path { get; set; }

        public GameObjectAssetModel(string path)
        {
            this.Path = path;
        }

        public static GameObjectAssetModel HexagonPrefab
        {
            get
            {
                return new GameObjectAssetModel("Prefabs/Hex");
            }
        }

        public static GameObjectAssetModel OutlinePrefab
        {
            get
            {
                return new GameObjectAssetModel("Prefabs/Outline");
            }
        }

        public static GameObjectAssetModel CellPrefab
        {
            get
            {
                return new GameObjectAssetModel("Prefabs/Cell");
            }
        }

        public static GameObjectAssetModel HexagonBomb
        {
            get
            {
                return new GameObjectAssetModel("Prefabs/Bomb");
            }
        }
    }
}
