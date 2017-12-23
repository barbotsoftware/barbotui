﻿using BarBot.Core;
using BarBot.Core.Model;
using BarBot.UWP.Pages;
using BarBot.UWP.Utils;
using BarBot.UWP.Websocket;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace BarBot.UWP.UserControls.CategoryList
{
    public sealed class Tc_CategoryTile : Control, INotifyPropertyChanged
    {
        private App app;
        private Category category;
        private UWPWebSocketService webSocketService;

        public Category Category
        {
            get { return category; }
            set
            {
                category = value;
                category.Name = Helpers.UppercaseWords(category.Name);
                OnPropertyChanged("Category");
            }
        }

        public Tc_CategoryTile()
        {
            this.DefaultStyleKey = typeof(Tc_CategoryTile);
            this.DataContext = this;

            this.app = Application.Current as App;
            webSocketService = app.webSocketService;
        }

        protected override void OnApplyTemplate()
        {
            var hexButton = GetTemplateChild("HexagonButton") as Button;
            var imageButton = GetTemplateChild("CategoryImageButton") as Button;
            var hexGradient = GetTemplateChild("HexagonGradientButton") as Button;
            var recipeName = GetTemplateChild("CategoryNameButton") as Button;

            hexButton.Click += Category_Click;
            imageButton.Click += Category_Click;
            hexGradient.Click += Category_Click;
            recipeName.Click += Category_Click;
        }

        private void Category_Click(object sender, RoutedEventArgs e)
        {
            // All Recipes
            if (category.CategoryId == null || "".Equals(category.CategoryId))
            {
                if (this.app.AllRecipes.Count == 0)
                {
                    webSocketService.Socket.GetRecipesEvent += Socket_GetRecipesEvent;
                    webSocketService.GetRecipes();
                }
                else
                {
                    DisplayRecipesInMenu(this.app.AllRecipes);
                }
            }
            // Custom Recipe -> Recipe Detail
            else if (category.Name.Equals(Constants.CustomCategoryName))
            {
                var customRecipe = Recipe.CustomRecipe();
                ((Window.Current.Content as Frame).Content as MainPage).ContentFrame.Navigate(typeof(Pages.RecipeDetail), customRecipe, new DrillInNavigationTransitionInfo());
            }
            // Get Category, Event handler deals with displaying Recipes for a Category
            else
            {
                webSocketService.Socket.GetCategoryEvent += Socket_GetCategoryEvent;
                webSocketService.GetCategory(category.CategoryId);
            }
        }

        private async void Socket_GetCategoryEvent(object sender, Core.WebSocket.WebSocketEvents.GetCategoryEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High,
            () =>
            {
                // If category has sub categories, navigate to category list, populate with sub category list
                // Otherwise, navigate to menu with recipe list
                if (args.Category.SubCategories != null && args.Category.SubCategories.Count > 0)
                {
                    // Map Category to Sub Categories and pass as navigation parameter
                    Dictionary<string, List<Category>> categories = new Dictionary<string, List<Category>>
                    {
                        { args.Category.Name, args.Category.SubCategories }
                    };

                    ((Window.Current.Content as Frame).Content as MainPage).ContentFrame.Navigate(typeof(Menu), categories, new DrillInNavigationTransitionInfo());
                }
                else
                {
                    webSocketService.Socket.GetRecipesEvent += Socket_GetRecipesEvent;
                    webSocketService.GetRecipes(args.Category.CategoryId);
                }
            });

            webSocketService.Socket.GetCategoryEvent -= Socket_GetCategoryEvent;
        }

        private async void Socket_GetRecipesEvent(object sender, Core.WebSocket.WebSocketEvents.GetRecipesEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High,
            () =>
            {
                if (Category.Name == Constants.AllRecipesCategoryName)
                {
                    this.app.AllRecipes = args.Recipes;
                }
                DisplayRecipesInMenu(args.Recipes);
            });

            webSocketService.Socket.GetRecipesEvent -= Socket_GetRecipesEvent;
        }

        private void DisplayRecipesInMenu(List<Recipe> recipes)
        {
            Dictionary<string, List<Recipe>> recipeDictionary = new Dictionary<string, List<Recipe>>
            {
                { Category.Name, recipes }
            };

            ((Window.Current.Content as Frame).Content as MainPage).ContentFrame.Navigate(typeof(Menu), recipeDictionary, new DrillInNavigationTransitionInfo());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
