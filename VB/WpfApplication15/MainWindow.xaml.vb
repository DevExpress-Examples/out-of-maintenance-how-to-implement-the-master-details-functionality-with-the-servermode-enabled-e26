Imports System
Imports System.Linq
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Data
Imports System.Data.Linq
Imports System.Windows.Markup
Imports DevExpress.Xpf.Grid
Imports DevExpress.Data.Linq

Namespace WpfApplication15

    ''' <summary>
    ''' Interaction logic for MainWindow.xaml
    ''' </summary>
    Public Partial Class MainWindow
        Inherits Window

'#Region "ExpandedProperty"
        Public Shared ReadOnly ExpandedProperty As DependencyProperty

        Public Shared Sub SetExpanded(ByVal element As DependencyObject, ByVal value As Boolean)
            element.SetValue(ExpandedProperty, value)
        End Sub

        Public Shared Function GetExpanded(ByVal element As DependencyObject) As Boolean
            Return CBool(element.GetValue(ExpandedProperty))
        End Function

        Shared Sub New()
            ExpandedProperty = DependencyProperty.RegisterAttached("Expanded", GetType(Boolean), GetType(MainWindow), New PropertyMetadata(False))
        End Sub

'#End Region
        Public Sub New()
            Me.InitializeComponent()
        End Sub

        Public Function CreateData() As DataSet
            Dim mdt As DataTable = New DataTable("Company")
            mdt.Columns.Add(New DataColumn("Name", GetType(String)))
            mdt.Columns.Add(New DataColumn("ID", GetType(Integer)))
            mdt.Rows.Add("Ford", 4)
            mdt.Rows.Add("Nissan", 5)
            mdt.Rows.Add("Mazda", 6)
            Dim ddt As DataTable = New DataTable("Models")
            ddt.Columns.Add(New DataColumn("Name", GetType(String)))
            ddt.Columns.Add(New DataColumn("MaxSpeed", GetType(Integer)))
            ddt.Columns.Add(New DataColumn("CompanyName", GetType(String)))
            ddt.Rows.Add("FordFocus", 400, "Ford")
            ddt.Rows.Add("FordST", 400, "Ford")
            ddt.Rows.Add("Note", 1000, "Nissan")
            'ddt.Rows.Add("Mazda3", 1000, "Mazda");
            ds = New DataSet("CM")
            ds.Tables.Add(mdt)
            ds.Tables.Add(ddt)
            Dim dr As DataRelation = New DataRelation("CompanyModels", mdt.Columns("Name"), ddt.Columns("CompanyName"))
            ds.Relations.Add(dr)
            Return ds
        End Function

        Private ds As DataSet

        Private Sub Window_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim LinqSM As LinqServerModeSource = New LinqServerModeSource() With {.ElementType = GetType(ProductModel), .KeyExpression = "ProductModelID", .QueryableSource = New DataClasses1DataContext().ProductModels}
            Me.gridControl1.ItemsSource = LinqSM
        End Sub
    End Class

    Public Class MyConverter
        Inherits MarkupExtension
        Implements IMultiValueConverter

        Public Overrides Function ProvideValue(ByVal serviceProvider As IServiceProvider) As Object
            Return Me
        End Function

'#Region "IMultiValueConverter Members"
        Public Function Convert(ByVal values As Object(), ByVal targetType As Type, ByVal parameter As Object, ByVal culture As Globalization.CultureInfo) As Object Implements IMultiValueConverter.Convert
            Dim view As TableView = TryCast(values(0), TableView)
            Dim gridControl1 As GridControl = view.Grid
            Dim rowIndex As Integer = CInt(values(1))
            Dim drv As ProductModel = TryCast(gridControl1.GetRow(rowIndex), ProductModel)
            Dim dLinqSM As LinqServerModeSource = New LinqServerModeSource() With {.ElementType = GetType(Product), .KeyExpression = "ProductID"}
            Dim tp As Table(Of Product) = New DataClasses1DataContext().Products
            Dim myQuery = From prod In tp Where prod.ProductModelID = drv.ProductModelID Select prod
            dLinqSM.QueryableSource = myQuery
            Return dLinqSM
        End Function

        Public Function ConvertBack(ByVal value As Object, ByVal targetTypes As Type(), ByVal parameter As Object, ByVal culture As Globalization.CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
            Throw New NotImplementedException()
        End Function
'#End Region
    End Class

    Public Class MyConverterExpanderState
        Inherits MarkupExtension
        Implements IMultiValueConverter

        Public Overrides Function ProvideValue(ByVal serviceProvider As IServiceProvider) As Object
            Return Me
        End Function

'#Region "IMultiValueConverter Members"
        Public Function Convert(ByVal values As Object(), ByVal targetType As Type, ByVal parameter As Object, ByVal culture As Globalization.CultureInfo) As Object Implements IMultiValueConverter.Convert
            Dim view As TableView = TryCast(values(0), TableView)
            Dim gridControl1 As GridControl = view.Grid
            Dim rowIndex As Integer = CInt(values(1))
            Dim drv As ProductModel = TryCast(gridControl1.GetRow(rowIndex), ProductModel)
            Dim dLinqSM As LinqServerModeSource = New LinqServerModeSource() With {.ElementType = GetType(Product), .KeyExpression = "ProductID"}
            Dim tp As Table(Of Product) = New DataClasses1DataContext().Products
            Dim myQuery = From prod In tp Where prod.ProductModelID = drv.ProductModelID Select prod
            dLinqSM.QueryableSource = myQuery
            If myQuery.Count() = 0 Then Return False
            Return True
        End Function

        Public Function ConvertBack(ByVal value As Object, ByVal targetTypes As Type(), ByVal parameter As Object, ByVal culture As Globalization.CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
            Throw New NotImplementedException()
        End Function
'#End Region
    End Class
End Namespace
