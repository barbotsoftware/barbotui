using System;

using Android.App;

using GalaSoft.MvvmLight.Views;

namespace BarBot.Droid.View
{
	[Activity(Label = "BaseActivity")]
	public class BaseActivity : ActivityBase
	{
		protected static String m_TAG = "DrinkMenuActivity";
		public static bool m_isAppWentToBg = false;
		public static bool m_isWindowFocused = false;
		public static bool m_isMenuOpened = false;
		public static bool m_isBackPressed = false;
		public static int s_activitycounter = 0;
		public static bool s_mainactivityvisible = false;

		public override void OnWindowFocusChanged(bool hasFocus)
		{
			m_isWindowFocused = hasFocus;

			if (m_isBackPressed && !hasFocus)
			{
				m_isBackPressed = false;
				m_isWindowFocused = true;
			}

			base.OnWindowFocusChanged(hasFocus);
		}

		public void ApplicationDidEnterBackground()
		{
			m_isAppWentToBg = true;

			// Disconnect when App enters background
			App.DisconnectWebSocket();
		}

		public override void OnBackPressed()
		{
			if (this is BaseActivity)
			{
			}
			else
			{
				m_isBackPressed = true;
			}

			base.OnBackPressed();
		}

		protected override void OnPause()
		{
			if (s_mainactivityvisible == false && s_activitycounter < 1)
			{
				ApplicationDidEnterBackground();
			}

			App.SaveSharedPreferences();
			base.OnPause();
		}

		protected override void OnResume()
		{
			App.LoadSharedPreferences();

			if (m_isAppWentToBg)
			{
				App.ConnectWebSocket();
				m_isAppWentToBg = false;
			}

			base.OnResume();
		}
	}
}
