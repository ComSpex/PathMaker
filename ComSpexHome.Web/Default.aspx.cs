// $Header: /ComSpexHome.root/ComSpexHome/Visual Studio 2010/Projects/ComSpexHome/ComSpexHome.Web/Default.aspx.cs 2     11/16/13 9:17a Yosuke $
//#define verify_1
//#define verify_2
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;

namespace ComSpexHome.Web {
	public partial class _Default:System.Web.UI.Page {
		protected void Page_Load(object sender,EventArgs e) {
			if(IsMozilla){
				Response.Redirect("SilverlightHomeTestPage.aspx");
			}else{
				using(StringWriter Sw=new StringWriter()){
					if(IsJapaneseAccepted){
						Response.ContentEncoding=Encoding.GetEncoding("shift_jis");
						Sw.WriteLine("<h1{1}>{0}</h1>",@"<font color=""hotpink"">コムスペックへ、ようこそ</font>","");
						Sw.WriteLine("<br />");
						Sw.WriteLine("{0}<br />",@"<font color=""forestgreen"" size=""1"">コムスペックスは、１９８９年創業以来、コンピュータ・ソフトウェアを作り続けています。</font>");
						Sw.WriteLine("{0}<br />",@"<font color=""forestgreen"" size=""1"">近年では、ASP.NET, C/C++, C#, MSSQL, LINQ, WPF(Silverlight) の実績在り。</font>");
						Sw.WriteLine("<br />");
						Sw.WriteLine("{0}<br />",@"<font color=""RoyalBlue"" size=""1"">ＣｏｍＳｐｅｘ</font>");
						Sw.WriteLine("{0}<br />",@"<font color=""RoyalBlue"" size=""1"">〒518-0116三重県伊賀市上神戸４４９７－１５９</font>");
						Sw.WriteLine("{0}<br />",@"<font color=""RoyalBlue"" size=""1"">電話：0595-36-0808</font>");
						Sw.WriteLine("{0}<br />",@"<font color=""RoyalBlue"" size=""1"">mail : <a href=""mailto:sales@comspex.com"">sales@comspex.com</a></font>");
						Sw.WriteLine("{0}<br />",@"<font color=""RoyalBlue"" size=""1"">前田洋輔</font>");
						Sw.WriteLine("<br />{0}<br />",@"<font color=""#aaaaaa"" size=""1"">as of 2009/07/07</font>");
					} else {
						Sw.WriteLine("<h1>{0}</h1>",@"<font color=""forestgreen"">Welcome to <i><b>ComSpex</b></i></font>");
						Sw.WriteLine("<br />");
						Sw.WriteLine("{0}<br />",@"<font color=""black"" size=""3""><i><b>ComSpex</b></i> have been making many computer software programs at the inception since April, 1989.</font>");
						Sw.WriteLine("{0}<br />",@"<font color=""black"" size=""3"">We are well versed in Microsoft&reg; Windows and web applications by using ASP.NET, C/C++, C#, LINQ, WPF(Silverlight) and MSSQL.</font>");
						Sw.WriteLine("<br />");
						Sw.WriteLine("{0}<br />",@"<font color=""RoyalBlue"" size=""2"">ComSpex</font>");
						Sw.WriteLine("{0}<br />",@"<font color=""RoyalBlue"" size=""2"">4497-159 Kamikambe, Iga, 518-0116 Japan</font>");
						Sw.WriteLine("{0}<br />",@"<font color=""RoyalBlue"" size=""2"">Phone：+81-595-36-0808</font>");
						Sw.WriteLine("{0}<br />",@"<font color=""RoyalBlue"" size=""2"">mail : <a href=""mailto:sales@comspex.com"">sales@comspex.com</a></font>");
						Sw.WriteLine("{0}<br />",@"<font color=""RoyalBlue"" size=""2"">Yosuke Maeda</font>");
						
						Sw.WriteLine("<br />{0}<br />",@"<font color=""#aaaaaa"" size=""1"">Last updated : 2009/07/07</font>");
					}
					Response.Write(Sw.ToString());
				}
			}
		}
		bool IsMozilla{
			#if verify_1
			get{return false;}
			#else
			get{return Request.UserAgent.Contains("Mozilla");}
			#endif
		}
		bool IsJapaneseAccepted{
			#if verify_2
			get{return true;}
			#else
			get{return ALL_HTTP.Contains("ja-jp");}
			#endif
		}
		string AcceptLanguage{
			get{return Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"];}
		}
		string ALL_HTTP{
			get{return Request.ServerVariables["ALL_HTTP"];}
		}
	}
}
