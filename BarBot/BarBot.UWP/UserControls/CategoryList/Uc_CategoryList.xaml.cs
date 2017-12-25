﻿using BarBot.Core;
using BarBot.Core.Model;
using BarBot.UWP.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BarBot.UWP.UserControls.CategoryList
{
    public sealed partial class Uc_CategoryList : UserControl, INotifyPropertyChanged
    {
        private List<Category> categories;

        private int page = 0;
        private int itemsPerPage = 10;
        private int pages = 1;

        public List<Category> Categories
        {
            get { return categories; }
            set
            {
                categories = value;

                // Add Custom Cocktail 'Category'. Clicking this goes to Recipe Detail
                Category customCategory = new Category(Constants.CustomCategoryId, Constants.CustomCategoryName, null, null);

                if (categories.Count == 0)
                {
                    categories.Add(customCategory);
                }
                else if (categories[0].CategoryId == null || "".Equals(categories[0].CategoryId))
                {
                    categories.Insert(1, customCategory);
                }

                pages = (categories.Count + itemsPerPage - 1) / itemsPerPage;
                Page = 0;
                OnPropertyChanged("Categories");
            }
        }

        public int Page
        {
            get { return page; }
            set
            {
                page = value;
                displayPage(page);
            }
        }

        public Uc_CategoryList()
        {
            this.InitializeComponent();
        }

        private void displayPage(int page)
        {
            // Hide BackButton if it's the first page, hide NextButton if its the last page
            CategoryListBackButton.Visibility = page == 0 ? Visibility.Collapsed : Visibility.Visible;
            CategoryListNextButton.Visibility = page >= pages - 1 ? Visibility.Collapsed : Visibility.Visible;

            // clear out the current category tiles
            CategoryListCanvas.Children.Clear();

            for (int i = 0 + (itemsPerPage * page); i < Math.Min((itemsPerPage * page) + itemsPerPage, categories.Count); i++)
            {
                Tc_CategoryTile tile = new Tc_CategoryTile();
                tile.Category = categories[i];
                Point pos = Helpers.GetPoint(i % itemsPerPage, Constants.HexagonWidth);
                Canvas.SetLeft(tile, pos.X);
                Canvas.SetTop(tile, pos.Y);
                CategoryListCanvas.Children.Add(tile);
            }
        }

        private void Next_Page(object sender, RoutedEventArgs e)
        {
            page++;
            displayPage(page);
        }

        private void Previous_Page(object sender, RoutedEventArgs e)
        {
            page--;
            displayPage(page);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
