using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

using AngleSharp.Parser.Html;

namespace MathtodonViewer {
	/// <summary>
	/// Toot.xaml の相互作用ロジック
	/// </summary>
	public partial class Tootbox : UserControl, System.ComponentModel.INotifyPropertyChanged  {
		public Tootbox() {
			InitializeComponent();
		}
		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string name) {
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) {
				handler(this, new PropertyChangedEventArgs(name));
			}
		}

		private void ContextMenu_Debug_Click(object sender, RoutedEventArgs e) {
			Toot t = (Toot) this.DataContext;
			System.Windows.Clipboard.SetText(t.Content,TextDataFormat.UnicodeText);
		}



	}





	internal class ConverterStyleToBGcolor : IValueConverter {
		public object Convert(object _value, Type targetType, object parameter, CultureInfo culture) {
			EnumToottype value = (EnumToottype)_value;
			if (value == EnumToottype.mathjaxed) {
				return "#ff1e3634";
			} else {
				return "#fff0f0f0";
			}
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
	internal class ConverterStyleToBorderColor : IValueConverter {
		public object Convert(object _value, Type targetType, object parameter, CultureInfo culture) {
			EnumToottype value = (EnumToottype)_value;
			if (value == EnumToottype.mathjaxed) {
				return "#FF423325";
			} else {
				return "#FF423325";
			}
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
	internal class ConverterStyleToBorderThick : IValueConverter {
		public object Convert(object _value, Type targetType, object parameter, CultureInfo culture) {
			EnumToottype value = (EnumToottype)_value;
			if (value == EnumToottype.mathjaxed) {
				return 4;
			} else {
				return 1;
			}
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}

	internal class ConverterStyleToForeground : IValueConverter {
		public object Convert(object _value, Type targetType, object parameter, CultureInfo culture) {
			EnumToottype value = (EnumToottype)_value;
			if (value == EnumToottype.mathjaxed) {
				return "#FFFFFFFF";
			} else {
				return "#FF000000";
			}
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
	

	internal class ConverterContentsToInline : IValueConverter {
		public object Convert(object _value, Type targetType, object parameter, CultureInfo culture) {
			if (_value == null || !(_value is Toot) ) {
				return DefaultContent;
			}
			Toot toot = (Toot)_value;
			string value = toot.Content;
			return Converter.Mathtodon.Process(value , toot);
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}

		public static FlowDocument DefaultContent {
			get {
				var res = new FlowDocument();
				res.Blocks.Add(new Paragraph(new Run("...	")));
				return res;
			}
		}
	}


}
