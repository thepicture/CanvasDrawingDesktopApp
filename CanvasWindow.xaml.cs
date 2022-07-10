using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;

namespace CanvasDrawingDesktopApp
{
    /// <summary>
    /// Interaction logic for CanvasWindow.xaml
    /// </summary>
    public partial class CanvasWindow : Window
    {
        private List<Stroke> _strokes = new List<Stroke>();
        private bool _isChanging;

        public CanvasWindow()
        {
            InitializeComponent();
            Canvas.Strokes.StrokesChanged += HandleStrokesChanged;
        }

        private void HandleStrokesChanged(object sender, StrokeCollectionChangedEventArgs e)
        {
            if (_isChanging)
                return;
            if (e.Added.Count > 0)
                _strokes = Canvas.Strokes.ToList();
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
                throw new Exception("Drawing type is not implemented");
            }
        }

        private void HandleUndo(object sender, RoutedEventArgs e)
        {
            if (Canvas.Strokes.Count == 0)
                return;
            _isChanging = true;
            List<Stroke> cachedStrokes = Canvas.Strokes.ToList();
            Canvas.Strokes.Clear();
            Canvas.Strokes.Add(new StrokeCollection(cachedStrokes.Take(cachedStrokes.Count - 1)));
            _isChanging = false;
        }

        private void HandleRedo(object sender, RoutedEventArgs e)
        {
            _isChanging = true;
            int lastCount = Canvas.Strokes.Count;
            Canvas.Strokes.Clear();
            Canvas.Strokes.Add(new StrokeCollection(_strokes.Take(lastCount + 1)));
            _isChanging = false;
        }
    }
}
