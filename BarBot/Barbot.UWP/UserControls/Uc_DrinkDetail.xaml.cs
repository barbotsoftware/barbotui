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
using System.Collections.ObjectModel;
using System.Threading.Tasks;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BarBot.UWP.UserControls
{
    public sealed partial class Uc_DrinkDetail : UserControl, INotifyPropertyChanged
    {
        private Recipe _recipe;
        private WebSocketUtil socketUtil;
        private List<Uc_IngredientElement> _ingredientElementList;
        private List<Ingredient> AvailableIngredientList;
        private Recipe OrderRecipe;
        double _totalVolume;
        App app;
        Uc_AddIngredientButton addIngredientBtn;
        Uc_IngredientPicker ingredientPicker;

        public Uc_DrinkDetail(Recipe SelectedRecipe)
        {
            app = Application.Current as App;
            socketUtil = app.webSocketUtil;

            socketUtil.AddMenuEventHandlers(null, Socket_GetIngredientsEvent);
            socketUtil.GetIngredients();

            if (SelectedRecipe == null)
            {
                SelectedRecipe = FuckMeUp();
            }

            this.InitializeComponent();
            this.DataContext = this;
            Recipe = SelectedRecipe;

            // Moved init to the handler of getIngredients
        }

        public void init()
        {
            TotalVolume = 0;

            if (Recipe.Name.Equals("Fuck Me Up"))
            {
                Recipe.Ingredients = LoadEmUpBoiz();
                DisplayIngredients();
                // 
            }
            else if (Recipe.Name != "Custom Recipe")
            {
                // Attach event handler and then call GetRecipeDetails
                socketUtil.AddDetailEventHandlers(Socket_GetRecipeDetailEvent, null, null);
                socketUtil.GetRecipeDetails(Recipe.RecipeId);
            }
            else
            {
                DisplayIngredients();
            }
        }

        public double TotalVolume
        {
            get
            {
                return _totalVolume;
            }

            set
            {
                _totalVolume = value;
                OnPropertyChanged("TotalVolume");
            }
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

            TotalVolume = 0;
            for (int i = 0; i < Recipe.Ingredients.Count; i++)
            {
                TotalVolume += Recipe.Ingredients[i].Quantity;
            }

            double volumeAvailable = Constants.MaxVolume - TotalVolume;


            for (int i = 0; i < Recipe.Ingredients.Count; i++)
            {
                if (Recipe.Ingredients[i] != null)
                {
                    var ingredientElement = new Uc_IngredientElement(Recipe.Ingredients[i], volumeAvailable);
                    ingredientElement.VolumeAvailable = volumeAvailable;
                    ingredientElement.removeIngredientButton.Click += IngredientElement_RemoveIngredient;
                    _ingredientElementList.Add(ingredientElement);
                    ingredientList.Children.Add(ingredientElement);
                    //TotalVolume += (double)ingredientElement.ingredientVolume.Items[ingredientElement.ingredientVolume.SelectedIndex];
                    // Event for when volume is changed. Used for tracking total volume
                    ingredientElement.ingredientVolume.SelectionChanged += IngredientElement_SelectionChangeEvent;
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

        private void IngredientElement_SelectionChangeEvent(object sender, SelectionChangedEventArgs e)
        {
            var senderComboBox = (ComboBox)sender;
            var ingredientElement = (Uc_IngredientElement)senderComboBox.DataContext;

            // Boolean VolumeChangeInProgress is true when the ingredientElements is re-populating combobox
            if (ingredientElement.VolumeChangeInProgress != true)
            {
                // Re-count totals
                TotalVolume = 0;
                for (int i = 0; i < _ingredientElementList.Count; i++)
                {
                    TotalVolume += _ingredientElementList[i].Ingredient.Quantity;
                }

                // Now that we have the new total, we need to limit ingredientElements to a value
                double volumeAvailable = Constants.MaxVolume - TotalVolume;

                for (int i = 0; i < _ingredientElementList.Count; i++)
                {
                    _ingredientElementList[i].VolumeAvailable = volumeAvailable;
                }
            };
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

                init();
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

            TotalVolume = 0;
            // Calculate total volume to ensure added ingredient is within limits
            for (int i = 0; i < Recipe.Ingredients.Count; i++)
            {
                TotalVolume += Recipe.Ingredients[i].Quantity;
            }

            if (TotalVolume + ingredientPicker.selectedIngredient.Quantity > Constants.MaxVolume)
            {
                // set ingredient volume to be max within limits
                ingredientPicker.selectedIngredient.Quantity = Constants.MaxVolume - TotalVolume;
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
            if (app.barbotIOController.CupCount == 0)
            {
                var cupDialog = new ContentDialog()
                {
                    MaxWidth = ActualWidth,
                    Content = new TextBlock()
                    {
                        Text = "There are no cups left! Please place a cup or reset the cup dispenser.",
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        FontSize = 45
                    },
                    Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 56, 114)),
                    Foreground = new SolidColorBrush(Windows.UI.Colors.White),
                    BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(100, 34, 34, 34)),
                    IsPrimaryButtonEnabled = true,
                    IsSecondaryButtonEnabled = true,
                    PrimaryButtonText = "OK",
                    SecondaryButtonText = "RESET"
                };

                cupDialog.PrimaryButtonClick += CupDialog_PrimaryButtonClick;
                cupDialog.SecondaryButtonClick += CupDialog_SecondaryButtonClick;
                await cupDialog.ShowAsync();
            }

            // Can snag ingredients from _ingredientElementList[x]._ingredient
            Console.WriteLine(_ingredientElementList);
            ((Window.Current.Content as Frame).Content as MainPage).ContentFrame.Content = new Uc_PartyMode();

            OrderRecipe = new Recipe();
            OrderRecipe.Name = Recipe.Name;
            OrderRecipe.Ingredients = new List<Ingredient>();

            for (int i = 0; i < _ingredientElementList.Count; i++)
            {
                OrderRecipe.Ingredients.Add(_ingredientElementList[i]._ingredient);
            }

            var dialog = new ContentDialog()
            {
                //MaxWidth = this.ActualWidth,
                Content = new TextBlock()
                {
                    Text = string.Format("Your {0} Is Pouring!", OrderRecipe.Name),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextWrapping = TextWrapping.WrapWholeWords,
                    //Width = 1200,
                    FontSize = 45
                },
                Width = 1200,
                Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 56, 114)),
                Foreground = new SolidColorBrush(Windows.UI.Colors.White),
                BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(100, 34, 34, 34))
            };

            dialog.Opened += (s, a) => Dialog_Opened(s, a, OrderRecipe);
            await dialog.ShowAsync();
        }

        private async void Dialog_Opened(ContentDialog sender, ContentDialogOpenedEventArgs args, Recipe recipe)
        {
            await Task.Delay(1);

            Dictionary<IO.Devices.IContainer, double> ingredients = Utils.Helpers.GetContainersFromRecipe(OrderRecipe, app.barbotIOController.Containers);

            app.barbotIOController.PourDrink(ingredients, AddIce.IsChecked.Value, AddGarnish.IsChecked.Value);

            sender.Hide();
            ((Window.Current.Content as Frame).Content as MainPage).ContentFrame.Content = new Uc_PartyMode();
        }

        private void CupDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            app.barbotIOController.CupCount = 25;

            sender.Hide();
        }

        private void CupDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            app.barbotIOController.CupCount = 1;

            sender.Hide();
        }

        private Recipe FuckMeUp()
        {
            Recipe FuckMeUp = new Recipe();
            FuckMeUp.Name = "Fuck Me Up";
            FuckMeUp.Img = "barbotweb/public/img/recipe_images/dickbutt.png";
            FuckMeUp.Ingredients = new List<Ingredient>();

            return FuckMeUp;
        }

        private List<Ingredient> LoadEmUpBoiz()
        {
            List<Ingredient> demIngredients = new List<Ingredient>();
            double totalQuantity = 0;
            if (AvailableIngredientList != null)
            {
                List<int> usedIngredients = new List<int>();
                Random r = new Random();
                int ingredientCount = r.Next(3, AvailableIngredientList.Count);
                for (int i = 0; i < ingredientCount; i++)
                {
                    bool ingredientChosen = false;
                    while (!ingredientChosen)
                    {
                        int ingredientID = r.Next(0, AvailableIngredientList.Count);
                        if (usedIngredients.IndexOf(ingredientID) < 0)
                        {
                            usedIngredients.Add(ingredientID);
                            Ingredient ingy = AvailableIngredientList[ingredientID];
                            double quant = r.Next(1, (int)Constants.MaxVolume / 2);
                            if (totalQuantity + quant > Constants.MaxVolume)
                            {
                                quant = Constants.MaxVolume - totalQuantity;
                            }
                            totalQuantity += quant;
                            ingy.Quantity = quant;
                            demIngredients.Add(AvailableIngredientList[ingredientID]);
                            ingredientChosen = true;
                        }
                    }
                    if(totalQuantity >= Constants.MaxVolume)
                    {
                        break;
                    }
                }
            }
            return demIngredients;
        }
    }
}
