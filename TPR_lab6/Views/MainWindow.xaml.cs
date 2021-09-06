using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TPR_lab6.ViewModels;

namespace TPR_lab6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }

        #region mininmize / maximize / close buttons
        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; }
        private void CommandBinding_Executed_Minimize(object sender, ExecutedRoutedEventArgs e) { SystemCommands.MinimizeWindow(this); }
        private void CommandBinding_Executed_Maximize(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.WindowState != WindowState.Maximized)
            {
                SystemCommands.MaximizeWindow(this);
                icon_maximize.Source = new BitmapImage(new Uri("/Views/Icons/restore.png", UriKind.Relative));
            }
            else
            {
                SystemCommands.RestoreWindow(this);
                icon_maximize.Source = new BitmapImage(new Uri("/Views/Icons/maximize.png", UriKind.Relative));
            }
        }
        private void CommandBinding_Executed_Close(object sender, ExecutedRoutedEventArgs e) { SystemCommands.CloseWindow(this); }
        #endregion
        private void UIElement_OnMouseMove(object sender, MouseEventArgs e)
        {

            var vm = (MainViewModel)DataContext;
            var chart = (CartesianChart)sender;
            if (chart.Series == null)
                return;
            //lets get where the mouse is at our chart
            var mouseCoordinate = e.GetPosition(chart);

            //now that we know where the mouse is, lets use
            //ConverToChartValues extension
            //it takes a point in pixes and scales it to our chart current scale/values
            var p = chart.ConvertToChartValues(mouseCoordinate);

            //in the Y section, lets use the raw value
            vm.YPointer = p.Y;

            //for X in this case we will only highlight the closest point.
            //lets use the already defined ClosestPointTo extension
            //it will return the closest ChartPoint to a value according to an axis.
            //here we get the closest point to p.X according to the X axis
            try
            {
                var series = chart.Series[0];
                var closetsPoint = series.ClosestPointTo(p.X, AxisOrientation.X);

                vm.XPointer = closetsPoint.X;
            }
            catch { }
        }

    }
}
