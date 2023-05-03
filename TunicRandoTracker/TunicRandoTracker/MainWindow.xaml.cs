// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Dispatching;

namespace TunicRandoTracker
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public const string itemTrackerFolderPath = @"C:\Users\Radi\AppData\LocalLow\Andrew Shouldice\Secret Legend\Randomizer";
        public const string itemTrackerPath = @"C:\Users\Radi\AppData\LocalLow\Andrew Shouldice\Secret Legend\Randomizer\ItemTracker.json";
        public static FileSystemWatcher FileWatcher = new FileSystemWatcher();
        public static DispatcherQueue uiThread = DispatcherQueue.GetForCurrentThread();

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                Title = "TUNIC Randomizer Tracker v0.1";
                ParseRando();

                FileWatcher.Path = itemTrackerFolderPath;
                FileWatcher.NotifyFilter = NotifyFilters.LastWrite;
                FileWatcher.Changed += new FileSystemEventHandler(OnChanged);
                FileWatcher.Filter = "*.json";
                FileWatcher.IncludeSubdirectories = false;
                FileWatcher.EnableRaisingEvents = true;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public void OnChanged(object sender, FileSystemEventArgs e)
        {
            int milliseconds = 1000;
            Thread.Sleep(milliseconds);
            ParseRando();
        }

        public void ParseRando()
        {
            var trackerFile = JObject.Parse(File.ReadAllText(itemTrackerPath));
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
            ItemCollected(currentTracker, VaultKey, "Vault Key (Red)");
            ItemCollected(currentTracker, HexRed, "Hexagon Red");
            ItemCollected(currentTracker, HexBlue, "Hexagon Blue");
            ItemCollected(currentTracker, HexGreen, "Hexagon Green");

            // count collected items
            QuantityCollected(currentTracker, Coins, CoinsCount, "Trinket Coin");
            QuantityCollected(currentTracker, Slots, SlotsCount, "Trinket Slot");
            QuantityCollected(currentTracker, Cards, CardsCount, "Trinket Cards");
            QuantityCollected(currentTracker, Pages, PagesCount, "Pages");
            QuantityCollected(currentTracker, Fairies, FairiesCount, "Fairies");
            QuantityCollected(currentTracker, Trophies, TrophiesCount, "Golden Trophies");

            // resolve relics (upgrade count + hero relic)
            RelicUpdate(currentTracker, ATT, ATTCount, "Upgrade Offering - Attack - Tooth", "Relic - Hero Sword", "att");
            RelicUpdate(currentTracker, DEF, DEFCount, "Upgrade Offering - DamageResist - Effigy", "Relic - Hero Crown", "def");
            RelicUpdate(currentTracker, POTION, POTIONCount, "Upgrade Offering - PotionEfficiency Swig - Ash", "Relic - Hero Water", "potion");
            RelicUpdate(currentTracker, HP, HPCount, "Upgrade Offering - Health HP - Flower", "Relic - Hero Pendant HP", "hp");
            RelicUpdate(currentTracker, SP, SPCount, "Upgrade Offering - Magic MP - Mushroom", "Relic - Hero Pendant SP", "sp");
            RelicUpdate(currentTracker, MP, MPCount, "Upgrade Offering - Stamina SP - Feather", "Relic - Hero Pendant MP", "mp");

            //sword progression
            var swordProg = currentTracker.ImportantItems.SingleOrDefault(x => x.Key == "Sword Progression");
            uiThread.TryEnqueue(() =>
            {
                switch (swordProg.Value)
                {
                    case 3:
                        Sword.Source = new BitmapImage(new Uri($"ms-appx:///assets/items/sword3.png"));
                        break;
                    case 4:
                        Sword.Source = new BitmapImage(new Uri($"ms-appx:///assets/items/sword4.png"));
                        break;
                }
            });
        }

        private static void ItemCollected(ItemTracker current, Image item, string randoName)
        {
            uiThread.TryEnqueue(() =>
            {
                var foundItem = current.ImportantItems.SingleOrDefault(x => x.Key == randoName);
                item.Opacity = foundItem.Value > 0 ? 1 : 0.5;
            });
        }

        private static void QuantityCollected(ItemTracker current, Image item, TextBlock itemCount, string randoName)
        {
            uiThread.TryEnqueue(() =>
            {
                var foundItem = current.ImportantItems.SingleOrDefault(x => x.Key == randoName);
                item.Opacity = foundItem.Value > 0 ? 1 : 0.5;
                itemCount.Text = foundItem.Value.ToString();
            });
        }

        private static void RelicUpdate(ItemTracker current, Image item, TextBlock itemCount, string statName, string relicName, string code)
        {
            uiThread.TryEnqueue(() =>
            {
                // updates based on collected upgrades
                var foundStat = current.ImportantItems.SingleOrDefault(x => x.Key == statName);
                item.Opacity = foundStat.Value > 0 ? 1 : 0.5;
                itemCount.Text = foundStat.Value.ToString();

                // updates based on collected upgrades
                var foundRelic = current.ImportantItems.SingleOrDefault(x => x.Key == relicName);
                item.Source = foundRelic.Value > 0 ? new BitmapImage(new Uri($"ms-appx:///assets/items/relic-{code}2.png")) : item.Source;
            });
        }
    }
}
