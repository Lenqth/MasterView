using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace MathtodonViewer {
	/// <summary>
	/// Column.xaml の相互作用ロジック
	/// </summary>
	public partial class Column : UserControl {
		protected ColumnListener _listener;
		public ColumnListener listener {
			get { return _listener; }
			set {
				_listener = value;
				this.DataContext = _listener;
			}
		}


		public Column() {
			InitializeComponent();
		}


	}
	internal class ConverterModeToHeadercolor : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if ((bool)value) {
				return "#FFBBFFAA";
			} else {
				return "#FFFFBBAA";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
}
