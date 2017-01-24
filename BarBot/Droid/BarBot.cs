using System;

using Android.App;
using Android.Runtime;

using Calligraphy;

using BarBot.Droid.View.Menu;

namespace BarBot.Droid
{
	[Application(ManageSpaceActivity = typeof(DrinkMenuActivity))]
	public class BarBot : Application
	{
		public BarBot(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
		}

		public override void OnCreate()
		{
			base.OnCreate();
			CalligraphyConfig.InitDefault(new CalligraphyConfig.Builder()
					.SetDefaultFontPath("fonts/Microsoft-Yi-Baiti.ttf")
					.SetFontAttrId(Resource.Attribute.fontPath)
					.Build()
			);
		}
	}
}
