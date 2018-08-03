using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MathtodonViewer.Data {
	public class SettingManager {
		private static Settings settings;
		public static string configFile = "config.xml";
		private static InstanceSettings addInstance(string instance) {
			var res = new InstanceSettings();
			settings.InstanceSettings.Add(instance, res);
			return res;
		}
		public static InstanceSettings getInstanceOrDefault(string instance) {
			if (settings.InstanceSettings.ContainsKey(instance)) {
				return settings.InstanceSettings[instance];
			} else {
				return addInstance(instance);
			}
		}
		public static InstanceSettings getInstance(string instance) {
			if (settings.InstanceSettings.ContainsKey(instance)) {
				return settings.InstanceSettings[instance];
			} else {
				return null;
			}
		}
		public static string[] getInstanceList() {
			return settings.InstanceSettings.Keys.ToArray();
		}

		public static void Save() {
			var s = new Newtonsoft.Json.JsonSerializer();
			using (var f = new StreamWriter(configFile, false, Encoding.UTF8)) {
				using (var jw = new Newtonsoft.Json.JsonTextWriter(f)) {
					s.Serialize(jw,settings);
				}
			}
		}
		public static void Load() {
			if (!System.IO.File.Exists(configFile)) {
				settings = new Settings();
				return;
			}
			var s = new Newtonsoft.Json.JsonSerializer();
			using (var f = new StreamReader(configFile, Encoding.UTF8)) {;
				using (var jr = new Newtonsoft.Json.JsonTextReader(f)) {
					settings = s.Deserialize<Settings>(jr);
				}
			}
		}
	}
}
