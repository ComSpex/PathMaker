// $Header: /ComSpexHome.root/ComSpexHome/Visual Studio 2010/Projects/ComSpexHome/LicenserTest/Program.cs 1     11/07/13 7:23p Yosuke $
using System;
using System.Collections.Generic;
using System.Text;

namespace LicenserTest {
	using ComSpex.Tools;
	using ComSpexLix;
	using Licenser;
	using System.IO;
	using System.Threading;
	class Program {
		static void Main(string[] args) {
			using(LicenseService Ls=new LicenseService()) {
				byte[] buff=Encoding.ASCII.GetBytes("boss");
				string password=Convert.ToBase64String(buff);
				string xml=Ls.DownloadActivationSets(password);
				Report("{0}",xml);
				using(StreamWriter Sw=new StreamWriter("LicenseManagement.xml",false,Encoding.UTF8)){
					Sw.Write(xml);
				}
				#if false
				if(!String.IsNullOrEmpty(xml)){
					ActivationSet[] items=ActivationSet.ToArray(xml);
					foreach(ActivationSet item in items){
						Report("{0}",item.ToXmlString());
						Report("---------");
					}
				}
				#endif
			}
		}
		static void TestMain(){
			//試用期限2010年02月21日までのライセンス・キーです。
			string licenseKey="VfZ0soW2FEBPuTaK9z01vg/732/aG84pvihM3CfvKnW7Zp1dbjCBNNeKQao8UaqlAABJLHo8PQW05xCfpWjQnd0DdzJh8jd78RufH0PnCrC3lQoJrSJMQg6sudTmJ+8jCkFoEGy97lP7+CRTMgnhWEQq7+d8g5klh178R26llpM=";
			using(LicenseGenerator Ls=new LicenseGenerator()) {
				string text=Ls.ParseLicenseKey(licenseKey);
				DateTime expiry;
				DateTime.TryParse(text,out expiry);
				Report("{0} {1}",expiry.ToLongDateString(),expiry.ToLongTimeString());
			}
		}
		static void ProductionMain(){
			Thread.CurrentThread.CurrentCulture=
			Thread.CurrentThread.CurrentUICulture=new System.Globalization.CultureInfo("ja-JP");
			string licenseKey="bXvLQ7WQJLz4zOWV5Voyt8RJrQXnZTd1udgGXn+FZUB4Ix1NIYUuy+Rkv7aCSpN/NjkgVcdfJSGvlqvqFXzoFPJAnMUZ/BT4n6Ca+/7R6D0BIVjLHusFpSJblH9EjHPzardNaVkIFMwbUhVytP20Gds3/UAB9AJBR+2OaNV075s=";
			//試用期限2010年02月20日までのライセンス・キーです。
			licenseKey="RkU0PlcLa0DFX4FKOsNMRqvwwhoxQL1AWr7vqnC7zCeH6MyOX0tcq8Tq87l20rryt0G6a7w5bhrk2L55+AVqqgSQIABHVXRuvt3ZNIOx2iTpzeltbKZ5/dZuCOnVkzNuytNvdyNealCjtQvczlPfwaMGTJ1s8WcxRvpdx8bnhVo=";
			//SendNoticeToLix(null);
			using(LicenseService Ls=new LicenseService()){
				string text=Ls.ParseLicenseKey(licenseKey);
				DateTime expiry;
				DateTime.TryParse(text,out expiry);
				Report("{0}#{1}",expiry.ToLongDateString(),expiry.ToLongTimeString());
				//Ls.CleanActivationCache();
				Report("{0}",Ls.GetActivationSet("W8V154BX COM1"));
				Report("{0}",Ls.GetActivationSet("W8V154BX COM2"));
				Report("{0}",Guid.NewGuid());
				string[] names=Ls.GetLicenseStateStrings();
				foreach(string name in names){
					Print(@"case LicenseState.{0}:",name);
					Print(@"text=ResourceFactory.Get(""License_{0}"");",name);
					Print(@"break;");
				}
			}
			//GenerateTrialLicenseKey(14);
		}
		static void SendNoticeToLix(object param) {
			using(LicenseService Ls=new LicenseService()) {
				ActivationSet item=new ActivationSet(Environment.MachineName,typeof(Program).AssemblyQualifiedName);
				string xml=ActivationSet.ToXmlString(item);
				Ls.AddActivationSet(xml);
			}
		}
		static void GenerateTrialLicenseKey(double days) {
			string seed=DateTime.Today.Add(TimeSpan.FromDays(days)).ToLongDateString();
			LicenseService Bob=new LicenseService();
			using(LicenseGenerator Alice=new LicenseGenerator()) {
				string text64=Bob.EncryptSessionKeyByRSA(seed,Alice.Public_KeyUsed);
				byte[] buff=Convert.FromBase64String(text64);
				Report("Trial License until {1}:\n{0}",text64,Alice.DecryptSessionKeyByRSA(text64));
			}
		}
		static void QuodEratDemonstrandum(){
			bool AliceToBob=true;
			LicenseService Bob=new LicenseService();
			if(AliceToBob) {
				using(LicenseGenerator Alice=new LicenseGenerator()) {
					string text64=Bob.EncryptSessionKeyByRSA("How can it be",Alice.Public_KeyUsed);
					byte[] buff=Convert.FromBase64String(text64);
					Report("Local Result = [{0}]",Alice.DecryptSessionKeyByRSA(text64));
				}
			}else{
				using(LicenseGenerator Alice=new LicenseGenerator()) {
					string text64=Alice.EncryptSessionKeyByRSA("How can it be",Bob.GetPublicKey());
					Report("Result = [{0}]",Bob.DecryptSessionKeyByRSA(text64));
				}
			}
		}
		private static ActivationSet[] ToArray(string text) {
			using(StringReader sr=new StringReader(text)) {
				using(ComSpex.XmlPersist<ActivationSet[]> xp=new ComSpex.XmlPersist<ActivationSet[]>()) {
					return xp.Load(sr);
				}
			}
		}
		private static string ToString(ActivationSet item) {
			using(StringWriter sw=new StringWriter()){
				using(ComSpex.XmlPersist<ActivationSet> xp=new ComSpex.XmlPersist<ActivationSet>(item)){
					return xp.Save(sw);
				}
			}
		}
		private static ActivationSet ToClass(string text) {
			using(StringReader sr=new StringReader(text)){
				using(ComSpex.XmlPersist<ActivationSet> xp=new ComSpex.XmlPersist<ActivationSet>()){
					return xp.Load(sr);
				}
			}
		}
		public static void Report(Exception ex){
			Report("{0}",ex.ToString());
		}
		public static void Report(string format,params object[] args){
			string text=String.Format(format,args);
			System.Diagnostics.Debug.WriteLine(text,DateTime.Now.ToString("HH:mm:ss.fff"));
			Console.WriteLine(text);
		}
		public static void Print(string format,params object[] args) {
			string text=String.Format(format,args);
			System.Diagnostics.Debug.WriteLine(text);
			Console.WriteLine(text);
		}
	}
}
