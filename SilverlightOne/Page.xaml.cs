// $Header: /ComSpexHome.root/ComSpexHome/Visual Studio 2010/Projects/ComSpexHome/SilverlightOne/Page.xaml.cs 3     11/13/13 4:50p Yosuke $
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SilverlightOne {
	public partial class Page:UserControl {
		DispatcherTimer fpdTimer=new DispatcherTimer();
		public Page() {
			InitializeComponent();
			//theLogo.DesignMode=true;
			fpdTimer.Interval=TimeSpan.FromMilliseconds(10);
			fpdTimer.Tick+=new EventHandler(fpdTimer_Tick);
			this.MouseMove+=new MouseEventHandler(Page_MouseMove);
		}
		void Page_MouseMove(object sender,MouseEventArgs e) {
			if(!theLogo.IsStopped){
				//theLogo.StopAnimation();
			}
		}
		private void Grid_MouseEnter(object sender,MouseEventArgs e) {
			Grid grid=sender as Grid;
			grid.Cursor=Cursors.Hand;
			this.mailSwinger.Begin();
		}
		private void Grid_MouseLeave(object sender,MouseEventArgs e) {
			Grid grid=sender as Grid;
			grid.Cursor=Cursors.Arrow;
		}

		DispatcherTimer swiss,clockDimmer;
		private void mondaineSwissWatch_MouseEnter(object sender,MouseEventArgs e) {
			if(theLogo.DesignMode){
				return;
			}
			Clock clock=sender as Clock;
			if(swiss!=null){
				if(swiss.IsEnabled){
					swiss.Stop();
				}
			}
			if(clockDimmer!=null){
				if(clockDimmer.IsEnabled){
					clockDimmer.Stop();
				}
			}
			clock.Opacity=1.0;
			ScaleTransform st=new ScaleTransform();
			st.ScaleY=
			st.ScaleX=3.0;
			clock.RenderTransform=st;
			swissBack.Children.Remove(this.mondaineSwissWatch);
			if(!swissFore.Children.Contains(this.mondaineSwissWatch)){
				swissFore.Children.Add(this.mondaineSwissWatch);
			}
		}
		private void mondaineSwissWatch_MouseLeave(object sender,MouseEventArgs e) {
			if(theLogo.DesignMode) {
				return;
			}
			Clock clock=sender as Clock;
			//swissFore.Children.Remove(clock);
			//swissBack.Children.Add(clock);
			if(swiss==null) {
				swiss=new DispatcherTimer();
				swiss.Tick+=new EventHandler(swiss_Tick);
				swiss.Interval=TimeSpan.FromMilliseconds(5);
			}
			swiss.Start();
		}
		void swiss_Tick(object sender,EventArgs e) {
			DispatcherTimer timer=sender as DispatcherTimer;
			ScaleTransform scat=this.mondaineSwissWatch.RenderTransform as ScaleTransform;
			scat.ScaleY=scat.ScaleX-=0.01;
			Report("ScaleY={0}",scat.ScaleY);
			if(string.Format("{0:0.000}",scat.ScaleY)=="2.700") {
				swissFore.Children.Remove(this.mondaineSwissWatch);
				swissBack.Children.Add(this.mondaineSwissWatch);
			}
			if(scat.ScaleY<=1.0){
				timer.Stop();
				if(clockDimmer==null){
					clockDimmer=new DispatcherTimer();
					clockDimmer.Interval=TimeSpan.FromMilliseconds(10);
					clockDimmer.Tick+=new EventHandler(clockDimmer_Tick);
				}
				clockDimmer.Start();
			}
		}
		void clockDimmer_Tick(object sender,EventArgs e) {
			DispatcherTimer timer=sender as DispatcherTimer;
			this.mondaineSwissWatch.Opacity-=0.01;
			if(this.mondaineSwissWatch.Opacity<=0.1){
				timer.Stop();
			}
		}
		private void Report(string format,params object[] args) {
			System.Diagnostics.Debug.WriteLine(String.Format(format,args),DateTime.Now.ToString("HH:mm:ss.fff"));
		}

		private void DoubleAnimation_Completed(object sender,EventArgs e) {
			theLogo.AnimationCompleted();
			fpdTimer.Start();
		}
		void fpdTimer_Tick(object sender,EventArgs e) {
			theLogo.FpdOpacity+=0.01;
			if(theLogo.FpdOpacity>=1.0) {
				((DispatcherTimer)sender).Stop();
				//theLogo.StartAnimation();
			}
		}
		private void theLogo_Loaded(object sender,RoutedEventArgs e) {
			theLogo.FpdOpacity=0;
		}
		private void Grid_MouseLeftButtonUp(object sender,MouseButtonEventArgs e) {
			Uri uri=new Uri("http://samples.msdn.microsoft.com/Silverlight/SampleBrowser/index.htm#/?sref=HomePage");
			HyperlinkButton a=new HyperlinkButton();
			a.NavigateUri=uri;
			//MessageBox.Show("Thank you for clicking!!");
		}

		private void DoubleAnimationSince1989_Completed(object sender,EventArgs e) {
			//this.Content=new PatliteControl();
			System.Diagnostics.Debug.WriteLine(String.Format("{0} x {1}",LayoutRoot.ActualWidth,LayoutRoot.ActualHeight));
			this.LRmenu.Visibility=Visibility.Visible;
			DropMenu();
		}
		void DropMenu(){
			DispatcherTimer Menu=new DispatcherTimer();
			Menu.Interval=TimeSpan.FromSeconds(0.01);
			Menu.Tick+=new EventHandler(Menu_Tick);
			Canvas.SetTop(this.LRmenu,0.0);
			TranslateTransform tt=new TranslateTransform();
			tt.Y=Canvas.GetTop(this.LRmenu);
			this.LRmenu.RenderTransform=tt;
			Menu.Start();
		}
		void Menu_Tick(object sender,EventArgs e) {
			DispatcherTimer timer=sender as DispatcherTimer;
			double endY=this.LayoutRoot.RenderSize.Height-(this.LRmenu.RenderSize.Height+this.LRmenu.Margin.Bottom);
			TranslateTransform tt=this.LRmenu.RenderTransform as TranslateTransform;
			double fallSpeed=50.0;
			double ttY=tt.Y+fallSpeed;
			if(ttY>=endY) {
				double windowBottomFrameHeight=6.0;
				tt.Y=endY-windowBottomFrameHeight;
				this.LRmenu.RenderTransform=tt;
				timer.Stop();
				return;
			}
			tt.Y=ttY;
			this.LRmenu.RenderTransform=tt;
		}
		private void UserControl_SizeChanged(object sender,SizeChangedEventArgs e) {
			if(this.LRmenu.Visibility==Visibility.Visible) {
				DropMenu();
			}
		}

		private void goL_Click(object sender,RoutedEventArgs e) {
			this.Content=new Test_Patlite();
		}
		private void goR_Click(object sender,RoutedEventArgs e) {
			this.Content=new ColorfulPageOne();
		}
	}
}
