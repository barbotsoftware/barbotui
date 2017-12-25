using BarBot.Core;
using BarBot.Core.Model;
using BarBot.Core.WebSocket;
using BarBot.UWP.IO;
using BarBot.UWP.UserControls.RecipeDetail.Dialogs;
using BarBot.UWP.Utils;
using BarBot.UWP.Websocket;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace BarBot.UWP.UserControls.RecipeDetail
{
    public sealed partial class Uc_RecipeDetail : UserControl, INotifyPropertyChanged
    {
        private UWPWebSocketService webSocketService;
        private BarbotIOController barbotIOController;

        private Recipe recipe;
        private double totalVolume;
        private double volumeAvailable;

        private Recipe OrderRecipe;
        private List<Tc_IngredientRow> ingredientRows;
        private Dictionary<string, Ingredient> AvailableIngredientList;

        private Uc_AddIngredientButton addIngredientBtn;
        private Uc_IngredientPicker ingredientPicker;
        private string maxVolumeLabel;

        private BitmapImage _cachedImage;

        public Recipe Recipe
        {
            get
            {
                return recipe;
            }

            set
            {
                recipe = value;
                TotalVolume = recipe.GetVolume();
                VolumeAvailable = Constants.MaxVolume - TotalVolume;

                AppBar.Title = Helpers.UppercaseWords(Recipe.Name);

                if (!Recipe.Name.Equals(Constants.CustomRecipeName))
                {
                    // Attach event handler and then call GetRecipeDetails
                    webSocketService.AddDetailEventHandlers(Socket_GetRecipeDetailEvent, null, null);
                    webSocketService.GetRecipeDetails(Recipe.RecipeId);
                }
                else
                {
                    DisplayIngredients();
                }

                CachedImage = (Application.Current as App).getCachedImage(value);

                OnPropertyChanged("Recipe");
            }
        }

        public BitmapImage CachedImage
        {
            get { return _cachedImage; }
            set
            {
                _cachedImage = value;
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

        public Uc_RecipeDetail(Recipe recipe)
        {
            this.InitializeComponent();
            this.DataContext = this;

            this.webSocketService = (Application.Current as App).webSocketService;
            this.barbotIOController = (Application.Current as App).barbotIOController;
            this.Recipe = recipe;

            this.AvailableIngredientList = (Application.Current as App).IngredientsInBarbot;

            this.MaxVolumeLabel = string.Format("/{0} oz", Constants.MaxVolume);
        }

        private void Back_To_Menu(object sender, RoutedEventArgs e)
        {
            ((Window.Current.Content as Frame).Content as MainPage).ContentFrame.GoBack(new SlideNavigationTransitionInfo());
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
                    i.Name = AvailableIngredientList[i.IngredientId].Name;
                }
                DisplayIngredients();
            });

            webSocketService.Socket.GetRecipeDetailsEvent -= Socket_GetRecipeDetailEvent;
        }

        private void UpdateVolumes()
        {
            TotalVolume = Recipe.GetVolume();
            VolumeAvailable = Constants.MaxVolume - TotalVolume;
        }

        private void DisplayIngredients()
        {
            ingredientList.Children.Clear();
            ingredientRows = new List<Tc_IngredientRow>();

            UpdateVolumes();

            for (int i = 0; i < Recipe.Ingredients.Count; i++)
            {
                if (Recipe.Ingredients[i] != null)
                {
                    var ingredientRow = new Tc_IngredientRow(Recipe.Ingredients[i],
                                                             IngredientRow_DecrementVolume,
                                                             IngredientRow_IncrementVolume,
                                                             IngredientRow_RemoveIngredient);
                    ingredientRows.Add(ingredientRow);
                    ingredientList.Children.Add(ingredientRow);
                }
            }

            if (AvailableIngredientList != null)
            {
                if (Recipe.Ingredients.Count < AvailableIngredientList.Count)
                {
                    addIngredientBtn = new Uc_AddIngredientButton();
                    addIngredientBtn.PointerReleased += AddIngredientBtn_PointerReleased;
                    ingredientList.Children.Add(addIngredientBtn);
                }
            }
            else
            {
                addIngredientBtn = new Uc_AddIngredientButton();
                addIngredientBtn.PointerReleased += AddIngredientBtn_PointerReleased;
                ingredientList.Children.Add(addIngredientBtn);
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

        private void AddIngredientBtn_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            // Blast last element of ingredientList if it's the addIngredientBtn
            if (ingredientList.Children[ingredientList.Children.Count - 1] == addIngredientBtn)
            {
                ingredientList.Children.RemoveAt(ingredientList.Children.Count - 1);
            }

            // Add the new ingredient control

            List<Ingredient> nonRepeatedIngredients = new List<Ingredient>();
            foreach (Ingredient i in AvailableIngredientList.Values)
            {
                bool ingredientFound = false;
                for (var j = 0; j < Recipe.Ingredients.Count; j++)
                {
                    if (Recipe.Ingredients[j].Name == i.Name)
                    {
                        ingredientFound = true;
                        break;
                    }
                }
                if (!ingredientFound)
                {
                    nonRepeatedIngredients.Add(i);
                }
            }
            ingredientPicker = new Uc_IngredientPicker(nonRepeatedIngredients);
            ingredientList.Children.Add(ingredientPicker);

            ingredientPicker.addIngredientButton.Click += AddIngredientButton_Click;
            ingredientPicker.cancelAddIngredientButton.Click += CancelAddIngredientButton_Click;
        }

        private void AddIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(ingredientPicker.selectedIngredient);

            TotalVolume = 0;
            // Calculate total volume to ensure added ingredient is within limits
            for (int i = 0; i < Recipe.Ingredients.Count; i++)
            {
                TotalVolume += Recipe.Ingredients[i].Amount;
            }

            if (TotalVolume + ingredientPicker.selectedIngredient.Amount > Constants.MaxVolume)
            {
                // set ingredient volume to be max within limits
                ingredientPicker.selectedIngredient.Amount = Constants.MaxVolume - TotalVolume;
            }

            Recipe.Ingredients.Add(ingredientPicker.selectedIngredient);
            DisplayIngredients();
        }

        private void CancelAddIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayIngredients();
        }

        private async void Pour_Drink(object sender, RoutedEventArgs e)
        {
            if (barbotIOController != null)
            {
                if (barbotIOController.CupCount == 0)
                {
                    var cupDialog = new CupContentDialog();
                    await cupDialog.ShowAsync();
                }

                // Can snag ingredients from _ingredientElementList[x]._ingredient
                System.Diagnostics.Debug.WriteLine(ingredientRows);

                OrderRecipe = new Recipe();
                OrderRecipe.Name = Recipe.Name;
                OrderRecipe.Ingredients = new List<Ingredient>();

                for (int i = 0; i < ingredientRows.Count; i++)
                {
                    OrderRecipe.Ingredients.Add(ingredientRows[i].Ingredient);
                }

                // show pouring dialog
                var dialog = new PouringContentDialog(OrderRecipe, AddIce.IsChecked.Value, AddGarnish.IsChecked.Value);
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
