// $Header: /ComSpexHome.root/ComSpexHome/Visual Studio 2010/Projects/ComSpexHome/PathMaker/Window1.xaml.cs 1     11/07/13 7:24p Yosuke $
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;
using Petzold.ListNamedBrushes;
using System.Xml;
using System.IO;
using System.Windows.Threading;
using System.Diagnostics;
using System.Threading;

namespace PathMaker {
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1:Window {
		DispatcherTimer clipper;
		Process child;
		public Window1() {
			InitializeComponent();
			for(int i=0;i<frameBackground.Items.Count;++i){
				NamedBrush item=frameBackground.Items[i] as NamedBrush;
				string[] names=item.BrushName.Split(' ');
				if(names.Length==2&&names[names.Length-1]=="White"){
					frameBackground.SelectedIndex=i;
					break;
				}
			}
			clipper=new DispatcherTimer();
			clipper.Tick+=new EventHandler(clipper_Tick);
			clipper.Interval=TimeSpan.FromSeconds(1);
			clipper.Start();
		}
		protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
			base.OnClosing(e);
			Thread thread=new Thread(new ParameterizedThreadStart(CloseChild));
			thread.Start(child);
			Thread.Sleep(0);
		}
		void CloseChild(object param){
			Process child=param as Process;
			if(child!=null) {
				if(!child.HasExited) {
					child.CloseMainWindow();
				}
				child.Dispose();
			}
		}
		void clipper_Tick(object sender,EventArgs e) {
			string text=Clipboard.GetText();
			if(!String.IsNullOrEmpty(text)){
				if(text.StartsWith("FontFamily")){
					string[] pair=text.Split('=');
					if(pair.Length==2){
						targetFontFamily.Text=pair[1];
						Clipboard.Clear();
					}
				}
			}
		}
		private void Button_Click(object sender,RoutedEventArgs e) {
			if(textRequested!=null){
				Clipboard.SetText(textRequested.ToXAML(false));
				MessageBox.Show(this,"Copied to Clipboard",this.Title,MessageBoxButton.OK,MessageBoxImage.Information);
			}
		}
		private void Label_MouseDown(object sender,MouseButtonEventArgs e) {
			string path=String.Empty;
			Label la=sender as Label;
			string face=(string)la.Content;
			switch(face){
				case "FontFamily":
					path=@"D:\Users\Yosuke.REGGELT\Documents\Visual Studio 2008\Projects\SilverlightOne\FontsViewer\bin\Debug\FontsViewer.exe";
					path=@"D:\Users\Yosuke\Documents\Visual Studio 2008\Projects\SilverlightOne\FontsViewer\bin\Debug\FontsViewer.exe";
					path="FontsViewer.exe";
					if(child!=null&&child.HasExited) {
						try{
							child.Dispose();
						}catch{}
						child=null;
					}
					if(child==null) {
						child=Process.Start(path,String.Format("/key:{0}",face));
					}
					break;
			}
		}
	}
}
