using BarBot.Core.Model;
using BarBot.Core.ViewModel;
using BarBot.UWP.UserControls.RecipeDetail;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace BarBot.UWP.Pages
{
    public sealed partial class RecipeDetail : Page
    {
        public RecipeDetail()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            MainGrid.Children.Clear();
            MainGrid.Children.Add(new Uc_RecipeDetail(e.Parameter as Recipe));
        }
    }
}
