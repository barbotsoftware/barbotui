using Foundation;
using UIKit;

using System.Net.Http;
using System.Threading.Tasks;

using BarBot.Model;

namespace BarBot.iOS.ViewModel
{
	public class RecipeViewModel
	{
		public readonly Recipe Recipe;
		public readonly string RecipeName;
		public readonly UIImage DrinkImage;
		public readonly Step[] RecipeSteps;

		public RecipeViewModel(Recipe recipe)
		{
			Recipe = recipe;
			RecipeName = recipe.Name.ToUpper();
			//DrinkImage = await LoadImage(recipe.Img);
		}

		async Task<UIImage> LoadImage(string imageUrl)
		{
			var httpClient = new HttpClient();

			// await! control returns to the caller and the task continues to run on another thread
			var contents = await httpClient.GetByteArrayAsync(imageUrl);

			// load from bytes
			return UIImage.LoadFromData(NSData.FromArray(contents));
		}
	}
}
