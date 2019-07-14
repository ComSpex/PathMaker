// $Header: /ComSpexHome.root/ComSpexHome/Visual Studio 2010/Projects/ComSpexHome/Licenser/LicenseGenerator.cs 1     11/07/13 7:23p Yosuke $
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace ComSpex.Tools {
	[System.Runtime.InteropServices.ComVisible(false)]
	public class LicenseGenerator:IDisposable {
		const string ComSpexKeyContainer="ComSpex Key Container";
		protected string public_KeyUsed;
		protected string privateKeyUsed;
		protected string keyUsedGiven;
		public bool UseMSCompressExpand=false;
		public LicenseGenerator() {
		}
		public LicenseGenerator(string key)
			:this(){
			this.keyUsedGiven=key;
		}
		protected AsymmetricAlgorithm algo{
			get{
				CspParameters cspp=new CspParameters(1,null,ComSpexKeyContainer);
				cspp.Flags=CspProviderFlags.UseMachineKeyStore;
				RSACryptoServiceProvider rsa=new RSACryptoServiceProvider(cspp);
				rsa.PersistKeyInCsp=true;
				public_KeyUsed=rsa.ToXmlString(false);
				privateKeyUsed=rsa.ToXmlString(true);
				return rsa;
			}
		}
		public string SaveNewLicenseKey(bool isTrial,double days) {
			return SaveNewLicenseKey(isTrial,days,null);
		}
		public string SaveNewLicenseKey(bool isTrial,double days,string content) {
			string key=CreateLicenseKey(isTrial,days,content);
			string path=String.Format("ComSpex.Tools.{0}License.txt",isTrial?"Trial.":String.IsNullOrEmpty(content)?"":String.Format("{0}.",content));
			using(StreamWriter sw=new StreamWriter(path)) {
				sw.Write(key);
			}
			return key;
		}
		public string ParseLicenseKey(string key64) {
			string text=DecryptSessionKeyByRSA(key64);
			if(UseMSCompressExpand) {
				byte[] compressed=Convert.FromBase64String(text);
				text=MSExpand(compressed);
			}
			return text;
		}
		string CreateLicenseKey(bool isTrial,double days,string content) {
			string seed=String.Empty;
			if(isTrial) {
				seed=DateTime.Today.Add(TimeSpan.FromDays(days)).ToLongDateString();
			} else {
				if(String.IsNullOrEmpty(content)) {
					seed=String.Format("{0} {1}",this.GetType().FullName,Environment.MachineName);
				} else {
					seed=String.Format("{0} {1}",this.GetType().FullName,content);
				}
			}
			string text=EncryptSessionKeyByRSA(MSCompress(seed));
			return text;
		}
		string MSCompress(string text) {
			if(!UseMSCompressExpand) {
				return text;
			}
			if(text.Length<16) {
				throw new ArgumentException("Input text length must be more than 16.");
			}
			return MSCompress(Encoding.Default.GetBytes(text));
		}
		string MSExpand(string text) {
			if(!UseMSCompressExpand) {
				return text;
			}
			return MSExpand(Convert.FromBase64String(text));
		}
		string MSCompress(byte[] data) {
			string text=String.Empty;
			using(MemoryStream sour=new MemoryStream(data)) {
				using(MemoryStream dest=new MemoryStream()) {
					using(ITypicaloo Ms=new MicrosoftCompress()) {
						Ms.Run(sour,dest);
					}
					text=Convert.ToBase64String(dest.ToArray());
				}
			}
			return text;
		}
		string MSExpand(byte[] data) {
			if(!UseMSCompressExpand) {
				return Encoding.Default.GetString(data);
			}
			string text=String.Empty;
			using(MemoryStream sour=new MemoryStream(data)) {
				using(MemoryStream dest=new MemoryStream()) {
					using(ITypicaloo Ms=new MicrosoftExpand()) {
						Ms.Run(sour,dest);
					}
					text=Encoding.Default.GetString(dest.ToArray());
				}
			}
			return text;
		}
		public string Public_KeyUsed{
			get{
				if(String.IsNullOrEmpty(public_KeyUsed)){
					AsymmetricAlgorithm rsa=algo;
				}
				return public_KeyUsed;
			}
		}
		public string EncryptSessionKeyByRSA(string target) {
			return EncryptSessionKeyByRSA(target,null);
		}
		public string EncryptSessionKeyByRSA(string target,string keyToUse) {
			byte[] sessionKey=Encoding.Default.GetBytes(target);
			byte[] buff=EncryptSessionKeyByRSA(sessionKey,keyToUse);
			string text64=Convert.ToBase64String(buff);
			return text64;
		}
		public byte[] EncryptSessionKeyByRSA(byte[] sessionKey,string keyToUse) {
			AsymmetricAlgorithm rsa=algo;
			if(!String.IsNullOrEmpty(keyToUse)) {
				rsa.FromXmlString(keyToUse);
			}
			RSAOAEPKeyExchangeFormatter forma=new RSAOAEPKeyExchangeFormatter();
			forma.SetKey(rsa);
			byte[] exchangeData=forma.CreateKeyExchange(sessionKey);
			return exchangeData;
		}
		public string DecryptSessionKeyByRSA(string text64) {
			byte[] buff=Convert.FromBase64String(text64);
			byte[] keySession=DecryptSessionKeyByRSA(buff);
			string text=Encoding.Default.GetString(keySession);
			return text;
		}
		public byte[] DecryptSessionKeyByRSA(byte[] exchangeData) {
			AsymmetricAlgorithm rsa=algo;
			RSAOAEPKeyExchangeDeformatter defor=new RSAOAEPKeyExchangeDeformatter();
			defor.SetKey(rsa);
			byte[] keySession=defor.DecryptKeyExchange(exchangeData);
			return keySession;
		}
		public void Dispose() {
		}
	}
}
