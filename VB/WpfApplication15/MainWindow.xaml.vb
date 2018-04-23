Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows.Navigation
Imports System.Windows.Shapes
Imports System.Data
Imports System.Data.Linq
Imports System.Windows.Markup
Imports DevExpress.Xpf.Grid
Imports DevExpress.Data.Linq

Namespace WpfApplication15
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window


		#Region "ExpandedProperty"
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
		#End Region

		Public Sub New()
			InitializeComponent()
		End Sub

		Public Function CreateData() As DataSet
			Dim mdt As New DataTable("Company")
			mdt.Columns.Add(New DataColumn("Name", GetType(String)))
			mdt.Columns.Add(New DataColumn("ID", GetType(Integer)))
			mdt.Rows.Add("Ford", 4)
			mdt.Rows.Add("Nissan", 5)
			mdt.Rows.Add("Mazda", 6)

			Dim ddt As New DataTable("Models")
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
			Dim dr As New DataRelation("CompanyModels", mdt.Columns("Name"), ddt.Columns("CompanyName"))
			ds.Relations.Add(dr)

			Return ds
		End Function

		Private ds As DataSet
		Private Sub Window_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Dim LinqSM As New LinqServerModeSource() With {.ElementType = GetType(WpfApplication15.ProductModel), .KeyExpression = "ProductModelID", .QueryableSource = New DataClasses1DataContext().ProductModels}
			gridControl1.DataSource = LinqSM
		   ' gridControl1.DataSource = ds.Tables["Company"];

		End Sub
	End Class

	Public Class MyConverter
		Inherits MarkupExtension
		Implements IMultiValueConverter
		Public Overrides Function ProvideValue(ByVal serviceProvider As IServiceProvider) As Object
			Return Me
		End Function

		#Region "IMultiValueConverter Members"

		Public Function Convert(ByVal values() As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IMultiValueConverter.Convert
			Dim view As TableView = TryCast(values(0), TableView)
			Dim gridControl1 As GridControl = view.Grid
			Dim rowIndex As Integer = CInt(Fix(values(1)))
			Dim drv As ProductModel = TryCast(gridControl1.GetRow(rowIndex), ProductModel)
			Dim dLinqSM As New LinqServerModeSource() With {.ElementType = GetType(WpfApplication15.Product), .KeyExpression = "ProductID"}
			Dim tp As Table(Of Product) = New DataClasses1DataContext().Products
			Dim myQuery = _
				From prod In tp _
				Where prod.ProductModelID.Equals(drv.ProductModelID) _
				Select prod
			dLinqSM.QueryableSource = myQuery
			Return dLinqSM

		End Function

		Public Function ConvertBack(ByVal value As Object, ByVal targetTypes() As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
			Throw New NotImplementedException()
		End Function

		#End Region
	End Class

	Public Class MyConverterExpanderState
		Inherits MarkupExtension
		Implements IMultiValueConverter
		Public Overrides Function ProvideValue(ByVal serviceProvider As IServiceProvider) As Object
			Return Me
		End Function

		#Region "IMultiValueConverter Members"

		Public Function Convert(ByVal values() As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IMultiValueConverter.Convert

			Dim view As TableView = TryCast(values(0), TableView)
			Dim gridControl1 As GridControl = view.Grid
			Dim rowIndex As Integer = CInt(Fix(values(1)))
			Dim drv As ProductModel = TryCast(gridControl1.GetRow(rowIndex), ProductModel)
			Dim dLinqSM As New LinqServerModeSource() With {.ElementType = GetType(WpfApplication15.Product), .KeyExpression = "ProductID"}
			Dim tp As Table(Of Product) = New DataClasses1DataContext().Products
			Dim myQuery = _
				From prod In tp _
				Where prod.ProductModelID.Equals(drv.ProductModelID) _
				Select prod
			dLinqSM.QueryableSource = myQuery
			If myQuery.Count()= 0 Then
				Return False
			End If
			Return True
		End Function

		Public Function ConvertBack(ByVal value As Object, ByVal targetTypes() As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
			Throw New NotImplementedException()
		End Function

		#End Region
	End Class


End Namespace
