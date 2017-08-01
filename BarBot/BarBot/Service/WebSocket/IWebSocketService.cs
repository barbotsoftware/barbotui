﻿using System.Collections.Generic;

using BarBot.Core.Model;

namespace BarBot.Core.Service.WebSocket
{
    public interface IWebSocketService
    {
        void OpenWebSocket(string username, string password);
        void CloseWebSocket();
        void AddEventHandler(string eventName);
        void RemoveEventHandler(string eventName);
        void CreateCustomRecipe(Recipe recipe);
        void GetContainers();
        void GetIngredients();
        void GetRecipeDetails(string recipeId);
        void GetRecipes();
        void OrderDrink(string recipeId, bool ice, bool garnish);
        void SetContainers<Container>(List<Container> containers);
    }
}
