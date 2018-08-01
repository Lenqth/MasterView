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
using System.Windows.Shapes;

namespace MathtodonViewer.WPF {
	/// <summary>
	/// SimpleTextBoxDialog.xaml
	/// </summary>
	public partial class SimpleTextBoxDialog : Window {
		public SimpleTextBoxDialog() {
			InitializeComponent();
		}
		public SimpleTextBoxDialog(string textboxContent) {
			InitializeComponent();
			Content.Text = textboxContent;
		}

		private void Button_Click(object sender, RoutedEventArgs e) {
			this.Close();
		}
	}
}
