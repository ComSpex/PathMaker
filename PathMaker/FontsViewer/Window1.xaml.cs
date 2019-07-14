// $Header: /ComSpexHome.root/ComSpexHome/Visual Studio 2010/Projects/ComSpexHome/FontsViewer/Window1.xaml.cs 1     11/07/13 7:23p Yosuke $
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
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace FontsViewer {
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1:Window {
		ICollection<FontFamily> fontFamilies;
		public Window1() {
			InitializeComponent();
			fontFamilies=Fonts.SystemFontFamilies;
			this.unifog.Columns=4;
			this.unifog.Rows=fontFamilies.Count/this.unifog.Columns;
			if(fontFamilies.Count%this.unifog.Columns!=0) {
				++this.unifog.Rows;
			}
			foreach(FontFamily fontFamily in fontFamilies) {
				Report("FontFamily.Source={0}",fontFamily.Source);
				string[] familyName=fontFamily.Source.Split('#');
				string name=familyName[familyName.Length-1];
				this.fontList.Items.Add(name);
			}
			DispatcherTimer Loader=new DispatcherTimer();
			Loader.Interval=TimeSpan.FromSeconds(0.5);
			Loader.Tick+=new EventHandler(Loader_Tick);
			Loader.Start();
		}
		void Loader_Tick(object sender,EventArgs e) {
			statusText.Content=String.Format("{0}","Just moment, please...");
			statusText.Foreground=Brushes.White;
			
			this.Opacity=0.5;
			scroller.IsEnabled=false;
			((DispatcherTimer)sender).Stop();
			this.Cursor=Cursors.AppStarting;
			int index=0;
			double diff=0.5/Convert.ToDouble(fontFamilies.Count);
			foreach(FontFamily fontFamily in fontFamilies) {
				string[] familyName=fontFamily.Source.Split('#');
				string name=familyName[familyName.Length-1];
				FontFamily ff=new FontFamily(name);
				StackPanel child=new StackPanel();
				child.RenderTransformOrigin=new Point(0.5,0.5);
				{
					TextBlock block=new TextBlock();
					block.FontFamily=ff;
					block.Text=(string)this.Resources["alphaUpper"];
					child.Children.Add(block);
				}
				{
					TextBlock block=new TextBlock();
					block.FontFamily=ff;
					block.Text=(string)this.Resources["alphaLower"];
					child.Children.Add(block);
				}
				{
					TextBlock block=new TextBlock();
					block.FontFamily=ff;
					block.Text=(string)this.Resources["numbers"];
					child.Children.Add(block);
				}
				{
					TextBlock block=new TextBlock();
					block.FontFamily=ff;
					block.Text=(string)this.Resources["symbols"];
					child.Children.Add(block);
				}
				{
					TextBlock block=new TextBlock();
					block.FontFamily=ff;
					block.Text=(string)this.Resources["japanese"];
					child.Children.Add(block);
				}
				{
					TextBlock block=new TextBlock();
					block.FontFamily=ff;
					Binding bind=new Binding();
					bind.Source=samplet;
					bind.Path=new PropertyPath(TextBlock.TextProperty);
					block.SetBinding(TextBlock.TextProperty,bind);
					child.Children.Add(block);
				}
				Border inner=new Border();
				inner.Tag=index;
				inner.Style=this.Resources["fontInner"] as Style;
				inner.Child=child;

				Border outer=new Border();
				outer.Tag=index;
				outer.Style=this.Resources["fontOuter"] as Style;
				outer.MouseDown+=new MouseButtonEventHandler(SetViewboxMaximized);
				outer.PreviewMouseMove+=new MouseEventHandler(UpdateStatusBar);
				outer.Child=inner;
				
				unifog.Children.Add(outer);

				++index;
				DoEvents();
				this.Opacity+=diff;
				this.Title=String.Format("System Font Family - Loading {0,3}/{1} {2}",index,fontFamilies.Count,name);
			}
			this.Cursor=Cursors.Arrow;
			this.fontList.SelectedIndex=0;
			scroller.IsEnabled=true;
			this.Opacity=1.0;
			statusText.Content=String.Format("{0}","Ready");
			statusText.Foreground=Brushes.Blue;
		}
		void UpdateStatusBar(object sender,MouseEventArgs e) {
			Border outer=sender as Border;
			if(outer!=null){
				statusText.Content=String.Format("{0}",fontList.Items[(int)outer.Tag]);
				statusText.Foreground=Brushes.Black;
			}
		}
		void DoEvents() {
			DispatcherFrame frame=new DispatcherFrame();
			Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Render,new DispatcherOperationCallback(ExitFrames),frame);
			Dispatcher.PushFrame(frame);
		}
		object ExitFrames(object f) {
			((DispatcherFrame)f).Continue=false;
			return null;
		}
		private void SetViewboxMaximized(object sender,MouseButtonEventArgs e) {
			Border outer=sender as Border;
			Border inner=outer.Child as Border;
			if(inner!=null){
				inner.CaptureMouse();
				outer.Child=null;
				viewSpecimen.Child=inner;
				inner.BorderBrush=new SolidColorBrush(Color.FromRgb(0x33,0x99,0xff));
				fontList.SelectedIndex=(int)outer.Tag;
				fontList.ScrollIntoView(fontList.SelectedItem);
			}
		}
		private void Viewbox_MouseDown(object sender,MouseButtonEventArgs e) {
			if(e.LeftButton==MouseButtonState.Pressed) {
				SetViewboxRestored();
			}
		}
		private void Viewbox_KeyDown(object sender,KeyEventArgs e) {
			if(e.Key==Key.Escape&&e.IsDown) {
				SetViewboxRestored();
			}
		}
		private void SetViewboxRestored(){
			FrameworkElement specimen=viewSpecimen.Child as FrameworkElement;
			Border inner=specimen as Border;
			if(inner!=null){
				inner.ReleaseMouseCapture();
				viewSpecimen.Child=null;
				Border outer=unifog.Children[(int)inner.Tag] as Border;
				outer.Child=inner;
			}
		}
		void Report(string format,params object[] args) {
			System.Diagnostics.Debug.WriteLine(String.Format(format,args),DateTime.Now.ToString("HH:mm:ss.fff"));
		}
		List<Border> pools=new List<Border>();
		private void fontList_SelectionChanged(object sender,SelectionChangedEventArgs e) {
			string familyName=(string)fontList.SelectedItem;
			this.Title=String.Format("System Font Family - {0}",familyName);
			bool isChild=false;
			string key=String.Empty;
			string[] cmds=Environment.GetCommandLineArgs();
			if(cmds.Length==2){
				string cmd=cmds[1];
				if(cmd.ToLower().StartsWith("/key:")){
					key=cmd.Substring(cmd.IndexOf(':')+1);
					if(!String.IsNullOrEmpty(key)){
						isChild=true;
					}
				}
			}
			if(isChild){
				Clipboard.SetText(String.Format("{0}={1}",key,familyName));
			} else {
				Clipboard.SetText(familyName);
			}
			if(unifog.Children.Count>=fontFamilies.Count){
				foreach(Border pool in pools){
					Border inner=(pool.Child as Border);
					if(inner!=null){
						inner.BorderBrush=Brushes.Transparent;
					}
				} 
				pools.Clear();
				Border outer=unifog.Children[fontList.SelectedIndex] as Border;
				if(outer!=null){
					if(outer.Child!=null){
						Border inner=outer.Child as Border;
						if(inner!=null){
							inner.BorderBrush=new SolidColorBrush(Color.FromRgb(0x33,0x99,0xff));
							outer.BringIntoView();
							pools.Add(outer);
						}
					}
				}
			}
		}
		private void MenuItem_Click(object sender,RoutedEventArgs e) {
			MenuItem mitem=sender as MenuItem;
			switch((string)mitem.Header){
				case "Italic":
					if(mitem.IsChecked) {
						viewSpecimen.SetValue(TextBlock.FontStyleProperty,FontStyles.Normal);
					} else {
						viewSpecimen.SetValue(TextBlock.FontStyleProperty,FontStyles.Italic);
					}
					break;
				case "Bold":
					if(mitem.IsChecked) {
						viewSpecimen.SetValue(TextBlock.FontWeightProperty,FontWeights.Normal);
					} else {
						viewSpecimen.SetValue(TextBlock.FontWeightProperty,FontWeights.Bold);
					}
					break;
				default:
					viewSpecimen.SetValue(TextBlock.FontStyleProperty,FontStyles.Normal);
					viewSpecimen.SetValue(TextBlock.FontWeightProperty,FontWeights.Normal);
					doItalic.IsChecked=doBold.IsChecked=false;
					break;
			}
			mitem.IsChecked=!mitem.IsChecked;
			doClear.IsEnabled=(doItalic.IsChecked||doBold.IsChecked);
			doClear.IsChecked=false;
		}
		private void samplet_MouseDown(object sender,MouseButtonEventArgs e) {
			InputBox Box=new InputBox(samplet,this);
			Box.ShowDialog();
		}
		private void Window_PreviewKeyDown(object sender,KeyEventArgs e) {
			Viewbox_KeyDown(sender,e);
		}
	}
}
