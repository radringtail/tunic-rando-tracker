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
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;

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

        public static string itemTheme = "items";

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

        private void ThemeClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            switch (button.Name)
            {
                case "ThemeDefault":
                    itemTheme = "items";
                    ParseRando();
                    break;
                case "ThemeCircles":
                    itemTheme = "items-circle";
                    ParseRando();
                    break;
            }
        }

        private void ColorClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            switch (button.Name)
            {
                case "ColorDefault":
                    ItemGrid.Background = new SolidColorBrush(Colors.Black);
                    break;
                case "ColorGreen":
                    ItemGrid.Background = new SolidColorBrush(Colors.Lime);
                    break;
            }
        }

        public void ParseRando()
        {
            var trackerFile = File.ReadAllText(Environment.ExpandEnvironmentVariables(itemTrackerFile));
            var currentTracker = JsonSerializer.Deserialize<ItemTracker>(trackerFile.ToString());

            // check togglable items 
            ItemCollected(currentTracker, Stick, "Stick", "stick");
            ItemCollected(currentTracker, Sword, "Sword", "sword");
            ItemCollected(currentTracker, Dagger, "Stundagger", "dagger");
            ItemCollected(currentTracker, Wand, "Techbow", "wand");
            ItemCollected(currentTracker, Orb, "Wand", "orb");
            ItemCollected(currentTracker, Laurels, "Hyperdash", "laurels");
            ItemCollected(currentTracker, Lantern, "Lantern", "lantern");
            ItemCollected(currentTracker, Shield, "Shield", "shield");
            ItemCollected(currentTracker, Hourglass, "SlowmoItem", "hourglass");
            ItemCollected(currentTracker, Gun, "Shotgun", "shotgun");
            ItemCollected(currentTracker, Dath, "Dath Stone", "dath");
            ItemCollected(currentTracker, HouseKey, "Key (House)", "key");
            ItemCollected(currentTracker, VaultKey, "Vault Key (Red)", "vaultkey");
            ItemCollected(currentTracker, HexRed, "Hexagon Red", "hex-red");
            ItemCollected(currentTracker, HexBlue, "Hexagon Blue", "hex-blue");
            ItemCollected(currentTracker, HexGreen, "Hexagon Green", "hex-green");

            // count collected items
            QuantityCollected(currentTracker, Coins, CoinsCount, "Trinket Coin", "coin");
            QuantityCollected(currentTracker, Slots, SlotsCount, "Trinket Slot", "slot");
            QuantityCollected(currentTracker, Cards, CardsCount, "Trinket Cards", "card");
            QuantityCollected(currentTracker, Pages, PagesCount, "Pages", "book");
            QuantityCollected(currentTracker, Fairies, FairiesCount, "Fairies", "fairy");
            QuantityCollected(currentTracker, Trophies, TrophiesCount, "Golden Trophies", "trophy");

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
                        Sword.Source = new BitmapImage(new Uri($"ms-appx:///assets/{itemTheme}/sword.png"));
                        break;
                    case 3:
                        Sword.Source = new BitmapImage(new Uri($"ms-appx:///assets/{itemTheme}/sword3.png"));
                        break;
                    case 4:
                        Sword.Source = new BitmapImage(new Uri($"ms-appx:///assets/{itemTheme}/sword4.png"));
                        break;
                }
            });

            // hexagon quest
            var hexQuest = currentTracker.ImportantItems.SingleOrDefault(x => x.Key == "Hexagon Gold");
            uiThread.TryEnqueue(() =>
            {
                if (hexQuest.Value > 0)
                {
                    HexBlue.Visibility = Visibility.Collapsed;
                    HexGreen.Visibility = Visibility.Collapsed;
                    HexRed.Visibility = Visibility.Collapsed;

                    HexagonQuest.Visibility = Visibility.Visible;
                    HexGoldCount.Text = hexQuest.Value.ToString();
                }
                else
                {
                    var redHex = currentTracker.ImportantItems.SingleOrDefault(x => x.Key == "Hexagon Red");
                    var greenHex = currentTracker.ImportantItems.SingleOrDefault(x => x.Key == "Hexagon Green");
                    var blueHex = currentTracker.ImportantItems.SingleOrDefault(x => x.Key == "Hexagon Blue");

                    if (redHex.Value > 0 || greenHex.Value > 0 || blueHex.Value > 0)
                    {
                        HexBG.Source = new BitmapImage(new Uri($"ms-appx:///assets/{itemTheme}/hex-bg.png"));
                    }
                    else
                    {
                        HexBG.Source = new BitmapImage(new Uri($"ms-appx:///assets/{itemTheme}/dim/hex-bg.png"));
                    }

                    HexBlue.Visibility = Visibility.Visible;
                    HexGreen.Visibility = Visibility.Visible;
                    HexRed.Visibility = Visibility.Visible;

                    HexagonQuest.Visibility = Visibility.Collapsed;
                    HexGoldCount.Text = "0";
                }
            });
        }

        private static void ItemCollected(ItemTracker current, Image item, string randoName, string code)
        {
            uiThread.TryEnqueue(() =>
            {
                var foundItem = current.ImportantItems.SingleOrDefault(x => x.Key == randoName);
                item.Source = foundItem.Value > 0
                  ? new BitmapImage(new Uri($"ms-appx:///assets/{itemTheme}/{code}.png"))
                  : new BitmapImage(new Uri($"ms-appx:///assets/{itemTheme}/dim/{code}.png"));
            });
        }

        private static void QuantityCollected(ItemTracker current, Image item, TextBlock itemCount, string randoName, string code)
        {
            uiThread.TryEnqueue(() =>
            {
                var foundItem = current.ImportantItems.SingleOrDefault(x => x.Key == randoName);
                item.Source = foundItem.Value > 0
                  ? new BitmapImage(new Uri($"ms-appx:///assets/{itemTheme}/{code}.png"))
                  : new BitmapImage(new Uri($"ms-appx:///assets/{itemTheme}/dim/{code}.png"));
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

                // light up graphic if an upgrade or relic was found
                if (foundRelic.Value > 0)
                {
                    item.Source = new BitmapImage(new Uri($"ms-appx:///assets/{itemTheme}/relic-{code}2.png"));
                }
                else
                {
                    item.Source = foundStat.Value > 0
                        ? new BitmapImage(new Uri($"ms-appx:///assets/{itemTheme}/relic-{code}.png"))
                        : new BitmapImage(new Uri($"ms-appx:///assets/{itemTheme}/dim/relic-{code}.png"));
                }
            });
        }
    }
}
