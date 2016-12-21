using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UIKit;
using CoreGraphics;
using BarBot.Core.Model;
using BarBot.Core.ViewModel;
using BarBot.Core.WebSocket;
using GalaSoft.MvvmLight.Helpers;

namespace BarBot.iOS.View.Detail
{
	public class DrinkDetailViewController : UIViewController
    {
		// Keep track of bindings to avoid premature garbage collection
		private readonly List<Binding> bindings = new List<Binding>();

		private DetailViewModel ViewModel => Application.Locator.Detail;

		UIImageView DrinkImageView;
		UIImageView HexagonImageView;
		UITableView IngredientTableView;
		UIButton OrderButton;

		AppDelegate Delegate;
		WebSocketUtil WebSocketUtil;
		IngredientTableDataSource source;
        
		public DrinkDetailViewController()
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			View.BackgroundColor = Color.BackgroundGray;
			ConfigureHexagon();
			ConfigureIngredientTable();
			ConfigureOrderButton("ORDER DRINK", 20, View.Bounds.Bottom - 80, View.Bounds.Width - 40);

			Delegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			WebSocketUtil = Delegate.WebSocketUtil;
			WebSocketUtil.GetIngredients(Socket_GetIngredientsEvent);
		}

		public override void ViewWillAppear(bool animated)
		{
			// Get Recipe Details on Appear
			WebSocketUtil.GetRecipeDetails(Socket_GetRecipeDetailsEvent, ViewModel.RecipeId);
		}

		public override void ViewWillDisappear(bool animated)
		{
			// Clear ViewModel on Disappear
			ViewModel.Clear();
		}

		void ConfigureHexagon()
		{
			nfloat factor = 174.0f / 200.0f;
			var point = new CGPoint(View.Frame.X + 10, View.Frame.Y + 75);
			var imgPoint = new CGPoint(point.X, point.Y - 10);
			var size = new CGSize(View.Bounds.Width / 2, factor * (View.Bounds.Width / 2));

			HexagonImageView = new Hexagon("Images/HexagonTile.png",
										   point,
										   size);
			HexagonImageView.ContentMode = UIViewContentMode.ScaleAspectFit;

			DrinkImageView = new UIImageView();
			DrinkImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
			DrinkImageView.Frame = new CGRect(imgPoint, size);

			View.AddSubview(HexagonImageView);
			View.AddSubview(DrinkImageView);
		}

		void ConfigureIngredientTable()
		{
			IngredientTableView = new UITableView();
			IngredientTableView.BackgroundColor = Color.BackgroundGray;
			IngredientTableView.Frame = new CGRect(0, HexagonImageView.Frame.Bottom + 25, View.Frame.Width, 300);

			IngredientTableView.RowHeight = 45;
			IngredientTableView.ScrollEnabled = true;
			IngredientTableView.ShowsVerticalScrollIndicator = true;
			IngredientTableView.AllowsSelection = false;
			IngredientTableView.Bounces = true;

			IngredientTableView.RegisterClassForCellReuse(typeof(IngredientTableViewCell), IngredientTableViewCell.CellID);
			source = new IngredientTableDataSource();
			IngredientTableView.DataSource = source;

			bindings.Add(
				this.SetBinding(
					() => ViewModel.Ingredients,
					() => source.Rows));

			View.AddSubview(IngredientTableView);
		}

		void ConfigureOrderButton(string title, nfloat x, nfloat y, nfloat width)
		{
			OrderButton = UIButton.FromType(UIButtonType.System);
			OrderButton.Frame = new CGRect(x, y, width, 60);
			OrderButton.SetTitle(title, UIControlState.Normal);
			OrderButton.SetTitleColor(UIColor.White, UIControlState.Normal);
			OrderButton.Font = UIFont.FromName("Microsoft-Yi-Baiti", 23f);
			OrderButton.BackgroundColor = Color.BarBotBlue;
			OrderButton.Layer.CornerRadius = new nfloat(2.0);
			OrderButton.Layer.BorderWidth = new nfloat(0.9);
			OrderButton.Layer.BorderColor = Color.BarBotBlue.CGColor;
			var insets = new UIEdgeInsets(new nfloat(2.0), new nfloat(2.0), new nfloat(2.0),
								 new nfloat(2.0));
			OrderButton.TitleEdgeInsets = insets;

			View.AddSubview(OrderButton);
		}

		private async void Socket_GetIngredientsEvent(object sender, WebSocketEvents.GetIngredientsEventArgs args)
		{
			await Task.Run(() => UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
				Delegate.IngredientsInBarBot.Ingredients = args.Ingredients;
			}));
		}

		void UpdateIngredients()
		{
			
		}

		private async void Socket_GetRecipeDetailsEvent(object sender, WebSocketEvents.GetRecipeDetailsEventArgs args)
		{
			await Task.Run(() => UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
				ViewModel.Recipe = args.Recipe;
			}));
			Reload();
		}

		async void Reload()
		{
			Title = ViewModel.Recipe.Name.ToUpper();
			// TODO: Pass image instead of getting from ws server again
			DrinkImageView.Image = await AsyncUtil.LoadImage(ViewModel.Recipe.Img);
			IngredientTableView.ReloadData();
		}
    }
}