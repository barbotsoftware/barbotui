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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using BarBot.Core;
using BarBot.Core.Model;
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
        private double HexagonWidth = Constants.HexagonWidth;
        App app;
        private BitmapImage _cachedImage;
        public BitmapImage CachedImage
        {
            get { return _cachedImage; }
            set
            {
                _cachedImage = value;
                OnPropertyChanged("CachedImage");
            }
        }

        private string webserverUrl;

        public Windows.UI.Xaml.Shapes.Path hexagon;

        public Uc_RecipeTile()
        {
            this.InitializeComponent();

            app = Application.Current as App;

            Recipe = new Recipe();
            this.DataContext = this;

            webserverUrl = (Application.Current as App).webserverUrl;
        }

        public Recipe Recipe
        {
            get
            {
                return recipe;
            }

            set
            {
                value.Img = "http://" + webserverUrl + "/" + value.Img;
                recipe = value;
                if (value.Name != null)
                {

                    if (value.Name.Equals("Custom Recipe"))
                    {
                        var imageUri = new Uri("http://" + webserverUrl + "/barbotweb/public/img/recipe_images/custom_recipe.png");
                        var recipeImage = new BitmapImage(imageUri);
                        CachedImage = recipeImage;
                    }
                    else
                    {
                        CachedImage = app.getCachedImage(value);
                    }
                }
                OnPropertyChanged("Recipe");
            }
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

            //recipeNameGrid.Height = hexagon.Height;
            //recipeNameGrid.Width = hexagon.Width;
            //buttonWrapper.Height = hexagon.Height - 10;
            //buttonWrapper.Margin = new Thickness(0, 0, 0, 20);
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

            //double lineSegmentLength = width / 2;
            //double segmentHeight = Math.Sqrt(Math.Pow(lineSegmentLength, 2) - Math.Pow(lineSegmentLength/2, 2));

            double stroke = hexagon.StrokeThickness * 0.5;

            figure.StartPoint = new Point(0.25 * width, 0 + stroke); // Top left

            //LineSegment segment = new LineSegment();
            //segment.Point = new Point(0.75 + 0.5 * hexagon.StrokeThickness, 0 + 0.5 * hexagon.StrokeThickness);
            //figure.Segments.Add(segment);

            //figure.Segments.Add(new LineSegment(new Point(0.75 * width, 0)));

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

        private void Drink_Detail(object sender, RoutedEventArgs e)
        {
            ((Window.Current.Content as Frame).Content as MainPage).ContentFrame.Content = new Uc_DrinkDetail(Recipe);
        }
    }
}
