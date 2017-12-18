using BarBot.Core.Model;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BarBot.UWP.UserControls.RecipeDetail
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
