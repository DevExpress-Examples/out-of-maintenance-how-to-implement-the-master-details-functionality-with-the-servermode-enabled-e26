using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.Linq;
using System.Windows.Markup;
using DevExpress.Xpf.Grid;
using DevExpress.Data.Linq;

namespace WpfApplication15
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
  

        #region ExpandedProperty
        public static readonly DependencyProperty ExpandedProperty;

        public static void SetExpanded(DependencyObject element, bool value) {
            element.SetValue(ExpandedProperty, value);
        }

        public static bool GetExpanded(DependencyObject element) {
            return (bool)element.GetValue(ExpandedProperty);
        }

        static MainWindow()
        {
            ExpandedProperty = DependencyProperty.RegisterAttached("Expanded", typeof(bool),
                typeof(MainWindow), new PropertyMetadata(false));
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        public DataSet CreateData()
        {
            DataTable mdt = new DataTable("Company");
            mdt.Columns.Add(new DataColumn("Name", typeof(string)));
            mdt.Columns.Add(new DataColumn("ID", typeof(int)));
            mdt.Rows.Add("Ford", 4);
            mdt.Rows.Add("Nissan", 5);
            mdt.Rows.Add("Mazda", 6);

            DataTable ddt = new DataTable("Models");
            ddt.Columns.Add(new DataColumn("Name", typeof(string)));
            ddt.Columns.Add(new DataColumn("MaxSpeed", typeof(int)));
            ddt.Columns.Add(new DataColumn("CompanyName", typeof(string)));
            ddt.Rows.Add("FordFocus", 400, "Ford");
            ddt.Rows.Add("FordST", 400, "Ford");
            ddt.Rows.Add("Note", 1000, "Nissan");
            //ddt.Rows.Add("Mazda3", 1000, "Mazda");

            ds = new DataSet("CM");
            ds.Tables.Add(mdt);
            ds.Tables.Add(ddt);
            DataRelation dr = new DataRelation("CompanyModels", mdt.Columns["Name"], ddt.Columns["CompanyName"]);
            ds.Relations.Add(dr);

            return ds;
         }

        DataSet ds;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LinqServerModeSource LinqSM = new LinqServerModeSource()
            {
                ElementType = typeof(WpfApplication15.ProductModel),
                KeyExpression = "ProductModelID",
                QueryableSource = new DataClasses1DataContext().ProductModels
            };
            gridControl1.DataSource = LinqSM;
            
        }

    }

    public class MyConverter : MarkupExtension, IMultiValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TableView view = values[0] as TableView;
            GridControl gridControl1 = view.Grid;
            int rowIndex = (int)values[1];
            ProductModel drv = gridControl1.GetRow(rowIndex) as ProductModel;
            LinqServerModeSource dLinqSM = new LinqServerModeSource()
            {
                ElementType = typeof(WpfApplication15.Product),
                KeyExpression = "ProductID",
            };
            Table<Product> tp = new DataClasses1DataContext().Products;
            var myQuery = 
                          from prod in tp
                          where prod.ProductModelID == drv.ProductModelID
                          select prod;
            dLinqSM.QueryableSource = myQuery;
            return dLinqSM;
 
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class MyConverterExpanderState : MarkupExtension, IMultiValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            TableView view = values[0] as TableView;
            GridControl gridControl1 = view.Grid;
            int rowIndex = (int)values[1];
            ProductModel drv = gridControl1.GetRow(rowIndex) as ProductModel;
            LinqServerModeSource dLinqSM = new LinqServerModeSource()
            {
                ElementType = typeof(WpfApplication15.Product),
                KeyExpression = "ProductID",
            };
            Table<Product> tp = new DataClasses1DataContext().Products;
            var myQuery =
                          from prod in tp
                          where prod.ProductModelID == drv.ProductModelID
                          select prod;
            dLinqSM.QueryableSource = myQuery;
            if (myQuery.Count()== 0) return false;
            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


}
