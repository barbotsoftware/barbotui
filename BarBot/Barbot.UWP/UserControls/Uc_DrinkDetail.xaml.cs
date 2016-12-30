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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using BarBot.Core;
using BarBot.Core.Model;
using BarBot.Core.WebSocket;
using System.ComponentModel;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BarBot.UWP.UserControls
{
    public sealed partial class Uc_DrinkDetail : UserControl, INotifyPropertyChanged
    {
        private Recipe _recipe;
        private WebSocketUtil socketUtil;
        private List<Uc_IngredientElement> _ingredientElementList;
        private List<Ingredient> AvailableIngredientList;
        Uc_AddIngredientButton addIngredientBtn;
        Uc_IngredientPicker ingredientPicker;

        public Uc_DrinkDetail(Recipe SelectedRecipe)
        {
            App app = Application.Current as App;
            socketUtil = app.webSocketUtil;

            this.InitializeComponent();
            this.DataContext = this;
            Recipe = SelectedRecipe;

            init();
        }

        public void init()
        {
            if (Recipe.Name != "Custom Recipe")
            {

                // Attach event handler and then call GetRecipeDetails
                socketUtil.AddDetailEventHandlers(Socket_GetRecipeDetailEvent, null, null);
                socketUtil.GetRecipeDetails(Recipe.RecipeId);
            }
            else
            {
                DisplayIngredients();
            }

            socketUtil.AddMenuEventHandlers(null, Socket_GetIngredientsEvent);
            socketUtil.GetIngredients();
        }

        public Recipe Recipe
        {
            get
            {
                return _recipe;
            }

            set
            {
                _recipe = value;
                OnPropertyChanged("Recipe");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void Back_To_Menu(object sender, RoutedEventArgs e)
        {
            ((Window.Current.Content as Frame).Content as MainPage).ContentFrame.Content = new Uc_Menu();
        }

        private async void Socket_GetRecipeDetailEvent(object sender, WebSocketEvents.GetRecipeDetailsEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High,
            () =>
            {
                Recipe.Ingredients = args.Recipe.Ingredients;
                DisplayIngredients();
            });

            socketUtil.Socket.GetRecipeDetailsEvent -= Socket_GetRecipeDetailEvent;
        }

        private void DisplayIngredients()
        {
            ingredientList.Children.Clear();
            _ingredientElementList = new List<Uc_IngredientElement>();


            for (int i = 0; i < Recipe.Ingredients.Count; i++)
            {
                if (Recipe.Ingredients[i] != null)
                {
                    var ingredientElement = new Uc_IngredientElement(Recipe.Ingredients[i]);
                    ingredientElement.removeIngredientButton.Click += IngredientElement_RemoveIngredient;
                    _ingredientElementList.Add(ingredientElement);
                    ingredientList.Children.Add(ingredientElement);
                }
            }

            addIngredientBtn = new Uc_AddIngredientButton();
            addIngredientBtn.PointerReleased += AddIngredientBtn_PointerReleased;
            ingredientList.Children.Add(addIngredientBtn);
        }

        private void IngredientElement_RemoveIngredient(object sender, RoutedEventArgs e)
        {
            var senderButton = (Button)sender;
            var ingredientElement = (Uc_IngredientElement)senderButton.DataContext;
            // keep ingredient element list up to date
            Recipe.Ingredients.RemoveAll(i => i.Name == ingredientElement._ingredient.Name);

            DisplayIngredients();
        }

        private async void Socket_GetIngredientsEvent(object sender, WebSocketEvents.GetIngredientsEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High,
            () =>
            {
                AvailableIngredientList = new List<Ingredient>();
                // Populate AllRecipes
                for (var i = 0; i < args.Ingredients.Count; i++)
                {
                    AvailableIngredientList.Add(args.Ingredients[i]);
                }
            });

            socketUtil.Socket.GetIngredientsEvent -= Socket_GetIngredientsEvent;
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
            for (var i = 0; i < AvailableIngredientList.Count; i++)
            {
                bool ingredientFound = false;
                for (var j = 0; j < Recipe.Ingredients.Count; j++)
                {
                    if (Recipe.Ingredients[j].Name == AvailableIngredientList[i].Name)
                    {
                        ingredientFound = true;
                        break;
                    }
                }
                if (!ingredientFound)
                {
                    nonRepeatedIngredients.Add(AvailableIngredientList[i]);
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
            Recipe.Ingredients.Add(ingredientPicker.selectedIngredient);
            DisplayIngredients();
        }

        private void CancelAddIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayIngredients();
        }

        private void Pour_Drink(object sender, RoutedEventArgs e)
        {
            // Can snag ingredients from _ingredientElementList[x]._ingredient
            Console.WriteLine(_ingredientElementList);
        }
    }
}
