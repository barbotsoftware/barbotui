using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using BarBot.Core;
using BarBot.Core.Model;
using System.ComponentModel;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BarBot.UWP.UserControls
{
    public sealed partial class Uc_IngredientElement : UserControl, INotifyPropertyChanged
    {
        private double _volumeAvailable;
        public bool VolumeChangeInProgress = false;
        public Ingredient _ingredient;
        public Uc_IngredientElement(Ingredient ingredient, double volumeAvailable)
        {
            this.InitializeComponent();

            this.DataContext = this;
            Ingredient = ingredient;
            VolumeAvailable = volumeAvailable;
            init();
        }

        public void init()
        {
            // Populate combobox w/ shit
            PopulateVolumeSelector();
            ingredientName.Text = Ingredient.Name.ToUpper();
        }

        private void PopulateVolumeSelector()
        {
            /*VolumeChangeInProgress = true;
            ingredientVolume.Items.Clear();


            if (Ingredient.Quantity >= 0.5)
            {
                for (var i = 0.5; i <= VolumeAvailable + Ingredient.Quantity; i += 0.5)
                {
                    ingredientVolume.Items.Add(i);
                }
            }
            else
            {
                ingredientVolume.Items.Add((double)0);
                ingredientVolume.SelectedIndex = 0;

                for (double i = 0; i <= VolumeAvailable + Ingredient.Quantity; i += 0.5)
                {
                    ingredientVolume.Items.Add(i);
                }
            }

            if (ingredientVolume.Items.IndexOf(Ingredient.Quantity) > -1)
            {
                ingredientVolume.SelectedIndex = ingredientVolume.Items.IndexOf(Ingredient.Quantity);
            }
            else
            {
                if(ingredientVolume.Items.Count > 0)
                {
                    ingredientVolume.SelectedIndex = 0;
                }
            }

            VolumeChangeInProgress = false;*/
        }

        public double VolumeAvailable
        {
            get
            {
                return _volumeAvailable;
            }
            set
            {
                _volumeAvailable = value;
                OnPropertyChanged("VolumeAvailable");
                PopulateVolumeSelector();
            }
        }

        public Ingredient Ingredient
        {
            get
            {
                return _ingredient;
            }

            set
            {
                _ingredient = value;
                OnPropertyChanged("Ingredient");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void Volume_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (ingredientVolume.SelectedIndex >= 0)
            {
                //Ingredient.Quantity = (double)ingredientVolume.Items[ingredientVolume.SelectedIndex];
            }
        }
    }
}
