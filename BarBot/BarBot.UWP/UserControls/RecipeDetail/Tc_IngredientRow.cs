using BarBot.Core.Model;
using BarBot.UWP.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace BarBot.UWP.UserControls.RecipeDetail
{
    public sealed class Tc_IngredientRow : Control, INotifyPropertyChanged
    {
        private Ingredient ingredient;
        private string volumeText;

        public List<Ingredient> AvailableIngredients;

        // Action Button Click Event Handlers
        private RoutedEventHandler DecrementVolumeButton_Click;
        private RoutedEventHandler IncrementVolumeButton_Click;
        private RoutedEventHandler RemoveIngredientButton_Click;

        public Ingredient Ingredient
        {
            get
            {
                return ingredient;
            }

            set
            {
                ingredient = value;
                ingredient.Name = Helpers.UppercaseWords(ingredient.Name);
                VolumeText = ingredient.Amount.ToString() + " oz";
                OnPropertyChanged("Ingredient");
            }
        }

        public string VolumeText
        {
            get
            {
                return volumeText;
            }

            set
            {
                volumeText = value;
                OnPropertyChanged("VolumeText");
            }
        }

        public Tc_IngredientRow(Ingredient ingredient,
                                List<Ingredient> availableIngredients,
                                RoutedEventHandler decrementVolumeButton_Click,
                                RoutedEventHandler incrementVolumeButton_Click,
                                RoutedEventHandler removeIngredientButton_Click)
        {
            this.DefaultStyleKey = typeof(Tc_IngredientRow);
            this.DataContext = this;

            Ingredient = ingredient;
            AvailableIngredients = availableIngredients;

            DecrementVolumeButton_Click += decrementVolumeButton_Click;
            IncrementVolumeButton_Click += incrementVolumeButton_Click;
            RemoveIngredientButton_Click += removeIngredientButton_Click;
        }

        protected override void OnApplyTemplate()
        {
            var decrementVolumeButton = GetTemplateChild("DecrementVolumeButton") as Button;
            var incrementVolumeButton = GetTemplateChild("IncrementVolumeButton") as Button;
            var removeIngredientButton = GetTemplateChild("RemoveIngredientButton") as Button;

            decrementVolumeButton.Click += DecrementVolumeButton_Click;
            incrementVolumeButton.Click += IncrementVolumeButton_Click;
            removeIngredientButton.Click += RemoveIngredientButton_Click;

            List<Button> buttons = new List<Button>()
            {
                decrementVolumeButton,
                incrementVolumeButton,
                removeIngredientButton
            };

            foreach (Button btn in buttons)
            {                
                btn.AddHandler(PointerPressedEvent, new PointerEventHandler(Pointer_Pressed), true);
                btn.AddHandler(PointerReleasedEvent, new PointerEventHandler(Pointer_Released), true);
            }
        }

        // Ingredient Row Action is pressed
        
        private void Pointer_Released(object sender, PointerRoutedEventArgs e)
        {
            this.CapturePointer(e.Pointer);
            VisualStateManager.GoToState(sender as Button, "PointerUp", true);
        }

        private void Pointer_Pressed(object sender, PointerRoutedEventArgs e)
        {
            this.CapturePointer(e.Pointer);
            VisualStateManager.GoToState(sender as Button, "PointerDown", true);
        }

        // Entire Ingredient Row is pressed

        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            this.CapturePointer(e.Pointer);
            VisualStateManager.GoToState(this, "PointerDown", true);
        }

        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "PointerUp", true);
            this.ReleasePointerCapture(e.Pointer);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
