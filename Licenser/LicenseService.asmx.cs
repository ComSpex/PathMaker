// $Header: /ComSpexHome.root/ComSpexHome/Visual Studio 2010/Projects/ComSpexHome/Licenser/LicenseService.asmx.cs 1     11/07/13 7:23p Yosuke $
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Collections;
using System.Text;
using System.IO;

namespace Licenser {
	using ComSpex.Tools;
	/// <summary>
	/// Summary description for Service1
	/// </summary>
	[WebService(Namespace="http://comspex.com/lix/")]
	public partial class LicenseService:WebService {
		static Dictionary<string,ActivationSet> actCache;
		static string filename=HttpContext.Current.Server.MapPath("actCache.xml");
		public LicenseService() {
			lock(actCache=new Dictionary<string,ActivationSet>()){
				LoadData(filename);
			}
		}
		public static void LoadData(string path){
			if(File.Exists(path)) {
				FileInfo inf=new FileInfo(path);
				HttpContext.Current.Trace.Write(inf.FullName);
				using(StreamReader sr=new StreamReader(path)) {
					using(ComSpex.XmlPersist<ActivationSet[]> Xp=new ComSpex.XmlPersist<ActivationSet[]>()) {
						ActivationSet[] items=Xp.Load(sr);
						foreach(ActivationSet item in items){
							if(!actCache.ContainsKey(item.MachineName.ToUpper())){
								actCache.Add(item.MachineName.ToUpper(),item);
							}
						}
					}
				}
			}
		}
		public static void SaveData(string path){
			lock(actCache){
				ActivationSet[] items=new ActivationSet[actCache.Count];
				actCache.Values.CopyTo(items,0);
				using(StreamWriter sw=new StreamWriter(path)){
					using(ComSpex.XmlPersist<ActivationSet[]> Xp=new ComSpex.XmlPersist<ActivationSet[]>(items)){
						Xp.Save(sw);
					}
				}
			}
		}
		void Save(){
			SaveData(filename);
		}
		string DecryptPassword(string password) {
			byte[] buff=Convert.FromBase64String(password);
			string text=Encoding.ASCII.GetString(buff);
			return text;
		}
		[WebMethod]
		public void GenerateTestData(){
			lock(actCache){
				actCache.Clear();
				for(int i=0;i<10;++i){
					ActivationSet item=new ActivationSet(String.Format("Machine_{0:00}",i),typeof(ActivationSet).FullName);
					actCache.Add(item.MachineName.ToUpper(),item);
				}
			}
			Save();
		}
		[WebMethod]
		public string HelloWorld() {
			return "Hello World";
		}
		[WebMethod]
		public string AddActivationSet(string data){
			try{
				ActivationSet item=new ActivationSet(data);
				lock(actCache){
					if(!actCache.ContainsKey(item.MachineName.ToUpper())){
						actCache.Add(item.MachineName.ToUpper(),item);
					}else{
						ulong count=actCache[item.MachineName].Count;
						actCache[item.MachineName]=item;
						actCache[item.MachineName].Count=++count;
					}
				}
				Save();
			}catch(Exception ex){
				return ex.ToString();
			}
			return null;
		}
		[WebMethod]
		public string GetActivationSet(string key){
			lock(actCache){
				if(actCache.ContainsKey(key.ToUpper())){
					ActivationSet item=actCache[key.ToUpper()];
					return item.ToXmlString();
				}
			}
			return null;
		}
		[WebMethod]
		public void CleanActivationCache(string password){
			if("boss"!=DecryptPassword(password)) {
				return;
			}
			while(File.Exists(filename)) {
				File.Delete(filename);
			}
			lock(actCache){
				actCache.Clear();
			}
		}
		[WebMethod]
		public string GetBase64String(string text){
			byte[] buff=Encoding.ASCII.GetBytes(text);
			return Convert.ToBase64String(buff);
		}
		[WebMethod]
		public string DownloadActivationSets(string password) {
			if("boss"!=DecryptPassword(password)){
				return null;
			}
			ActivationSet[] items=new ActivationSet[actCache.Count];
			actCache.Values.CopyTo(items,0);
			return ActivationSet.FromArray(items);
		}
		[WebMethod]
		public void UploadActivationSets(string password,string data,bool isMerged){
			if("boss"!=DecryptPassword(password)){
				return;
			}
			ActivationSet[] items=ActivationSet.ToArray(data);
			if(!isMerged) {
				actCache.Clear();
			}
			foreach(ActivationSet item in items) {
				if(!actCache.ContainsKey(item.MachineName.ToUpper())) {
					actCache.Add(item.MachineName.ToUpper(),item);
				}
			}
			Save();
		}
		[WebMethod]
		public bool AlreadyRegistered(string key){
			return actCache.ContainsKey(key.ToUpper());
		}
		[WebMethod]
		public string EncryptSessionKeyByRSA(string text,string keyToUse){
			using(LicenseGenerator Lg=new LicenseGenerator()){
				return Lg.EncryptSessionKeyByRSA(text,keyToUse);
			}
		}
		[WebMethod]
		public string DecryptSessionKeyByRSA(string text64){
			using(LicenseGenerator Lg=new LicenseGenerator()){
				return Lg.DecryptSessionKeyByRSA(text64);
			}
		}
		[WebMethod]
		public string GetPublicKey(){
			using(LicenseGenerator Lg=new LicenseGenerator()) {
				return Lg.Public_KeyUsed;
			}
		}
		[WebMethod]
		public string ParseLicenseKey(string text64){
			//LicenseKey is:
			// During trial periods, it contains an expiry date.
			// After payment honored, it changes to GUID. So,...
			try{
				Guid guid=new Guid(text64);
				foreach(KeyValuePair<string,ActivationSet> item in actCache){
					if(guid==item.Value.Id){
						string xml=ActivationSet.ToXmlString(item.Value);
						return xml;
					}
				}
			}catch(Exception){}
			using(LicenseGenerator Lg=new LicenseGenerator()) {
				string text=Lg.ParseLicenseKey(text64);
				DateTime trialExpiry;
				bool isDateTime=DateTime.TryParse(text,out trialExpiry);
				if(isDateTime){
					return text;
				}
			}
			return String.Empty;
		}
		[WebMethod]
		public string[] GetLicenseStateStrings(){
			string[] names=Enum.GetNames(typeof(LicenseState));
			return names;
		}
	}
}
