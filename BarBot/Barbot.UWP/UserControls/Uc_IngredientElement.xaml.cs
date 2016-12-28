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
        double maxVolume = 12.0;
        public Ingredient _ingredient;
        public Uc_IngredientElement(Ingredient ingredient)
        {
            this.InitializeComponent();

            this.DataContext = this;
            Ingredient = ingredient;
            init();
        }

        public void init()
        {
            // Populate combobox w/ shit
            for (var i = 0.5; i <= maxVolume; i += 0.5)
            {
                ingredientVolume.Items.Add(i);
            }

            if (ingredientVolume.Items.IndexOf(Math.Truncate(Ingredient.Quantity)) > -1)
            {
                ingredientVolume.SelectedIndex = ingredientVolume.Items.IndexOf(Math.Truncate(Ingredient.Quantity));
            } else if(ingredientVolume.Items.IndexOf(Ingredient.Quantity) > -1)
            {
                ingredientVolume.SelectedIndex = ingredientVolume.Items.IndexOf(Ingredient.Quantity);
            } else
            {
                ingredientVolume.SelectedIndex = 0;
            }
            ingredientName.Text = Ingredient.Name;
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
            Ingredient.Quantity = (double)ingredientVolume.Items[ingredientVolume.SelectedIndex];
        }
    }
}
