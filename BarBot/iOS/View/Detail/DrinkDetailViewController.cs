using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UIKit;
using CoreGraphics;
using BarBot.Core.ViewModel;
using BarBot.Core.WebSocket;
using GalaSoft.MvvmLight.Helpers;

namespace BarBot.iOS.View.Detail
{
	public class DrinkDetailViewController : UIViewController
	{
		// Keep track of bindings to avoid premature garbage collection
		private readonly List<Binding> bindings = new List<Binding>();

		// ViewModel
		private DetailViewModel ViewModel => Application.Locator.Detail;

		// UI Elements
		List<UIView> UIElements;
		UIImageView DrinkImageView;
		UITableView IngredientTableView;

		// Data Properties
		AppDelegate Delegate;
		WebSocketUtil WebSocketUtil;
		IngredientTableDataSource source;

		public DrinkDetailViewController()
		{
		}

		// UIViewController

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			View.BackgroundColor = Color.BackgroundGray;

			UIElements = new List<UIView>();

			ConfigureHexagon();
			ConfigureIceSwitch();
			ConfigureIceLabel();
			ConfigureGarnishSwitch();
			ConfigureGarnishLabel();
			ConfigureIngredientTable();
			ConfigureOrderButton("ORDER DRINK", 20, View.Bounds.Bottom - 80, View.Bounds.Width - 40);

			AddUIElementsToView();

			Delegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			WebSocketUtil = Delegate.WebSocketUtil;
		}

		public override void ViewWillAppear(bool animated)
		{
			// Get Recipe Details
			WebSocketUtil.GetRecipeDetails(Socket_GetRecipeDetailsEvent, ViewModel.RecipeId);
		}

		public override void ViewWillDisappear(bool animated)
		{
			// Clear ViewModel
			ViewModel.Clear();
		}

		// UI Setup

		void AddUIElementsToView()
		{
			foreach (UIView view in UIElements)
			{
				View.AddSubview(view);
			}
		}

		void ConfigureHexagon()
		{
			nfloat factor = 174.0f / 200.0f;
			var point = new CGPoint(View.Frame.X + 10, View.Frame.Y + 84);
			var imgPoint = new CGPoint(point.X, point.Y - 10);
			var size = new CGSize(View.Bounds.Width / 2, factor * (View.Bounds.Width / 2));

			UIImageView HexagonImageView = new Hexagon("Images/HexagonTile.png",
										   point,
										   size);
			HexagonImageView.ContentMode = UIViewContentMode.ScaleAspectFit;

			DrinkImageView = new UIImageView();
			DrinkImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
			DrinkImageView.Frame = new CGRect(imgPoint, size);

			UIElements.Add(HexagonImageView);
			UIElements.Add(DrinkImageView);
		}

		void ConfigureIceSwitch()
		{
			var IceSwitch = new UISwitch();
			IceSwitch.OnTintColor = Color.BarBotBlue;
			IceSwitch.Frame = new CGRect(View.Bounds.Right - 66, View.Bounds.Top + 170, 51, 31);
			IceSwitch.On = true;
			UIElements.Add(IceSwitch);
		}

		void ConfigureIceLabel()
		{
			UILabel IceLabel = new UILabel()
			{
				TextColor = UIColor.White,
				TextAlignment = UITextAlignment.Center,
				Font = UIFont.FromName("Microsoft-Yi-Baiti", 36f)
			};

			IceLabel.Text = "ICE";

			//IceLabel.Layer.BorderWidth = new nfloat(0.9);
			//IceLabel.Layer.BorderColor = Color.BarBotBlue.CGColor;

			IceLabel.Frame = new CGRect(View.Frame.Right - 127, View.Bounds.Top + 165, 51, 36);
			UIElements.Add(IceLabel);
		}

		void ConfigureGarnishSwitch()
		{
			var GarnishSwitch = new UISwitch();
			GarnishSwitch.OnTintColor = Color.BarBotBlue;
			GarnishSwitch.Frame = new CGRect(View.Bounds.Right - 66, View.Bounds.Top + 216, 51, 31);
			GarnishSwitch.On = false;
			UIElements.Add(GarnishSwitch);
		}

		void ConfigureGarnishLabel()
		{
			UILabel GarnishLabel = new UILabel()
			{
				TextColor = UIColor.White,
				TextAlignment = UITextAlignment.Center,
				Font = UIFont.FromName("Microsoft-Yi-Baiti", 36f)
			};

			GarnishLabel.Text = "GARNISH";

			//GarnishLabel.Layer.BorderWidth = new nfloat(0.9);
			//GarnishLabel.Layer.BorderColor = Color.BarBotBlue.CGColor;

			GarnishLabel.Frame = new CGRect(View.Frame.Right - 207, View.Bounds.Top + 211, 131, 36);

			UIElements.Add(GarnishLabel);
		}

		void ConfigureIngredientTable()
		{
			IngredientTableView = new UITableView();
			IngredientTableView.BackgroundColor = Color.BackgroundGray;
			IngredientTableView.Frame = new CGRect(0, View.Bounds.Top + 270, View.Frame.Width, 300);

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

			UIElements.Add(IngredientTableView);
		}

		void ConfigureOrderButton(string title, nfloat x, nfloat y, nfloat width)
		{
			UIButton OrderButton = UIButton.FromType(UIButtonType.System);
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

			UIElements.Add(OrderButton);
		}

		// WebSocket Handling

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