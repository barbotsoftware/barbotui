using System;
using System.Collections.Generic;
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
using BarBot.Model;
using System.ComponentModel;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BarBot.UWP.UserControls
{
    public sealed partial class Uc_RecipeTile : UserControl, INotifyPropertyChanged
    {
        private Recipe recipe;

        private double lastWidth = 0;
        private double lastHeight = 0;
        private PathFigure figure;

        public Uc_RecipeTile()
        {
            this.InitializeComponent();

            Recipe = new Recipe();
            this.DataContext = this;
        }

        public Recipe Recipe
        {
            get
            {
                return recipe;
            }

            set
            {
                recipe = value;
                OnPropertyChanged("Recipe");
            }
        }

        private void hexagon_Loaded(object sender, RoutedEventArgs e)
        {
            Windows.UI.Xaml.Shapes.Path hexagon = sender as Windows.UI.Xaml.Shapes.Path;
            CreateDataPath(hexagon.Width, hexagon.Height);
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

            figure.StartPoint = new Point(0.25 * width, 0);
            AddPoint(0.75 * width, 0);
            AddPoint(width, 0.5 * height);
            AddPoint(0.75 * width, height + (0.5 * hexagon.StrokeThickness));
            AddPoint(0.25 * width, height);
            AddPoint(0, 0.5 * height);
            figure.IsClosed = true;
            geometry.Figures.Add(figure);
            hexagon.Data = geometry;
        }

        private void AddPoint(double x, double y)
        {
            LineSegment segment = new LineSegment();
            segment.Point = new Point(x + 0.5 * hexagon.StrokeThickness,
                y + 0.5 * hexagon.StrokeThickness);
            figure.Segments.Add(segment);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
