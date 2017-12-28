using BarBot.Core;
using BarBot.Core.Model;
using BarBot.Core.WebSocket;
using BarBot.UWP.UserControls.RecipeDetail.Dialogs;
using BarBot.UWP.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace BarBot.UWP.UserControls.RecipeDetail
{
    public sealed partial class Uc_RecipeDetail : UserControl, INotifyPropertyChanged
    {
        private App app;
        private List<Ingredient> AvailableIngredients;

        private Recipe recipe;
        private double totalVolume;
        private double volumeAvailable;
        private string maxVolumeLabel;
        private string pourButtonDisplayText;
        private bool ice;

        private Uc_AddIngredientButton AddIngredientButton;

        private BitmapImage cachedImage;

        public Recipe Recipe
        {
            get
            {
                return recipe;
            }

            set
            {
                recipe = value;
                recipe.Name = Helpers.UppercaseWords(recipe.Name);
                TotalVolume = recipe.GetVolume();
                VolumeAvailable = Constants.MaxVolume - TotalVolume;

                AppBar.Title = Recipe.Name;

                if (!Recipe.Name.Equals(Constants.CustomRecipeName))
                {
                    // Attach event handler and then call GetRecipeDetails
                    app.webSocketService.AddDetailEventHandlers(Socket_GetRecipeDetailEvent, null, null);
                    app.webSocketService.GetRecipeDetails(Recipe.RecipeId);
                }
                else
                {
                    DisplayIngredients();
                }

                CachedImage = this.app.getCachedImage(value);

                OnPropertyChanged("Recipe");
            }
        }

        public Boolean Ice
        {
            get { return ice; }
            set
            {
                ice = value;
                OnPropertyChanged("Ice");
            }
        }

        public BitmapImage CachedImage
        {
            get { return cachedImage; }
            set
            {
                cachedImage = value;
                OnPropertyChanged("CachedImage");
            }
        }

        public double TotalVolume
        {
            get
            {
                return totalVolume;
            }

            set
            {
                totalVolume = value;
                OnPropertyChanged("TotalVolume");
            }
        }

        public double VolumeAvailable
        {
            get
            {
                return volumeAvailable;
            }

            set
            {
                volumeAvailable = value;
                OnPropertyChanged("VolumeAvailable");
            }
        }

        public string MaxVolumeLabel
        {
            get { return maxVolumeLabel; }
            set
            {
                maxVolumeLabel = value;
                OnPropertyChanged("MaxVolumeLabel");
            }
        }

        public string PourButtonDisplayText
        {
            get { return pourButtonDisplayText; }
            set
            {
                pourButtonDisplayText = value;
                OnPropertyChanged("PourButtonDisplayText");
            }
        }

        public Uc_RecipeDetail(Recipe recipe)
        {
            this.InitializeComponent();
            this.DataContext = this;

            this.app = (Application.Current as App);
            this.Recipe = recipe;
            this.PourButtonDisplayText = string.Format("Pour {0}", Recipe.Name);
            this.MaxVolumeLabel = string.Format("/{0} oz", Constants.MaxVolume);
            this.ice = false;
        }

        private async void Socket_GetRecipeDetailEvent(object sender, WebSocketEvents.GetRecipeDetailsEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High,
            () =>
            {
                // Get Ingredient Names from Global Ingredient List
                Recipe.Ingredients = args.Recipe.Ingredients;
                foreach (Ingredient i in Recipe.Ingredients)
                {
                    i.Name = app.IngredientsInBarbot[i.IngredientId].Name;
                }
                DisplayIngredients();
            });

            app.webSocketService.Socket.GetRecipeDetailsEvent -= Socket_GetRecipeDetailEvent;
        }

        private void UpdateVolumes()
        {
            TotalVolume = Recipe.GetVolume();
            VolumeAvailable = Constants.MaxVolume - TotalVolume;
        }

        // Clears AvailableIngredients, finds all IngredientIds for
        // the Recipe, then adds all Ingredients that are NOT in the
        // Recipe to AvailableIngredients from all the Ingredients
        // in the BarBot. Then sort AvailableIngredients by Name.
        private void UpdateAvailableIngredients()
        {
            if (AvailableIngredients != null)
            {
                AvailableIngredients.Clear();
            }
            else
            {
                AvailableIngredients = new List<Ingredient>();
            }
            var recipeIngredientIds = Recipe.Ingredients.Select(i => i.IngredientId).ToList();

            foreach (var ingredient in app.IngredientsInBarbot)
            {
                if (!recipeIngredientIds.Contains(ingredient.Key))
                {
                    // Take copy to avoid modifying app's IngredientsInBarBot list
                    Ingredient ingredientCopy = new Ingredient(ingredient.Key,
                                                               ingredient.Value.Name,
                                                               ingredient.Value.Amount);
                    AvailableIngredients.Add(ingredientCopy);
                }
            }

            AvailableIngredients.Sort((x, y) => x.Name.CompareTo(y.Name));
        }

        // Update Ingredient Row Name Combo Boxes
        // for all Rows
        private void UpdateIngredientComboBoxes()
        {
            foreach (var ingredientRow in IngredientRowStackPanel.Children)
            {
                if (ingredientRow != AddIngredientButton)
                {
                    (ingredientRow as Tc_IngredientRow).AvailableIngredients = AvailableIngredients.ToList();
                    (ingredientRow as Tc_IngredientRow).PopulateComboBox();
                }
            }
        }

        // Display all Ingredient Rows
        private void DisplayIngredients()
        {
            IngredientRowStackPanel.Children.Clear();

            UpdateVolumes();
            UpdateAvailableIngredients();

            for (int i = 0; i < Recipe.Ingredients.Count; i++)
            {
                if (Recipe.Ingredients[i] != null)
                {
                    var ingredientRow = new Tc_IngredientRow(Recipe.Ingredients[i],
                                                             AvailableIngredients.ToList(),
                                                             IngredientRow_IngredientChanged,
                                                             IngredientRow_DecrementVolume,
                                                             IngredientRow_IncrementVolume,
                                                             IngredientRow_RemoveIngredient);
                    IngredientRowStackPanel.Children.Add(ingredientRow);
                }
            }

            DisplayAddIngredientButton();
        }

        // Ingredient Row Button Event Handlers

        private void IngredientRow_IngredientChanged(object sender, SelectionChangedEventArgs e)
        {
            var senderComboBox = sender as ComboBox;
            
            if (senderComboBox.SelectedIndex > 0)
            {
                var ingredientRow = (Tc_IngredientRow)senderComboBox.DataContext;
                var newIngredient = senderComboBox.Items[senderComboBox.SelectedIndex] as Ingredient;

                if (newIngredient != null)
                {
                    // update Ingredient Row
                    ingredientRow.Ingredient.IngredientId = newIngredient.IngredientId;
                    ingredientRow.Ingredient.Name = newIngredient.Name;

                    // Update Available Ingredients
                    UpdateAvailableIngredients();
                    UpdateIngredientComboBoxes();
                }
            }            
        }

        private void IngredientRow_DecrementVolume(object sender, RoutedEventArgs e)
        {
            var senderButton = sender as Button;
            var ingredientRow = (Tc_IngredientRow)senderButton.DataContext;
            var ingredient = ingredientRow.Ingredient;

            if (ingredient.Amount > 0.5)
            {
                var newIngredient = new Ingredient(ingredient.IngredientId, ingredient.Name, ingredient.Amount - 0.5);
                ingredientRow.Ingredient = newIngredient;

                Recipe.Ingredients.RemoveAll(i => i.IngredientId == newIngredient.IngredientId);
                Recipe.Ingredients.Add(newIngredient);

                UpdateVolumes();

                if (VolumeAvailable >= 0.5)
                {
                    DisplayAddIngredientButton();
                }
            }
        }

        private void IngredientRow_IncrementVolume(object sender, RoutedEventArgs e)
        {
            var senderButton = sender as Button;
            var ingredientRow = (Tc_IngredientRow)senderButton.DataContext;
            var ingredient = ingredientRow.Ingredient;

            if (VolumeAvailable >= 0.5)
            {
                var newIngredient = new Ingredient(ingredient.IngredientId, ingredient.Name, ingredient.Amount + 0.5);
                ingredientRow.Ingredient = newIngredient;

                Recipe.Ingredients.RemoveAll(i => i.IngredientId == newIngredient.IngredientId);
                Recipe.Ingredients.Add(newIngredient);

                UpdateVolumes();

                if (VolumeAvailable < 0.5)
                {
                    HideAddIngredientButton();
                }
            }
        }

        private void IngredientRow_RemoveIngredient(object sender, RoutedEventArgs e)
        {
            var senderButton = sender as Button;
            var ingredientRow = (Tc_IngredientRow)senderButton.DataContext;

            // keep ingredient element list up to date
            Recipe.Ingredients.RemoveAll(i => i.IngredientId == ingredientRow.Ingredient.IngredientId);

            DisplayIngredients();
        }

        // Add Ingredient Button

        private void DisplayAddIngredientButton()
        {
            if (!IngredientRowStackPanel.Children.Contains(AddIngredientButton)
                && AvailableIngredients.Count > 0)
            {
                AddIngredientButton = new Uc_AddIngredientButton();
                AddIngredientButton.PointerReleased += AddIngredientBtn_PointerReleased;
                IngredientRowStackPanel.Children.Add(AddIngredientButton);
            }
        }

        private void HideAddIngredientButton()
        {
            if (IngredientRowStackPanel.Children.Contains(AddIngredientButton))
            {
                IngredientRowStackPanel.Children.Remove(AddIngredientButton);
                AddIngredientButton = null;
            }
        }

        private void AddIngredientBtn_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var newIngredient = AvailableIngredients.First();
            newIngredient.Amount = 0.5;
            Recipe.Ingredients.Add(newIngredient);

            UpdateVolumes();
            UpdateAvailableIngredients();
            UpdateIngredientComboBoxes();
            HideAddIngredientButton();

            var newIngredientRow = new Tc_IngredientRow(newIngredient,
                                                        AvailableIngredients,
                                                        IngredientRow_IngredientChanged,
                                                        IngredientRow_DecrementVolume,
                                                        IngredientRow_IncrementVolume,
                                                        IngredientRow_RemoveIngredient);
            IngredientRowStackPanel.Children.Add(newIngredientRow);

            if (AvailableIngredients.Count > 0 && VolumeAvailable >= 0.5)
            {
                DisplayAddIngredientButton();
            }
        }

        // Pour Button

        private async void Pour_Drink(object sender, RoutedEventArgs e)
        {
            if (app.barbotIOController != null)
            {
                if (app.barbotIOController.CupCount == 0)
                {
                    // show cup dialog
                    var cupDialog = new CupContentDialog();
                    await cupDialog.ShowAsync();

                    if (!cupDialog.ShouldProceed)
                    {
                        return;
                    }
                }

                // show ice dialog
                var iceDialog = new IceContentDialog(this);
                await iceDialog.ShowAsync();

                if (!iceDialog.ShouldProceed)
                {
                    return;
                }

                // show garnish dialog
                var garnishDialog = new GarnishContentDialog();
                await garnishDialog.ShowAsync();

                if (!garnishDialog.ShouldProceed)
                {
                    return;
                }
                
                // show pouring dialog
                var dialog = new PouringContentDialog(Recipe, Ice, /*AddGarnish.IsChecked.Value*/ 0); // TODO: update this to use int value for garnish type
                await dialog.ShowAsync();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("BarbotIOController is null");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
