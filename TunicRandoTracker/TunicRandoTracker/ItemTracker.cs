using System.Collections.Generic;

namespace TunicRandoTracker
{
    internal class ItemTracker
    {
        public int Seed { get; set; }

        public struct SceneInfo
        {
            public int SceneId;
            public string SceneName;
            public ColorPalette Fur;
            public ColorPalette Puff;
            public ColorPalette Details;
            public ColorPalette Tunic;
            public ColorPalette Scarf;
        }

        public SceneInfo CurrentScene { get; set; }

        public Dictionary<string, int> ImportantItems { get; set; }

        public List<ItemData> ItemsCollected { get; set; }
    }


    internal class ColorPalette
    {
        public string HexValue { get; set; }
        public string ColorName { get; set; }
    }

    internal class ItemData
    {
        public Reward Reward { get; set; }
        public Location Location { get; set; }
    }

    public class Reward
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Amount { get; set; }
    }

    public class Location
    {
        public string LocationId { get; set; }
        public string SceneName { get; set; }
        public int SceneId { get; set; }
        public string Position { get; set; }
        public List<string> RequiredItems { get; set; }
    }
}
