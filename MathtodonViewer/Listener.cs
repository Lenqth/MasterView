using Mastonet;
using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathtodonViewer{
	public abstract class ColumnListener : INotifyPropertyChanged {

		protected LinkedList<Toot> _Contents = new LinkedList<Toot>();
		public ICollection<Toot> Contents { get { return new List<Toot>(_Contents).AsReadOnly(); } }

		public bool StreamState { get; set; }
		public string ColumnTitle { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;
		public void Changed(string propname) {
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propname));
			}
		}
		// public abstract void GetTimeline(Client client);
		public abstract Task GetTimelineAsync(Client client);
		public abstract void StreamStart(Client client);

		public virtual void UpdateListener(object sender, StreamUpdateEventArgs e) {
			var status = e.Status;
			_Contents.AddFirst(Toot.fromMastonetStatus(status));
			Changed("Contents");
		}

		public Task LoadTimelineAndStart(Client client){
			return Task.Run(async () => {
				await GetTimelineAsync(client);
				StreamStart(client);
			});
		}

    }


	public class BlankListener : ColumnListener {

		TimelineStreaming stream;

		public BlankListener() {
			ColumnTitle = "デバッグ用空カラム";
			Changed("ColumnTitle");
		}

		public override async Task GetTimelineAsync(Client client) {
			Changed("Contents");
		}


		public override void StreamStart(Client client) {
		}
	}
	public class LocalListener : ColumnListener {

		TimelineStreaming stream;

		public LocalListener() {
			ColumnTitle = "ローカル";
			Changed("ColumnTitle");
		}

		public override async Task GetTimelineAsync(Client client) {
			var res = await client.GetLocalTimeLine();
			foreach (var item in res.Reverse<Status>()) {
				_Contents.AddFirst(Toot.fromMastonetStatus(item));
			}
			Changed("Contents");
		}


		public override void StreamStart(Client client) {
			Changed("StreamState");
			stream = client.GetStream("public:local");
			stream.OnUpdate += this.UpdateListener;
			StreamState = true;
			Changed("StreamState");
			stream.Start();
		}
	}
	public class UnionListener : ColumnListener {

		TimelineStreaming stream;

		public UnionListener() {
			ColumnTitle = "連合";
			Changed("ColumnTitle");
		}


		public override async Task GetTimelineAsync(Client client) {
			var res = await client.GetPublicTimeLine();
			foreach (var item in res.Reverse<Status>()) {
				_Contents.AddFirst(Toot.fromMastonetStatus(item));
			}
			Changed("Contents");
		}

		public override void StreamStart(Client client) {
			Changed("StreamState");
			stream = client.GetPublicStream(); // client.GetStream("public");
			stream.OnUpdate += this.UpdateListener;
			stream.Start();
			StreamState = true;
			Changed("StreamState");
		}
	}

	public class UserListener : ColumnListener {

		TimelineStreaming stream;

		public UserListener() {
			ColumnTitle = "ホーム";
			Changed("ColumnTitle");
		}

		public override async Task GetTimelineAsync(Client client) {
			var res = await client.GetHomeTimeLine();
			foreach (var item in res.Reverse<Status>()) {
				_Contents.AddFirst(Toot.fromMastonetStatus(item));
			}
			Changed("Contents");
		}

		public override void StreamStart(Client client) {
			Changed("StreamState");
			stream = client.GetStream("user"); // client.GetStream("public");
			stream.OnUpdate += this.UpdateListener;
			stream.Start();
			StreamState = true;
			Changed("StreamState");
		}
	}

}
