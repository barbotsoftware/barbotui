using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using GalaSoft.MvvmLight.Views;

using BarBot.Core.ViewModel;

namespace BarBot.Droid.Util
{
	public class NavigationServiceExtension : NavigationService, INavigationServiceExtension
	{
		private Dictionary<string, Type> _pageKeys = new Dictionary<string, Type>();

		private const string ParameterKeyName = "ParameterKey";

		public void Initialize()
		{
			Core.ViewModel.NavigationServiceExtension.Current = this;
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
