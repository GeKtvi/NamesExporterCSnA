using System.Windows;
using System.Windows.Controls;

namespace NamesExporterCSnA.View
{
    /// <summary>
    /// Interaction logic for BoxImage.xaml
    /// </summary>
    public partial class BoxVisualize : UserControl
    {
        public BoxVisualize()
        {
            InitializeComponent();
        }


        public static readonly DependencyProperty DisplayableWidthProperty = DependencyProperty.Register(
            nameof(DisplayableWidth),
            typeof(string),
            typeof(BoxVisualize)
            );

        public string DisplayableWidth
        {
            get => (string)GetValue(DisplayableWidthProperty);
            set => SetValue(DisplayableWidthProperty, value);
        }


        public static readonly DependencyProperty DisplayableHeightProperty = DependencyProperty.Register(
            nameof(DisplayableHeight),
            typeof(string),
            typeof(BoxVisualize)
            );

        public string DisplayableHeight
        {
            get => (string)GetValue(DisplayableHeightProperty);
            set => SetValue(DisplayableHeightProperty, value);
        }


        public static readonly DependencyProperty DisplayableDepthProperty = DependencyProperty.Register(
            nameof(DisplayableDepth),
            typeof(string),
            typeof(BoxVisualize)
            );

        public string DisplayableDepth
        {
            get => (string)GetValue(DisplayableDepthProperty);
            set => SetValue(DisplayableDepthProperty, value);
        }
    }
}
