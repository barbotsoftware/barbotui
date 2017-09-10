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
using BarBot.Core.WebSocket;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BarBot.UWP.UserControls
{
    public sealed partial class Uc_IngredientPicker : UserControl
    {
        double maxVolume = 12.0;
        List<Ingredient> availableIngredientList;
        public Ingredient selectedIngredient = new Ingredient();

        public Uc_IngredientPicker(List<Ingredient> AvailableIngredientList)
        {
            this.InitializeComponent();
            availableIngredientList = AvailableIngredientList;
            init();
        }
        
        public void init()
        {
            // Populate combobox w/ shit
            for (var i = 0.5; i <= maxVolume; i += 0.5)
            {
                volumeSelector.Items.Add(i);
            }
            volumeSelector.SelectedIndex = 1;

            // Get available ingredients
            for(var i = 0; i < availableIngredientList.Count; i++)
            {
                ingredientSelector.Items.Add(availableIngredientList[i].Name.ToUpper());
            }
            ingredientSelector.SelectedIndex = 0;

            selectedIngredient = availableIngredientList[0];
            //selectedIngredient.Quantity = 1;
        }

        private void Volume_Changed(object sender, SelectionChangedEventArgs e)
        {
            //selectedIngredient.Quantity = (double)volumeSelector.Items[volumeSelector.SelectedIndex];
        }

        private void Ingredient_Changed(object sender, SelectionChangedEventArgs e)
        {
            selectedIngredient = availableIngredientList[ingredientSelector.SelectedIndex];
            //selectedIngredient.Quantity = (double)volumeSelector.Items[volumeSelector.SelectedIndex];
        }
    }
}
