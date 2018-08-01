using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace MathtodonViewer {
	public class HyperRichTextBox : RichTextBox {
		#region 依存関係プロパティ
		public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register(
			"Document", typeof(FlowDocument), typeof(HyperRichTextBox), new UIPropertyMetadata(null, OnRichTextItemsChanged));
		#endregion  // 依存関係プロパティ

		#region 公開プロパティ
		public new FlowDocument Document {
			get { return (FlowDocument)GetValue(DocumentProperty); }
			set { SetValue(DocumentProperty, value); }
		}
		#endregion  // 公開プロパティ

		#region イベントハンドラ
		private static void OnRichTextItemsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
			var control = sender as RichTextBox;
			if (control != null) {
				control.Document = e.NewValue as FlowDocument;
			}
		}
		#endregion  // イベントハンドラ
	}
}
