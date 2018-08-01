using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfMath.Exceptions;

namespace MathtodonViewer.Converter {
	public class Mathtodon {

		static Regex latex_begin_pat = new Regex(@"(\$\$|\$|\\begin\{(.*)\})", RegexOptions.Compiled | RegexOptions.Multiline);

		protected static Inline FormulaToInline(string formula) {
			var contain = new InlineUIContainer();
			var fobj = new WpfMath.Controls.FormulaControl();
			contain.Child = fobj;
			contain.BaselineAlignment = System.Windows.BaselineAlignment.Center;
			contain.Foreground = Brushes.White;
			fobj.Foreground = Brushes.White;
			fobj.Margin = new System.Windows.Thickness(4, 4, 2, 2);
			fobj.MinHeight = 4;
			fobj.MinWidth = 4;
			fobj.Formula = formula;
			return contain;
		}


		protected static Inline Image(string src) {
			var contain = new InlineUIContainer();
			var image_source = new BitmapImage(new Uri(src));
			var img = new Image();
			contain.Child = img;
			img.Source = image_source;
			return contain;
		}

		protected static Inline PlainTextTransform(string text, Toot context) {
			//			return new Run(text);
			var li = new Span();
			int i = 0;
			Match match;
			bool mathjaxed = false;
			try {
				while ((match = latex_begin_pat.Match(text, i)).Success) {
					mathjaxed = true;
					int plain_len = match.Index - i;
					li.Inlines.Add(new Run(text.Substring(i, plain_len)));
					i = match.Index + match.Length;
					if (match.Value == "$") {
						var j = text.IndexOf("$", i);
						var formula = text.Substring(i, j - i);
						i = j + 1;
						li.Inlines.Add(FormulaToInline(formula));

					} else if (match.Value == "$$") {
						var j = text.IndexOf("$$", i);
						var formula = text.Substring(i, j - i);
						i = j + 2;

						li.Inlines.Add(FormulaToInline(formula));

					} else {
						var envname = match.Groups[1];
						var endtag = @"\end{" + envname + "}";
						var j = text.IndexOf(endtag, i);
						var formula = text.Substring(i, j - i);
						i = j + endtag.Length;

						li.Inlines.Add(FormulaToInline(formula));
					}
				}
				li.Inlines.Add(new Run(text.Substring(i)));
			} catch { 
				var x = new Run(text);
				x.Foreground = Brushes.Orange;
				x.ToolTip = "Math parse error";
				return x;
			}
			if (mathjaxed == false) {
				return new Run(text);
			} else {
				context.Style = EnumToottype.mathjaxed;
				context.Changed("Style");
				return li;
			}
		}
		protected static Inline InlineRecursive(IElement el, Toot context) {
			var res = new Span();
			foreach (var node in el.ChildNodes) {
				if (node.NodeType == NodeType.Text) {
					res.Inlines.Add(PlainTextTransform(node.TextContent, context));
				} else if (node.NodeType == NodeType.Element) {
					var t = (IElement)node;
					var tag = t.TagName.ToLower();
					switch (tag) {
						case "a":
							var link = new Hyperlink(InlineRecursive(t, context));
							var href = t.GetAttribute("href");
							if ( href.Contains("/media/") ) { // image 
								link.IsEnabled = true;
								link.NavigateUri = new System.Uri(href);
								link.RequestNavigate += HyperLinkClick;
								res.Inlines.Add(Image(href));
							} else { 
								link.IsEnabled = true;
								link.NavigateUri = new System.Uri(href);
								link.RequestNavigate += HyperLinkClick;
								res.Inlines.Add(link);
							}
							break;
						case "br":
							res.Inlines.Add(new LineBreak());
							break;
						case "span":
							res.Inlines.Add(new Span(InlineRecursive(t, context)));
							break;
						default:
							var x = new Run( t.OuterHtml );
							x.Foreground = Brushes.Red;
							x.ToolTip = "Unknown Tag";
							res.Inlines.Add(x);
							break;
					}
				}
			}
			return res;
		}
		protected static void HyperLinkClick(object sender, System.Windows.Navigation.RequestNavigateEventArgs e) {
			//Debug.WriteLine( String.Format("link:{0}", e.Uri.AbsoluteUri) );
			System.Diagnostics.Process.Start(e.Uri.AbsoluteUri);
			e.Handled = true;
		}

		public static FlowDocument Process(string value , Toot context) {
			var res = new FlowDocument();
			var parser = new HtmlParser();
			var doc = parser.Parse(value).Body;
			foreach (var t in doc.Children) {
				if (t.TagName.ToLower() == "p") {
					res.Blocks.Add(new Paragraph( InlineRecursive(t, context) ) );
				} else {
					var x = new Run(t.OuterHtml);
					x.Foreground = Brushes.DarkRed;
					x.ToolTip = "Unknown Tag(Block Level)";
					res.Blocks.Add(new Paragraph(x));
				}
			}
			return res;
		}
	}
}
