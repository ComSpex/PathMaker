using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Dispatcher;

namespace WcfServiceOne {
	class Program:IInteractiveChannelInitializer {
		static void Main(string[] args) {
			Service1Client cli=new Service1Client();
			cli.Open();
			cli.DisplayInitializationUI();
			Report("GetData = {0}",cli.GetData(1000));
			CompositeType ct=new CompositeType();
			ct.StringValue="CompositeType";
			ct.BoolValue=true;
			Report("CompositType = {0}",cli.GetDataUsingDataContract(ct).StringValue);
			cli.Close();
		}
		public static void Report(Exception ex){
			Report("{0}",ex.ToString());
		}
		public static void Report(string format,params object[] args){
			string text=String.Format(format,args);
			System.Diagnostics.Debug.WriteLine(text,DateTime.Now.ToString("HH:mm:ss.fff"));
			Console.WriteLine(text);
		}
		public IAsyncResult BeginDisplayInitializationUI(System.ServiceModel.IClientChannel channel,AsyncCallback callback,object state) {
			throw new NotImplementedException();
		}
		public void EndDisplayInitializationUI(IAsyncResult result) {
			throw new NotImplementedException();
		}
	}
}
