using BarBot.Core;
using BarBot.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BarBot.UWP.UserControls
{
    public sealed partial class Uc_CategoryTile : UserControl, INotifyPropertyChanged
    {
        private Category category;
        public Windows.UI.Xaml.Shapes.Path hexagon;
        private double lastWidth = 0;
        private double lastHeight = 0;
        private PathFigure figure;
        private double HexagonWidth = Constants.HexagonWidth;

        public Category Category
        {
            get { return category; }
            set
            {
                category = value;
                OnPropertyChanged("Category");
            }
        }

        public Uc_CategoryTile()
        {
            this.InitializeComponent();
            Category category = new Category();
            this.DataContext = this;
        }

        private void Category_Click(object sender, RoutedEventArgs e)
        {

        }
        private void hexagon_Loaded(object sender, RoutedEventArgs e)
        {
            hexagon = sender as Windows.UI.Xaml.Shapes.Path;
            hexagon.Width = Constants.HexagonWidth;
            hexagon.Height = 2 * Math.Sqrt(Math.Pow(hexagon.Width / 2, 2) - Math.Pow(hexagon.Width / 4, 2));
            CreateDataPath(hexagon.Width, hexagon.Height);
            buttonWrapper.Width = hexagon.Width;
            //recipeImage.Height = hexagon.Height - 10;

            imageButton.Height = hexagon.Height - 10;
            imageButton.Width = hexagon.Width;
            // Left, Top, Right, Bottom?
            textButtonWrapper.Margin = new Thickness((hexagon.Width - textButtonWrapper.ActualWidth) / 2, (hexagon.Height - textButtonWrapper.ActualHeight) / 2, 0, 0);
        }

        private void CreateDataPath(double width, double height)
        {
            height -= hexagon.StrokeThickness;
            width -= hexagon.StrokeThickness;

            if (lastWidth == width && lastHeight == height)
                return;

            lastWidth = width;
            lastHeight = height;

            PathGeometry geometry = new PathGeometry();
            figure = new PathFigure();

            double stroke = hexagon.StrokeThickness * 0.5;

            figure.StartPoint = new Point(0.25 * width, 0 + stroke); // Top left

            AddPoint((0.75 * width), 0 + stroke); // Top right
            AddPoint(width - stroke, (0.5 * height)); // Mid Right
            AddPoint((0.75 * width), height - stroke); // bottom Right
            AddPoint((0.25 * width), height - stroke);  // Bottom Left
            AddPoint(0 + stroke, 0.5 * height); // Middle Left
            figure.IsClosed = true;
            geometry.Figures.Add(figure);
            hexagon.Data = geometry;
        }

        private void AddPoint(double x, double y)
        {
            LineSegment segment = new LineSegment();
            segment.Point = new Point(x, y);
            figure.Segments.Add(segment);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
