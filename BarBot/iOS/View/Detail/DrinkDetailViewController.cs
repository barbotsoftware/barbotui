using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Foundation;
using UIKit;
using CoreGraphics;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Helpers;
using Microsoft.Practices.ServiceLocation;
using BarBot.Core.ViewModel;
using BarBot.Core.WebSocket;
using BarBot.iOS.Util;
using BarBot.iOS.Util.WebSocket;

namespace BarBot.iOS.View.Detail
{
	public class DrinkDetailViewController : UIViewController
	{
		// Positioning Constants
		private const int SWITCH_WIDTH = 51;
		private const int SWITCH_HEIGHT = 31;
		private const int TEXT_SIZE = 28;
		private const int OPTIONS_BOTTOM_OFFSET = 130;
		private const int ICE_SWITCH_LEFT_OFFSET = 27;
		private const int ICE_BUTTON_LEFT_OFFSET = ICE_SWITCH_LEFT_OFFSET + SWITCH_WIDTH + 10;
		private const int GARNISH_SWITCH_LEFT_OFFSET = ICE_BUTTON_LEFT_OFFSET + TEXT_SIZE + 30;
		private const int GARNISH_BUTTON_LEFT_OFFSET = GARNISH_SWITCH_LEFT_OFFSET + SWITCH_WIDTH + 10;
		private const int ORDER_LEFT_OFFSET = 20;

		// Keep track of bindings to avoid premature garbage collection
		private readonly List<Binding> bindings = new List<Binding>();

		// ViewModel
		private DetailViewModel ViewModel => Application.Locator.Detail;

		// UI Elements
		List<UIView> UIElements;
		UIImageView DrinkImageView;
		UITableView IngredientTableView;
		UISwitch IceSwitch;
		UISwitch GarnishSwitch;

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
			ConfigureIceButton();
			ConfigureGarnishSwitch();
			ConfigureGarnishButton();
			ConfigureIngredientTable();
			ConfigureOrderButton();

			AddUIElementsToView();

			Delegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			WebSocketUtil = Delegate.WebSocketUtil;
			WebSocketUtil.AddDetailEventHandlers(Socket_GetRecipeDetailsEvent, Socket_OrderDrinkEvent);
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
			var point = new CGPoint(View.Center.X - (View.Bounds.Width / 4), View.Frame.Y + 84);
			var imgPoint = new CGPoint(point.X, point.Y - 10);
			var size = new CGSize(View.Bounds.Width / 2, factor * (View.Bounds.Width / 2));

			UIImageView HexagonImageView = new Hexagon("Images/HexagonTile.png",
										   point,
										   size);
			HexagonImageView.ContentMode = UIViewContentMode.ScaleAspectFit;

			DrinkImageView = new UIImageView();
			DrinkImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
			DrinkImageView.Frame = new CGRect(imgPoint, size);
			DrinkImageView.Image = UIImage.LoadFromData(NSData.FromArray(ViewModel.ImageContents));

			UIElements.Add(HexagonImageView);
			UIElements.Add(DrinkImageView);
		}

		void ConfigureIceSwitch()
		{
			IceSwitch = new UISwitch();
			IceSwitch.OnTintColor = Color.BarBotBlue;
			IceSwitch.Frame = new CGRect(View.Bounds.Left + ICE_SWITCH_LEFT_OFFSET,
										 View.Bounds.Bottom - OPTIONS_BOTTOM_OFFSET,
										 SWITCH_WIDTH,
										 SWITCH_HEIGHT);
			IceSwitch.On = true;
			UIElements.Add(IceSwitch);
		}

		void ConfigureIceButton()
		{
			var IceButton = new UIButton();

			IceButton.SetTitleColor(UIColor.White, UIControlState.Normal);
			IceButton.TitleLabel.Font = UIFont.FromName("Microsoft-Yi-Baiti", TEXT_SIZE);

			IceButton.SetTitle("ICE", UIControlState.Normal);

			IceButton.Frame = new CGRect(View.Bounds.Left + ICE_BUTTON_LEFT_OFFSET,
										View.Bounds.Bottom - OPTIONS_BOTTOM_OFFSET,
										IceButton.IntrinsicContentSize.Width,
										TEXT_SIZE);
			IceButton.TouchUpInside += (sender, e) =>
			{
				bool IsOn = IceSwitch.On;
				IceSwitch.SetState(!IsOn, true);
			};

			UIElements.Add(IceButton);
		}

		void ConfigureGarnishSwitch()
		{
			GarnishSwitch = new UISwitch();
			GarnishSwitch.OnTintColor = Color.BarBotBlue;
			GarnishSwitch.Frame = new CGRect(View.Bounds.Left + GARNISH_SWITCH_LEFT_OFFSET, 
			                                 View.Bounds.Bottom - OPTIONS_BOTTOM_OFFSET,
			                                 SWITCH_WIDTH,
			                                 SWITCH_HEIGHT);
			GarnishSwitch.On = false;
			UIElements.Add(GarnishSwitch);
		}

		void ConfigureGarnishButton()
		{
			var GarnishButton = new UIButton();

			GarnishButton.SetTitleColor(UIColor.White, UIControlState.Normal);
			GarnishButton.TitleLabel.Font = UIFont.FromName("Microsoft-Yi-Baiti", TEXT_SIZE);

			GarnishButton.SetTitle("LIME WEDGE", UIControlState.Normal);

			GarnishButton.Frame = new CGRect(View.Bounds.Left + GARNISH_BUTTON_LEFT_OFFSET,
			                                View.Bounds.Bottom - OPTIONS_BOTTOM_OFFSET, 
			                                GarnishButton.IntrinsicContentSize.Width,
			                                TEXT_SIZE);
			GarnishButton.TouchUpInside += (sender, e) =>
			{
				bool IsOn = GarnishSwitch.On;
				GarnishSwitch.SetState(!IsOn, true);
			};

			UIElements.Add(GarnishButton);
		}

		void ConfigureIngredientTable()
		{
			IngredientTableView = new UITableView();
			IngredientTableView.BackgroundColor = Color.BackgroundGray;
			IngredientTableView.Frame = new CGRect(0, View.Bounds.Top + 270, View.Frame.Width, 250);

			IngredientTableView.RowHeight = 45;
			IngredientTableView.ScrollEnabled = true;
			IngredientTableView.ShowsVerticalScrollIndicator = true;
			IngredientTableView.AllowsSelection = false;
			IngredientTableView.Bounces = true;

			IngredientTableView.SeparatorColor = Color.NavBarGray;

			IngredientTableView.RegisterClassForCellReuse(typeof(IngredientTableViewCell), IngredientTableViewCell.CellID);
			source = new IngredientTableDataSource();
			IngredientTableView.DataSource = source;

			bindings.Add(
				this.SetBinding(
					() => ViewModel.Ingredients,
					() => source.Rows));

			UIElements.Add(IngredientTableView);
		}

		void ConfigureOrderButton()
		{
			UIButton OrderButton = UIButton.FromType(UIButtonType.System);
			OrderButton.Frame = new CGRect(ORDER_LEFT_OFFSET,
			                               View.Bounds.Bottom - 80,
			                               View.Bounds.Width - (ORDER_LEFT_OFFSET * 2),
			                               60);
			OrderButton.SetTitle("ORDER DRINK", UIControlState.Normal);
			OrderButton.SetTitleColor(UIColor.White, UIControlState.Normal);
			OrderButton.TitleLabel.Font = UIFont.FromName("Microsoft-Yi-Baiti", TEXT_SIZE);
			OrderButton.BackgroundColor = Color.BarBotBlue;
			OrderButton.Layer.CornerRadius = new nfloat(2.0);
			OrderButton.Layer.BorderWidth = new nfloat(0.9);
			OrderButton.Layer.BorderColor = Color.BarBotBlue.CGColor;
			var insets = new UIEdgeInsets(new nfloat(2.0), new nfloat(2.0), new nfloat(2.0),
								 new nfloat(2.0));
			OrderButton.TitleEdgeInsets = insets;

			OrderButton.TouchUpInside += (sender, e) =>
			{
				WebSocketUtil.OrderDrink(Socket_OrderDrinkEvent, ViewModel.RecipeId, IceSwitch.On, GarnishSwitch.On);
			};

			UIElements.Add(OrderButton);
		}

		// WebSocket Handling

		private async void Socket_GetRecipeDetailsEvent(object sender, WebSocketEvents.GetRecipeDetailsEventArgs args)
		{
			await Task.Run(() => UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
				ViewModel.Recipe = args.Recipe;
				System.Diagnostics.Debug.WriteLine(args.Recipe.RecipeId);
				Reload();
			}));
		}

		void Reload()
		{
			Title = ViewModel.Recipe.Name.ToUpper();
			IngredientTableView.ReloadSections(NSIndexSet.FromIndex(0), UITableViewRowAnimation.Automatic);
		}

		private async void Socket_OrderDrinkEvent(object sender, WebSocketEvents.OrderDrinkEventArgs args)
		{
			await Task.Run(() => UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
				var nav = ServiceLocator.Current.GetInstance<INavigationService>();
				nav.GoBack();
				System.Diagnostics.Debug.WriteLine(args.DrinkOrderId);
			}));
		}
	}
}