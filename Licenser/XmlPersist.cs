// $Header: /ComSpexHome.root/ComSpexHome/Visual Studio 2010/Projects/ComSpexHome/Licenser/XmlPersist.cs 1     11/07/13 7:23p Yosuke $
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ComSpex{
	//Type library exporter warning processing 'ComSpex.IXmlPersist`1, ComSpex.Serialio'. 
	//Warning: Type library exporter encountered a generic type. Generic classes may not 
	//be exposed to COM.	ComSpex.Serialio
	[ComVisible(false)]
	public class XmlPersist<T>:IDisposable {
		protected XmlSerializer Xs;
		protected T Target;
		public XmlPersist() {
			Xs=new XmlSerializer(typeof(T));
		}
		public XmlPersist(T target) {
			Target=target;
			Xs=new XmlSerializer(Target.GetType());
		}
		public string Save(TextWriter writer) {
			return Save(writer,null);
		}
		public string Save(TextWriter writer,XmlSerializerNamespaces ns) {
			if(ns!=null){
				Xs.Serialize(writer,Target,ns);
			}else{
				Xs.Serialize(writer,Target);
			}
			return writer.ToString();
		}
		public T Load(TextReader reader) {
			try {
				return (T)Xs.Deserialize(reader);
			} catch(Exception ex) {
				System.Diagnostics.Debug.Write(ex.ToString());
			}
			return default(T);
		}
		public string Convert(T target) {
			XmlSerializer X=new XmlSerializer(target.GetType());
			using(StringWriter Sw=new StringWriter()) {
				X.Serialize(Sw,target);
				return Sw.ToString();
			}
		}
		public T Convert(string xml) {
			XmlSerializer X=new XmlSerializer(typeof(T));
			using(StringReader Sr=new StringReader(xml)) {
				return (T)X.Deserialize(Sr);
			}
		}
		virtual public void Dispose() {
		}
	}
	[ComVisible(false)]
	public interface IXmlPersist<T> {
		string Save(TextWriter writer);
		T Load(TextReader reader);
		string Convert(T target);
		T Convert(string xml);
	}
}
