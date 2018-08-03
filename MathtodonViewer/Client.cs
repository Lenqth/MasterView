using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Mastonet;
using System.Diagnostics;

namespace MathtodonViewer{
	public enum EnumClientState {
		notlogined,
		logined,
		started
	}

	public class Client {
		private MastodonClient client;

		private AuthenticationClient auth_client;
		private Mastonet.Entities.AppRegistration app;


		public string instance { get; set; }

		public async Task<Mastonet.Entities.AppRegistration> CreateAppAsync(string target_instance) {
			throw new NotImplementedException();
			//return await auth_client.CreateApp("Mathter_Viewer_AUTOCREATE_TEST", Scope.Read | Scope.Write | Scope.Follow);
		}

		public async Task LoadAppAsync() {
			Mastonet.Entities.AppRegistration appreg = null;
			string filename = String.Format("{0}.json", instance);
			if (File.Exists(filename) ) {
				using ( var f = new StreamReader(filename) ) {
					var json = f.ReadToEnd();
					appreg = JsonConvert.DeserializeObject<Mastonet.Entities.AppRegistration>(json);
				}
				appreg.Instance = instance;
			}
			if (appreg == null) {
				// await CreateAppAsync(instance);
				//ではなく、とりあえず自分で作ったトークンを使う。
				var o = new Mastonet.Entities.AppRegistration();
				o.Instance = instance;
				o.Scope = Scope.Read | Scope.Write | Scope.Follow;
				o.ClientId = "76772cbabef0e50a900fc34f6b44525edea369de8580294bb0bb01b6f684192b";
				o.ClientSecret = "8cd0382533a5b345daa25fa63f0feab1af13dc5a8bc3ff115be9e0958bc8e3c5";
				appreg = o;
				string json = JsonConvert.SerializeObject(appreg);
				using (var f = new StreamWriter(filename)) {
					f.Write(json);
				}
			}
			if (appreg == null) {
				throw new System.Security.Authentication.AuthenticationException();
			}
			app = appreg;
			auth_client.AppRegistration = appreg;
		}
		protected List<TimelineStreaming> streams;
		public void Close() {
			foreach (var s in streams) {
				s.Stop();
			}
			streams.Clear();
		}

		public async Task<bool> Login() {
			Mastonet.Entities.Auth auth;
			if (auth_client?.AuthToken == null) {
				var d = DialogLogin.Dialog();
				if (d?.DialogResult != true ) {
					return false;
				}
				var instance = d.instance;
				var id = d.id;
				var pw = d.password;

				this.instance = instance;
				auth_client = new AuthenticationClient(instance);
				await LoadAppAsync();

				auth = await auth_client.ConnectWithPassword(id,pw);
			} else {
				auth = auth_client.AuthToken;
			}
			if (app != null && auth != null) {
				client = new MastodonClient(app, auth);
			} else {
				throw new System.Security.SecurityException("Login Failed");
			}
			return true;
		}

		public async Task<bool> Initialize() {
			try {
				if ( !(await Login()) ) {
					return false;
				}
			} catch ( Exception e ) {
				Debug.WriteLine("Error On Login:");
				Debug.Indent();
				Debug.WriteLine(e.Message);
				Debug.WriteLine(e.StackTrace);
				Debug.Unindent();
				return false;
			}
			return true;
		}
		
		public Client() {
			streams = new List<TimelineStreaming>();
		}

		public TimelineStreaming GetStream(string type) {
			// string url = "https://" + client.StreamingApiUrl + "/api/v1/streaming/public";
			string url = "https://" + BlackMagics.GetProp<string>(client, "StreamingApiUrl") + "/api/v1/streaming/" + type;
			url = url.Replace("https:///", "https://");
//			Uri u = new Uri(url);
			// return new TimelineStreaming(url, client.AuthToken.AccessToken);
			var s = BlackMagics.Constructor<TimelineStreaming>(url, client.AuthToken.AccessToken);
			streams.Add(s);
			return s;
		}

		public TimelineStreaming GetPublicStream() {
			var s = client.GetPublicStreaming();
			streams.Add(s);
			return s;
		}
		public Task<Mastonet.Entities.MastodonList<Mastonet.Entities.Status>> GetPublicTimeLine() {
			return client.GetPublicTimeline();
		}
		public Task<Mastonet.Entities.MastodonList<Mastonet.Entities.Status>> GetLocalTimeLine() {
			return client.GetPublicTimeline(local: true);
		}
		public Task<Mastonet.Entities.MastodonList<Mastonet.Entities.Status>> GetHomeTimeLine() {
			return client.GetHomeTimeline();
		}
	}
}
