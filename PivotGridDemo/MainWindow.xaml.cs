using System;
using System.Windows;

namespace PivotGridDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel viewModel = new MainViewModel();
        public MainWindow()
        {
            InitializeComponent();
            // Set the DataContext of the window to the view model
            DataContext = viewModel;

            // Subscribe to the DataRefreshing event of the pivot grid
            pivotGrid.DataRefreshing += PivotGrid_DataRefreshing;
        }

        private async void PivotGrid_DataRefreshing(object sender, Syncfusion.Windows.Controls.PivotGrid.DataRefreshingArgs e)
        {
            if (viewModel != null)
                viewModel.IsBusy = true;

            // Queues a work item to run on a ThreadPool thread (background thread)
            // This prevents blocking the UI thread during processing
            System.Threading.ThreadPool.QueueUserWorkItem(_ =>
            {
                // Switch back to the UI thread using Dispatcher
                // UI elements can only be updated from the UI thread
                pivotGrid.InternalGrid.Dispatcher.Invoke(new Action(() =>
                {
                    if (viewModel != null)
                        viewModel.IsBusy = false;
                }), System.Windows.Threading.DispatcherPriority.ContextIdle);
            });
        }
    }
}
