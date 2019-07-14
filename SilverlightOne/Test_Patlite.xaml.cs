// $Header: /ComSpexHome.root/ComSpexHome/Visual Studio 2010/Projects/ComSpexHome/SilverlightOne/Test_Patlite.xaml.cs 3     11/13/13 4:50p Yosuke $
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
using System.Windows.Navigation;
using System.Windows.Threading;

namespace SilverlightOne {
	public partial class Test_Patlite {
		public Test_Patlite() {
			InitializeComponent();
		}
		protected override Size ArrangeOverride(Size finalSize) {
			this.LayoutRoot.Height=finalSize.Height;
			return base.ArrangeOverride(finalSize);
		}
		protected override void OnNavigatedTo(NavigationEventArgs e) {
			//MessageBox.Show("Here comes");
		}
		private void Button_Click(object sender,RoutedEventArgs e) {
			Button bu=sender as Button;
			string name=(string)bu.Content;
			switch(name){
				case "Red":
					this.patLite.Red=!this.patLite.Red;
					break;
				case "Yellow":
					this.patLite.Yellow=!this.patLite.Yellow;
					break;
				case "Green":
					this.patLite.Green=!this.patLite.Green;
					break;
				case "Off":
					this.patLite.Off();
					break;
				case "On":
					this.patLite.On();
					break;
			}
		}

		private void LayoutRoot_Loaded(object sender,RoutedEventArgs e) {
			DispatcherTimer delay=new DispatcherTimer();
			delay.Interval=TimeSpan.FromSeconds(1.5);
			delay.Tick+=new EventHandler(delay_Tick);
			delay.Start();
		}
		void delay_Tick(object sender,EventArgs e) {
			DispatcherTimer timer=sender as DispatcherTimer;
			timer.Stop();
			this.LRmenu.Visibility=Visibility.Visible;
			DropMenu();
		}
		void DropMenu() {
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
		private void Page_SizeChanged(object sender,SizeChangedEventArgs e) {
			if(this.LRmenu.Visibility==Visibility.Visible) {
				DropMenu();
			}
		}

		private void goL_Click(object sender,RoutedEventArgs e) {
			this.Content=new ColorfulPageOne();
		}
		private void goR_Click(object sender,RoutedEventArgs e) {
			this.Content=new Page();
		}
	}
}
