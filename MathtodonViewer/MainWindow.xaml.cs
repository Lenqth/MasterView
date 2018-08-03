using MathtodonViewer.Data;
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
		List<ColumnListener> listeners = new List<ColumnListener>();
		DependencyProperty listenersProperty = DependencyProperty.Register(
			"listeners", typeof(List<ColumnListener>), typeof(MainWindow));


		public MainWindow() {
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			//			var o = new Tootbox();
			//			o.Mathjaxed = true ;
			//			o.UserName = "Lenqth";
			//			MainColumn.Children.Add(o);
			SettingManager.Load();
			client = new Client();
		}

		private async void Menu_Login_Click(object sender, RoutedEventArgs e) {
			if (!await client.Initialize()) return;

			var column1 = new UserListener();
			var column2 = new LocalListener();
			var column3 = new UnionListener();

			listeners.Add(column1);
			listeners.Add(column2);
			listeners.Add(column3);

			SetValue(listenersProperty,listeners);

			var t1 = column1.LoadTimelineAndStart(client);
			var t2 = column2.LoadTimelineAndStart(client);
			var t3 = column3.LoadTimelineAndStart(client);
			Task.WaitAll(new Task[] { t1, t2, t3 });
			
		}


		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			client.Close();
			SettingManager.Save();
		}

		private void Menu_About_Click(object sender, RoutedEventArgs e) {

		}

		private void Menu_Test_Click(object sender, RoutedEventArgs e) {
			var s = new Sandbox();
			s.ShowDialog();
		}
		private void Menu_Test2_Click(object sender, RoutedEventArgs e) {
			var column1 = new BlankListener();
			listeners.Add(column1);
			var column2 = new BlankListener();
			listeners.Add(column2);
			SetValue(listenersProperty, listeners);
		}
	}


}
