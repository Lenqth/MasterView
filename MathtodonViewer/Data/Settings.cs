using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace MathtodonViewer.Data {
	[Serializable]
	public class Settings {
		public Dictionary<string, InstanceSettings> InstanceSettings=new Dictionary<string, InstanceSettings>();
	}
}
