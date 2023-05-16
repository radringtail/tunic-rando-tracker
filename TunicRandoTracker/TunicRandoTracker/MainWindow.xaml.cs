// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Dispatching;
using System.Text.Json;
using WinUIEx;

namespace TunicRandoTracker
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public const string itemTrackerFolder = @"%UserProfile%\AppData\LocalLow\Andrew Shouldice\Secret Legend\Randomizer";
        public const string itemTrackerFile = @"%UserProfile%\AppData\LocalLow\Andrew Shouldice\Secret Legend\Randomizer\ItemTracker.json";
        public static FileSystemWatcher FileWatcher = new FileSystemWatcher();
        public static DispatcherQueue uiThread = DispatcherQueue.GetForCurrentThread();

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                Title = "TUNIC Randomizer Tracker v1.0.0";
                this.SetWindowSize(1216, 239); // 12x2

                // fire once
                ParseRando();

                FileWatcher.Path = Environment.ExpandEnvironmentVariables(itemTrackerFolder);
                FileWatcher.NotifyFilter = NotifyFilters.LastWrite;
                FileWatcher.Changed += new FileSystemEventHandler(OnChanged);
                FileWatcher.Filter = "*.json";
                FileWatcher.IncludeSubdirectories = false;
                FileWatcher.EnableRaisingEvents = true;
            }
            catch (Exception ex)
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
        private void SizeClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            switch (button.Name)
            {
                case "Size2":
                    this.SetWindowSize(1216, 239); // 12x2
                    break;
                case "Size3":
                    this.SetWindowSize(816, 339); // 8x3
                    break;
                case "Size4":
                    this.SetWindowSize(616, 439); // 6x4
                    break;
                case "Size5":
                    this.SetWindowSize(516, 539); // 5x5
                    break;
                case "Size6":
                    this.SetWindowSize(416, 639); // 4x6
                    break;
                case "Size8":
                    this.SetWindowSize(316, 839); // 3x8
                    break;
            }
        }
        public void ParseRando()
        {
            var trackerFile = File.ReadAllText(Environment.ExpandEnvironmentVariables(itemTrackerFile));
            var currentTracker = JsonSerializer.Deserialize<ItemTracker>(trackerFile.ToString());

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
            RelicUpdate(currentTracker, ATT, ATTCount, "Level Up - Attack", "Relic - Hero Sword", "att");
            RelicUpdate(currentTracker, DEF, DEFCount, "Level Up - DamageResist", "Relic - Hero Crown", "def");
            RelicUpdate(currentTracker, POTION, POTIONCount, "Level Up - PotionEfficiency", "Relic - Hero Water", "potion");
            RelicUpdate(currentTracker, HP, HPCount, "Level Up - Health", "Relic - Hero Pendant HP", "hp");
            RelicUpdate(currentTracker, SP, SPCount, "Level Up - Stamina", "Relic - Hero Pendant SP", "sp");
            RelicUpdate(currentTracker, MP, MPCount, "Level Up - Magic", "Relic - Hero Pendant MP", "mp");

            // sword progression
            var swordProg = currentTracker.ImportantItems.SingleOrDefault(x => x.Key == "Sword Progression");
            uiThread.TryEnqueue(() =>
            {
                switch (swordProg.Value)
                {
                    case 1:
                    case 2:
                        Sword.Source = new BitmapImage(new Uri($"ms-appx:///assets/items/sword.png"));
                        break;
                    case 3:
                        Sword.Source = new BitmapImage(new Uri($"ms-appx:///assets/items/sword3.png"));
                        break;
                    case 4:
                        Sword.Source = new BitmapImage(new Uri($"ms-appx:///assets/items/sword4.png"));
                        break;
                }
            });

            // hexagon quest
            var hexQuest = currentTracker.ImportantItems.SingleOrDefault(x => x.Key == "Hexagon Gold");
            uiThread.TryEnqueue(() =>
            {
                if(hexQuest.Value > 0)
                {
                    HexBlue.Visibility = Visibility.Collapsed;
                    HexGreen.Visibility = Visibility.Collapsed;
                    HexRed.Visibility = Visibility.Collapsed;

                    HexagonQuest.Visibility = Visibility.Visible;
                    HexGoldCount.Text = hexQuest.Value.ToString();
                }
                else
                {
                    HexBlue.Visibility = Visibility.Visible;
                    HexGreen.Visibility = Visibility.Visible;
                    HexRed.Visibility = Visibility.Visible;

                    HexagonQuest.Visibility = Visibility.Collapsed;
                    HexGoldCount.Text = "0";
                }
            });
        }

        private static void ItemCollected(ItemTracker current, Image item, string randoName)
        {
            uiThread.TryEnqueue(() =>
            {
                var foundItem = current.ImportantItems.SingleOrDefault(x => x.Key == randoName);
                item.Opacity = foundItem.Value > 0 ? 1 : 0.3;
            });
        }

        private static void QuantityCollected(ItemTracker current, Image item, TextBlock itemCount, string randoName)
        {
            uiThread.TryEnqueue(() =>
            {
                var foundItem = current.ImportantItems.SingleOrDefault(x => x.Key == randoName);
                item.Opacity = foundItem.Value > 0 ? 1 : 0.3;
                itemCount.Text = foundItem.Value.ToString();
            });
        }

        private static void RelicUpdate(ItemTracker current, Image item, TextBlock itemCount, string statName, string relicName, string code)
        {
            uiThread.TryEnqueue(() =>
            {
                // updates based on collected upgrades
                var foundStat = current.ImportantItems.SingleOrDefault(x => x.Key == statName);
                itemCount.Text = foundStat.Value.ToString();

                // updates based on collected upgrades
                var foundRelic = current.ImportantItems.SingleOrDefault(x => x.Key == relicName);
                item.Source = foundRelic.Value > 0
                    ? new BitmapImage(new Uri($"ms-appx:///assets/items/relic-{code}2.png"))
                    : new BitmapImage(new Uri($"ms-appx:///assets/items/relic-{code}.png"));

                // light up graphic if either an upgrade was found or the relic was obtained
                item.Opacity = (foundStat.Value > 0 || foundRelic.Value > 0) ? 1 : 0.3;
            });
        }
    }
}
