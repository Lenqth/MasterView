using MathtodonViewer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MathtodonViewer {
	/// <summary>
	/// Login.xaml の相互作用ロジック
	/// </summary>
	public partial class DialogLogin : Window {
		public string instance { get { return instanceBox.Text; } set { instanceBox.Text = value; } }
		public string id { get { return idBox.Text; } set { idBox.Text = value; } }
		public string password { get { return passwordBox.Password; } set { passwordBox.Password = value; } }
		public bool? save { get { return SaveCheck.IsChecked; } set { SaveCheck.IsChecked = value; } }
		public static DialogLogin Dialog() {
			var win = new DialogLogin();
			bool? res = win.ShowDialog();
			return res == true ? win : null;
		}

		public DialogLogin() {
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e) {
			if (SaveCheck.IsChecked == true) {
				var setting = SettingManager.getInstanceOrDefault(instance);
				setting.Id = id;
				SettingManager.Save();
			}
			this.DialogResult = true;
			this.Close();
		}

		private void passwordBox_KeyDown(object sender, KeyEventArgs e) {
			if( e.Key == Key.Enter){
				// by http://blog.xin9le.net/entry/2013/10/27/195614
				var peer = (IInvokeProvider) new ButtonAutomationPeer(LoginButton);
				peer.Invoke();
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			instanceBox.ItemsSource = SettingManager.getInstanceList();
		}

		private void instanceBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			var st = SettingManager.getInstance(e.AddedItems[0].ToString());
			this.id = st.Id;
			this.passwordBox.Focus();
		}
	}
}
