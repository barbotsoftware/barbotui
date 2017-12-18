using BarBot.Core.Model;
using BarBot.Core.ViewModel;
using BarBot.UWP.UserControls.RecipeDetail;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BarBot.UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
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
            MainGrid.Children.Add(new Uc_RecipeDetail(e.Parameter as RecipeDetailViewModel));
        }
    }
}
