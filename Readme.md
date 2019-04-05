<!-- default file list -->
*Files to look at*:

* [MainWindow.xaml](./CS/WpfApplication15/MainWindow.xaml) (VB: [MainWindow.xaml](./VB/WpfApplication15/MainWindow.xaml))
* [MainWindow.xaml.cs](./CS/WpfApplication15/MainWindow.xaml.cs) (VB: [MainWindow.xaml.vb](./VB/WpfApplication15/MainWindow.xaml.vb))
<!-- default file list end -->
# How to implement the Master-Details functionality with the ServerMode enabled


<p>One of known <a href="https://www.devexpress.com/Support/Center/Example/Details/E2622/how-to-implement-the-master-details-functionality-with-the-servermode-enabled">Server Mode limitations</a> is the inability to use such sources in the master and detail grids. In this example, we demonstrated how to avoid this limitation by displaying detailed information in a custom <a href="https://documentation.devexpress.com/WPF/DevExpress.Xpf.Grid.TableView.RowDetailsTemplate.property">RowDetailsTemplate</a> (in versions where this property does not exist, <a href="https://documentation.devexpress.com/WPF/DevExpress.Xpf.Grid.TableView.DataRowTemplate.property">DataRowTemplate</a> is used).<br><br>In this example, we used the AdventureWorks database, which can be downloaded at <a href="http://msftdbprodsamples.codeplex.com/releases/view/93587">AdventureWorks DB</a>, <a href="https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/linq/">LINQ to SQL</a> classes, and <a href="https://documentation.devexpress.com/CoreLibraries/DevExpress.Data.Linq.LinqServerModeSource.class">LinqServerModeSource</a> - a synchronous Server Mode source designed for LINQ to SQL.</p>

<br/>


