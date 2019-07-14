// $Header: /ComSpexHome.root/ComSpexHome/Visual Studio 2010/Projects/ComSpexHome/Licenser/Default.aspx.cs 1     11/07/13 7:23p Yosuke $
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComSpex.Tools;

namespace Licenser {
	public partial class Tools:System.Web.UI.Page {
		protected void Page_Load(object sender,EventArgs e) {
			if(!String.IsNullOrEmpty(Request.QueryString["days"])){
				try{
					using(LicenseGenerator Lg=new LicenseGenerator()){
						double value=double.Parse(Request.QueryString["days"]);
						DateTime test=DateTime.Today.AddDays(value);
						DateTime expire=new DateTime(test.Year,test.Month,test.Day,23,59,59);
						string text=Lg.EncryptSessionKeyByRSA(String.Format("{0} {1}",expire.ToLongDateString(),expire.ToLongTimeString()));
						Report("{0}",String.Format("//試用期限{0:yyyy年MM月dd日}までのライセンス・キーです。",expire));
						Report(@"COM1.LicenseKey=COM2.LicenseKey=""{0}"";",text);
					}
				}catch(Exception ex){
					Report(ex);
				}
			}else{
				Response.Redirect("LicenseService.asmx");
			}
		}
		void Report(Exception ex){
			Report("{0}",ex.ToString());
			Response.Write("<pre 'style=color:red;'>");
			Report("{0}",ex.ToString());
			Response.Write("</pre>");
		}
		void Report(string format,params object[] args){
			string text=String.Format(format,args);
			Trace.Write(text);
			Response.Write("<pre 'style=color:blue;'>");
			Response.Write(text);
			Response.Write("</pre>");
		}
	}
}
