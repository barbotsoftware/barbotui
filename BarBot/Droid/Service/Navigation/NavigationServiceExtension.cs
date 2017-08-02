using System;
using System.Collections.Generic;
using System.Reflection;

using Android.App;
using Android.Content;

using GalaSoft.MvvmLight.Views;

using BarBot.Core.Service.Navigation;

namespace BarBot.Droid.Service.Navigation
{
	public class NavigationServiceExtension : NavigationService, INavigationServiceExtension
	{
		private Dictionary<string, Type> _pageKeys = new Dictionary<string, Type>();

		private const string ParameterKeyName = "ParameterKey";

		public void Initialize()
		{
			Core.Service.Navigation.NavigationServiceExtension.Current = this;
		}

		public void OpenModal(string key)
		{
			var activityType = _pageKeys[key];

			var intent = new Intent(Application.Context, activityType);
			intent.SetFlags(ActivityFlags.NoHistory);
			intent.SetFlags(ActivityFlags.NewTask);

			Application.Context.StartActivity(intent);
		}

		public void OpenModal(string key, object parameter)
		{
			var activityType = _pageKeys[key];

			var intent = new Intent(Application.Context, activityType);
			intent.SetFlags(ActivityFlags.NoHistory);
			intent.SetFlags(ActivityFlags.NewTask);

			var field = this.GetType().BaseType.GetField("_parametersByKey", BindingFlags.Instance | BindingFlags.NonPublic);

			var parameters = field.GetValue(this) as Dictionary<string, object>;

			var guid = Guid.NewGuid().ToString();
			parameters.Add(guid, parameter);
			intent.PutExtra(ParameterKeyName, guid);

			Application.Context.StartActivity(intent);
		}

		public void CloseModal()
		{
			ActivityBase.CurrentActivity.Finish();
		}

		public new void Configure(string key, Type controllerType)
		{
			base.Configure(key, controllerType);

			_pageKeys.Add(key, controllerType);
		}
	}
}
