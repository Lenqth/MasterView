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
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window {

		Client client;

		public MainWindow() {
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			//			var o = new Tootbox();
			//			o.Mathjaxed = true ;
			//			o.UserName = "Lenqth";
			//			MainColumn.Children.Add(o);
			client = new Client();
		}

		private async void Menu_Login_Click(object sender, RoutedEventArgs e) {
			await client.Initialize();

			column1.listener = new UserListener();
			column2.listener = new LocalListener();
			column3.listener = new UnionListener();

			var t1 = column1.listener.LoadTimelineAndStart(client);
			var t2 = column2.listener.LoadTimelineAndStart(client);
			var t3 = column3.listener.LoadTimelineAndStart(client);
			Task.WaitAll(new Task[] { t1, t2, t3 });
		}



		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			client.Close();
		}

		private void Menu_About_Click(object sender, RoutedEventArgs e) {

		}

		private void Menu_Test_Click(object sender, RoutedEventArgs e) {
			var s = new Sandbox();
			s.ShowDialog();
		}
	}


}
