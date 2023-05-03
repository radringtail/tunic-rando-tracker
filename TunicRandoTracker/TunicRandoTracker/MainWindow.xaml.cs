// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using ThreadingTimer = System.Threading.Timer;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace TunicRandoTracker
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Title = "TUNIC Randomizer Tracker v0.1";
            ParseRando();
        }

        private void ParseRando()
        {
            var trackerFile = JObject.Parse(File.ReadAllText(@"C:\Users\Radi\AppData\LocalLow\Andrew Shouldice\Secret Legend\Randomizer\ItemTracker.json"));
            var currentTracker = JsonConvert.DeserializeObject<ItemTracker>(trackerFile.ToString());

            // check togglable items 
            ItemCollected(currentTracker, Stick, "Stick");
            ItemCollected(currentTracker, Sword, "Sword");
            ItemCollected(currentTracker, Dagger, "Stundagger");
            ItemCollected(currentTracker, Wand, "Techbow");
            ItemCollected(currentTracker, Orb, "Wand");
            ItemCollected(currentTracker, Laurels, "Hyperdash");
            ItemCollected(currentTracker, Lantern, "Lantern");
            ItemCollected(currentTracker, Shield, "Shield");
            ItemCollected(currentTracker, Hourglass, "SlowmoItem");
            ItemCollected(currentTracker, Gun, "Shotgun");
            ItemCollected(currentTracker, Dath, "Dath Stone");
            ItemCollected(currentTracker, HouseKey, "Key (House)");
            ItemCollected(currentTracker, VaultKey, "Shotgun");
            ItemCollected(currentTracker, HexRed, "Hexagon Red");
            ItemCollected(currentTracker, HexBlue, "Hexagon Blue");
            ItemCollected(currentTracker, HexGreen, "Hexagon Green");

            // count collected items
            QuantityCollected(currentTracker, Coins, CoinsCount, "Trinket Coin");
            QuantityCollected(currentTracker, Slots, SlotsCount, "Trinket Slot");
            QuantityCollected(currentTracker, Cards, CardsCount, "Trinket Cards");
        }

        private static void ItemCollected(ItemTracker current, Image item, string randoName)
        {
            var foundItem = current.ImportantItems.SingleOrDefault(x => x.Key == randoName);
            item.Opacity = foundItem.Value > 0 ? 1 : 0.5;
        }

        private static void QuantityCollected(ItemTracker current, Image item, TextBlock itemCount, string randoName)
        {
            var foundItem = current.ImportantItems.SingleOrDefault(x => x.Key == randoName);
            item.Opacity = foundItem.Value > 0 ? 1 : 0.5;
            itemCount.Text = foundItem.Value.ToString();
        }
    }
}
