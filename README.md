# How to display a custom BusyIndicator during WPF PivotGridControl calculations?

In [WPF PivotGridControl](https://www.syncfusion.com/wpf-controls/pivot-grid), you can use a custom busyindicator by using **SfBusyIndicator**. To achieve this, place the **PivotGrid** inside the **SfBusyIndicator**.

**Code Snippet:**
```xml
<syncfusion:SfBusyIndicator IsBusy="{Binding IsBusy}" x:Name="busyIndicator" Grid.Column="0">
    <syncfusion:PivotGridControl x:Name="pivotGrid" ItemSource="{Binding Data}">
        <syncfusion:PivotGridControl.PivotRows>
            <engine:PivotItem FieldHeader="Product" FieldMappingName="Product" TotalHeader="Total" />
            <engine:PivotItem FieldHeader="Date" FieldMappingName="Date" TotalHeader="Total" />
        </syncfusion:PivotGridControl.PivotRows>
        <syncfusion:PivotGridControl.PivotColumns>
            <engine:PivotItem FieldHeader="Country" FieldMappingName="Country" TotalHeader="Total" />
            <engine:PivotItem FieldHeader="State" FieldMappingName="State" TotalHeader="Total" />
        </syncfusion:PivotGridControl.PivotColumns>
        <syncfusion:PivotGridControl.PivotCalculations>
            <engine:PivotComputationInfo CalculationName="Total" FieldName="Amount" Format="C" />
            <engine:PivotComputationInfo CalculationName="Total" FieldName="Quantity" SummaryType="Count" />
        </syncfusion:PivotGridControl.PivotCalculations>
    </syncfusion:PivotGridControl>
</syncfusion:SfBusyIndicator>
```

Whenever a calculation process occurs, you can enable **SfBusyIndicator.IsBusy** in the **PivotGrid.DataRefreshing** event. This is necessary because the DataRefreshing event is triggered when the calculation process starts, allowing the BusyIndicator to be displayed for the entire duration of the calculation. Once all background operations of the PivotGrid are fully completed, you should set **BusyIndicator.IsBusy** to **false**. This approach ensures that work is performed in the background without blocking the UI. It uses a ThreadPool thread to prevent the application from freezing and then uses the Dispatcher to switch back to the UI thread to update the BusyIndicator state.

**Code snippet for enabling and disabling SfBusyIndicator:**

```csharp
// Subscribe to the DataRefreshing event of the pivot grid
pivotGrid.DataRefreshing += PivotGrid_DataRefreshing;

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
```

**Note:** You can set the animation for the SfBusyIndicator by using the **AnimationType** property.

![Display Custom Busy Indicator](Display%20Custom%20Busy%20Indicator.gif)

Take a moment to peruse the [WPF PivotGridControl](https://help.syncfusion.com/wpf/pivot-grid/pivotgrid-getting-started) and [AnimationTypes in WPF SfBusyIndicator](https://help.syncfusion.com/wpf/busy-indicator/animationtypes) documentation, to learn more about pivotgrid and sfbusyindicator with examples.