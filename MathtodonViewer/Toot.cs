using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Mastonet;
using Mastonet.Entities;


namespace MathtodonViewer {


	public enum EnumToottype{
		normal,
		mathjaxed
	}

	public class Toot : INotifyPropertyChanged {

		public event PropertyChangedEventHandler PropertyChanged;
		public void Changed(string propname){
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propname));
			}
		}
		public Toot() {
			//_document = DefaultContent;
		}

		public static Toot fromMastonetStatus(Status status) {
			Toot o = new Toot();
			o._status = status;
			o._content = status.Content;
			o._date = status.CreatedAt;
			o._author = status.Account;
			//o.CompileFlowDocument();
			//Debug.WriteLine(o._content);
			return o;
		}

		protected Status _status;
		protected DateTime _date;
		protected Account _author;
		protected string _content;
//		protected FlowDocument _document;
		
		protected bool _Mathjaxed;

		public DateTime date {
			get { return _date; }
		}

		public Account author {
			get { return _author; }
		}

		public string Content {
			get { return _content; }
		}

//		public FlowDocument Document {
//			get { return _document; }
//		}

		public string UserName {
			get { return author.DisplayName; }
		}


		public string DateText {
			get {
				return _date.ToLocalTime().ToString();
			}
		}

		public EnumToottype Style { get; set; } = EnumToottype.normal;

/*		public void CompileFlowDocument() {
			if (this._document != null) {
				var res = Converter.Mathtodon.Process(Content, this);
				this._document = res;
				Changed("Document");
			}
		}
		*/

		public static List<Toot> GetMockToots() {
			int n = 3;
			List<Toot> res = new List<Toot>();
			for (int i = 0; i < n; i++) {
				Toot o = new Toot();
				o._content = "lorem ipsum";
				o._author = new Account();
				o._date = DateTime.Now;
				//o.CompileFlowDocument();
				res.Add(o);
			}
			return res;
		}
		public static FlowDocument DefaultContent{
			get {
				var res = new FlowDocument();
				res.Blocks.Add(new Paragraph(new Run("...	")));
				return res;
			}
		}

		public static List<Toot> Mocks {
			get {
				return GetMockToots();
			}
		}

	}
}
