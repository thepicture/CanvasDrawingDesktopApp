using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CanvasDrawingDesktopApp
{
    /// <summary>
    /// Interaction logic for CanvasWindow.xaml
    /// </summary>
    public partial class CanvasWindow : Window
    {
        public CanvasWindow()
        {
            InitializeComponent();
        }

        private void HandleColorChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CanvasAttributes == null)
                return;
            object selectedItem = (sender as ComboBox).SelectedItem;
            PropertyInfo colorProperty = selectedItem as PropertyInfo;
            Color color = (Color)colorProperty.GetValue(colorProperty);
            CanvasAttributes.Color = color;
            SelectedColorPresenter.Fill = new SolidColorBrush(color);
        }

        private void HandleBrushSizeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (CanvasAttributes != null)
                CanvasAttributes.Width = CanvasAttributes.Height = (sender as Slider).Value;
        }

        private void HandleDrawingType(object sender, RoutedEventArgs e)
        {
            if (CanvasAttributes == null)
                return;
            if (DrawingType.IsChecked.HasValue && DrawingType.IsChecked.Value)
            {
                Canvas.EditingMode = InkCanvasEditingMode.Ink;
            }
            else if (EditType.IsChecked.HasValue && EditType.IsChecked.Value)
            {
                Canvas.EditingMode = InkCanvasEditingMode.Select;
            }
            else if (DeleteType.IsChecked.HasValue && DeleteType.IsChecked.Value)
            {
                Canvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
            }
            else
            {
                throw new System.Exception("Drawing type is not implemented");
            }
        }
    }
}
