﻿using MCDSaveEdit.Save.Models.Enums;
using MCDSaveEdit.Save.Models.Profiles;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
#nullable enable

namespace MCDSaveEdit
{
    /// <summary>
    /// Interaction logic for EnchantmentScreen.xaml
    /// </summary>
    public partial class EnchantmentScreen : UserControl
    {
        public EnchantmentScreen()
        {
            InitializeComponent();
            if (ImageUriHelper.gameContentLoaded)
            {
                useGameContentImages();
            }

            tierLabel.Content = R.TIER;

            updateUI();
        }

        private void useGameContentImages()
        {
            enchantmentBackgroundImage.Source = fullImageForEnchantmentLevel(0);
            enchantmentPointsSymbolImage.Source = ImageUriHelper.instance.imageSource("/Dungeons/Content/UI/Materials/Inventory2/Enchantment/enchant_counter");
        }

        private Enchantment? _enchantment;
        public Enchantment? enchantment
        {
            get { return _enchantment; }
            set { _enchantment = value; updateUI(); }
        }

        private void updateUI()
        {
            if(_enchantment == null)
            {
                powerfulImage.Source = null;
                powerfulLabel.Content = string.Empty;
                enchantmentImage.Source = null;
                enchantmentLabel.Content = string.Empty;
                updateTierUI();
                return;
            }

            if (_enchantment.isPowerful())
            {
                powerfulImage.Source = ImageUriHelper.instance.imageSource("/Dungeons/Content/UI/Materials/Inventory2/Enchantment/Inspector/element_powerful");
                powerfulLabel.Content = R.POWERFUL;
            }
            else
            {
                powerfulImage.Source = null;
                powerfulLabel.Content = R.COMMON;
            }

            enchantmentImage.Source = ImageUriHelper.instance.imageSourceForEnchantment(_enchantment);
            enchantmentLabel.Content = _enchantment.Id;

            updateTierUI();
        }

        private void updateTierUI()
        {
            if(_enchantment != null)
            {
                if (ImageUriHelper.gameContentLoaded)
                {
                    enchantmentBackgroundImage.Source = imageForEnchantmentLevel(_enchantment!.Level);
                }
                tierTextBox.Text = _enchantment!.Level.ToString();
                pointsCostLabel.Content = _enchantment!.pointsCost().ToString();
            }
            else
            {
                enchantmentBackgroundImage.Source = null;
                tierTextBox.Text = string.Empty;
                pointsCostLabel.Content = string.Empty;
            }
        }

        private BitmapSource? imageForEnchantmentLevel(long level)
        {
            var enchantmentLevelImage = fullImageForEnchantmentLevel(level);
            if (enchantmentLevelImage == null) return null;
            var croppedImage = new CroppedBitmap(enchantmentLevelImage!, new Int32Rect(0, 0, 976, 959));
            return croppedImage;
        }

        private BitmapImage? fullImageForEnchantmentLevel(long level)
        {
            if (level < 0 || level > Constants.MAXIMUM_ENCHANTMENT_TIER)
            {
                return null;
            }
            var imageName = $"/Dungeons/Content/UI/Materials/Inventory2/Enchantment/Inspector2/lv{level}_frame";
            return ImageUriHelper.instance.imageSource(imageName);
        }

        public ICommand? saveChanges { get; set; }
        public ICommand? close { get; set; }

        private void tierTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_enchantment == null) { return; }
            if (int.TryParse(tierTextBox.Text, out int level))
            {
                _enchantment.Level = level;
                this.saveChanges?.Execute(_enchantment);
                updateTierUI();
            }
        }

        private void upButton_Click(object sender, RoutedEventArgs e)
        {
            if (_enchantment == null) { return; }
            if (int.TryParse(tierTextBox.Text, out int level) && level < Constants.MAXIMUM_ENCHANTMENT_TIER)
            {
                int newLevel = level + 1;
                tierTextBox.Text = newLevel.ToString();
            }
        }

        private void downButton_Click(object sender, RoutedEventArgs e)
        {
            if (_enchantment == null) { return; }
            if (int.TryParse(tierTextBox.Text, out int level) && level > Constants.MINIMUM_ENCHANTMENT_TIER)
            {
                int newLevel = level - 1;
                tierTextBox.Text = newLevel.ToString();
            }
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            close?.Execute(null);
        }
    }
}
