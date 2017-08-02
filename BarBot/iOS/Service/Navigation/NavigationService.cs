using System;
using System.Collections.Generic;

using UIKit;

namespace BarBot.iOS.Service.Navigation
{
	public class NavigationService : GalaSoft.MvvmLight.Views.NavigationService, Core.Service.Navigation.INavigationService
	{
		private Dictionary<string, Type> _pageKeys = new Dictionary<string, Type>();

		public new void Initialize(UINavigationController navigation)
		{
			base.Initialize(navigation);

			Core.Service.Navigation.NavigationService.Current = this;
		}

		public void OpenModal(string key)
		{
			var navigationController = UIApplication.SharedApplication.KeyWindow.RootViewController;

			var type = _pageKeys[key];
			var viewController = (UIViewController)Activator.CreateInstance(type);

			navigationController.PresentViewController(viewController, true, null);
		}

		public void CloseModal()
		{
			var navigationController = UIApplication.SharedApplication.KeyWindow.RootViewController;
			navigationController.DismissViewController(true, null);
		}

		public new void Configure(string key, Type controllerType)
		{
			base.Configure(key, controllerType);

			_pageKeys.Add(key, controllerType);
		}
	}
}