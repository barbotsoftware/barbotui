using MvvmCross.Platform;
using MvvmCross.Core.ViewModels;
using BarBot.Core.ViewModels;

namespace BarBot.Core
{
	public class App : MvxApplication
	{
		public App()
		{
			Mvx.RegisterSingleton<IMvxAppStart>(new MvxAppStart<DrinkMenuViewModel>());
		}
	}
}
