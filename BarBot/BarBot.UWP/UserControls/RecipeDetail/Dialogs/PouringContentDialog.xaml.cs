using BarBot.Core.Model;
using BarBot.UWP.IO;
using BarBot.UWP.Pages;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BarBot.UWP.UserControls.RecipeDetail.Dialogs
{
    public sealed partial class PouringContentDialog : ContentDialog, INotifyPropertyChanged
    {
        private BarbotIOController barbotIOController;
        private bool ice;
        private bool garnish;
        private string text;

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                OnPropertyChanged("Text");
            }
        }

        public PouringContentDialog(Recipe orderedRecipe, bool ice, bool garnish)
        {
            this.InitializeComponent();
            this.DataContext = this;

            this.barbotIOController = (Application.Current as App).barbotIOController;
            this.ice = ice;
            this.garnish = garnish;

            this.Opened += (s, a) => Dialog_Opened(s, a, orderedRecipe);

            this.Text = string.Format("Your {0} Is Pouring!", orderedRecipe.Name);
        }

        private async void Dialog_Opened(ContentDialog sender, ContentDialogOpenedEventArgs args, Recipe recipe)
        {
            Dictionary<IO.Devices.IContainer, double> ingredients = Utils.Helpers.GetContainersFromRecipe(recipe, barbotIOController.Containers);

            barbotIOController.PourDrinkSync(ingredients, ice, garnish);

            // TODO: Update with how many seconds it takes to pour a drink
            await Task.Delay(3000);

            sender.Hide();
            ((Window.Current.Content as Frame).Content as MainPage).ContentFrame.Navigate(typeof(Menu));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
