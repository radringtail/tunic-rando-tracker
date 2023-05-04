using System.Collections.Generic;

namespace TunicRandoTracker
{
    internal class ItemTracker
    {
        public int Seed { get; set; }

        public Dictionary<string, int> ImportantItems { get; set; }
    }
}
