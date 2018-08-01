using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MathtodonViewer{
	class BlackMagics {
		public static object GetField(object obj, string name) {
			const BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
										   | BindingFlags.Static;
			var field = obj.GetType().GetField(name, bindFlags);
			return field?.GetValue(obj);
		}
		public static object GetProp(object obj, string name) {
			var field = obj.GetType().GetProperty(name);
			return field?.GetValue(obj, null);
		}
		public static object GetField<T>(object obj, string name) {
			const BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
										   | BindingFlags.Static;
			var field = obj.GetType().GetField(name, bindFlags);
			return (T)(field?.GetValue(obj));
		}
		public static object GetProp<T>(object obj, string name) {
			var field = obj.GetType().GetProperty(name);
			return (T)field?.GetValue(obj, null);
		}
		public static T Constructor<T>(params object[] args) {
			const BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
										   | BindingFlags.Static;
			var types = args.Select(x => x.GetType()).ToArray();
			var ctor = typeof(T).GetConstructor(bindFlags,null,types,null);
			return (T)ctor.Invoke(args);
		}
	}
}
