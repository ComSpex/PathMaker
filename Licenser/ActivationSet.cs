// $Header: /ComSpexHome.root/ComSpexHome/Visual Studio 2010/Projects/ComSpexHome/Licenser/ActivationSet.cs 1     11/07/13 7:23p Yosuke $
using System;
using System.Collections.Generic;
using System.Web;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Globalization;

namespace Licenser {
	[System.Runtime.InteropServices.ComVisible(false)]
	public enum LicenseState {
		NotCheckedYet,
		TrialState,
		TrialPeriodExpired,
		InternalIntegrityError,
		MachineDifferent,
		Validated,
		LicenseServiceUnavailable,
		PortDifferent,
		ContractExpired,
	}
	[System.Runtime.InteropServices.ComVisible(false)]
	public partial class UserInfo {
		[XmlAttribute]
		public string MailAddress;
		public UserInfo(){}
		public UserInfo(UserInfo x)
			:this(){
			if(x!=null){
				MailAddress=x.MailAddress;
			}
		}
	}
	[System.Runtime.InteropServices.ComVisible(false)]
	public partial class PaymentInfo {
		[XmlAttribute]
		public string TransactionId;
		[XmlAttribute]
		public DateTime DatePaid;
		public bool IsPaid{
			get{
				return 
					DatePaid!=DateTime.MinValue&&
					!String.IsNullOrEmpty(TransactionId);
			}
		}
		public PaymentInfo(){}
		public PaymentInfo(PaymentInfo x)
			:this(){
			if(x!=null){
				TransactionId=x.TransactionId;
				DatePaid=x.DatePaid;
			}
		}
	}
	[System.Runtime.InteropServices.ComVisible(false)]
	public partial class InstallerInfo {
		[XmlAttribute]
		public Guid ProductCode;
		[XmlAttribute]
		public Guid UpgradeCode;
		public InstallerInfo(){}
		public InstallerInfo(Guid pc,Guid uc)
			:this(){
			ProductCode=pc;
			UpgradeCode=uc;
		}
		public InstallerInfo(InstallerInfo x)
			:this(){
			if(x!=null){
				ProductCode=x.ProductCode;
				UpgradeCode=x.UpgradeCode;
			}
		}
	}
	[System.Runtime.InteropServices.ComVisible(false)]
	public partial class ActivationSet {
		[XmlAttribute]
		public Guid Id;
		[XmlAttribute]
		public DateTime DateRegistered;
		[XmlAttribute]
		public string MachineName;
		[XmlAttribute]
		public string TargetType;
		[XmlAttribute]
		public ulong Count;
		[XmlAttribute]
		public DateTime DateUpdated;
		[XmlAttribute]
		public DateTime Expiry;
		[XmlAttribute]
		public LicenseState Status;
		public UserInfo User;
		public PaymentInfo[] Sales;
		public InstallerInfo Setup;
		public ActivationSet() { 
			User=new UserInfo();
			Sales=new PaymentInfo[0];
			Setup=new InstallerInfo();
		}
		public ActivationSet(string xml){
			if(!String.IsNullOrEmpty(xml)){
				ActivationSet item=ToClass(xml);
				Id=item.Id;
				DateUpdated=item.DateUpdated;
				MachineName=item.MachineName;
				TargetType=item.TargetType;
				Count=item.Count;
				DateRegistered=item.DateRegistered;
				Expiry=item.Expiry;
				User=new UserInfo(item.User);
				Sales=new PaymentInfo[item.Sales.Length];
				Array.Copy(item.Sales,Sales,Sales.Length);
				Setup=new InstallerInfo(item.Setup);
				Status=item.Status;
			}
		}
		public ActivationSet(string machineName,string typeName)
			: this() {
			Id=Guid.NewGuid();
			DateUpdated=
			DateRegistered=DateTime.Now;
			MachineName=machineName.ToUpper();
			TargetType=typeName;
			Count=1;
		}
		public ActivationSet(string machineName,string typeName,Guid pc,Guid uc)
			: this(machineName,typeName) {
			Setup=new InstallerInfo(pc,uc);
		}
		public string ToXmlString(){
			return ActivationSet.ToXmlString(this);
		}
		public bool IsPaid{
			get{
				//When the latest payment is good, it's good!
				if(Sales!=null&&Sales.Length>0) {
					PaymentInfo Sale=Sales[Sales.Length-1];
					return Sale.IsPaid;
				}
				return false;
			}
		}
		public static ActivationSet[] ToArray(string xml) {
			XmlSerializer X=new XmlSerializer(typeof(ActivationSet[]));
			using(StringReader Sr=new StringReader(xml)) {
				return X.Deserialize(Sr) as ActivationSet[];
			}
		}
		public static string FromArray(ActivationSet[] target){
			XmlSerializer X=new XmlSerializer(target.GetType());
			using(StringWriter Sw=new StringWriter()) {
				X.Serialize(Sw,target);
				return Sw.ToString();
			}
		}
		public static string ToXmlString(ActivationSet target) {
			XmlSerializer X=new XmlSerializer(target.GetType());
			using(StringWriter Sw=new StringWriter()) {
				X.Serialize(Sw,target);
				return Sw.ToString();
			}
		}
		public static ActivationSet ToClass(string xml) {
			XmlSerializer X=new XmlSerializer(typeof(ActivationSet));
			using(StringReader sr=new StringReader(xml)) {
				return X.Deserialize(sr) as ActivationSet;
			}
		}
	}
}
