using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography;
using System.IO;

namespace MathtodonViewer.Data {
	//[Serializable]
	//public class ByteArray {
	//	public byte[] arr { get; set; }
	//	public static explicit operator ByteArray(byte[] data) {
	//		return new ByteArray(data);
	//	}
	//	public static implicit operator byte[](ByteArray ba) {
	//		return ba.arr;
	//	}
	//	public ByteArray(byte[] data) {
	//		arr = data;
	//	}
	//	public ByteArray() {

	//	}
	//}

	public class InstanceSettings {
		string Version = "";
		string Id = null;
		string Password = null;
		string InitialVector = null;

		public void SetEncodedPassword(string password, string master) {
			var aes = new AesManaged();
			aes.KeySize = 256;
			aes.BlockSize = 128;
			aes.Mode = CipherMode.CBC;
			aes.GenerateIV();
			aes.Key = Encoding.UTF8.GetBytes(master);
			byte[] password_binary = Encoding.UTF8.GetBytes(password);

			this.InitialVector = Convert.ToBase64String(aes.IV);
			using (var ms = new MemoryStream())
			using (var encryptor = aes.CreateEncryptor())
			using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
			using (var writer = new StreamWriter(cs)) {
				writer.Write(password);
				this.Password = Convert.ToBase64String(ms.ToArray());
			}
		}
		public string GetEncodedPassword(string master) {
			var aes = new AesManaged();
			aes.KeySize = 256;
			aes.BlockSize = 128;
			aes.Mode = CipherMode.CBC;
			aes.IV = Convert.FromBase64String(this.InitialVector);
			aes.Key = Encoding.UTF8.GetBytes(master);
			using (var ms = new MemoryStream(Convert.FromBase64String(this.Password)) )
			using (var decryptor = aes.CreateDecryptor())
			using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
			using (var reader = new StreamReader(cs)) {
				return reader.ReadToEnd();
			}
		}

	}
}
