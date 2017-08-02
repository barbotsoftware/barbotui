﻿namespace BarBot.Core.Service.Navigation
{
	public interface INavigationService
	{
		void OpenModal(string key);
		void CloseModal();
        void NavigateTo(string key);
	}
}
