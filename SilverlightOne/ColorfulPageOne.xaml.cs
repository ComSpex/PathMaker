// $Header: /ComSpexHome.root/ComSpexHome/Visual Studio 2010/Projects/ComSpexHome/SilverlightOne/ColorfulPageOne.xaml.cs 9     11/17/13 8:15a Yosuke $
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
using System.IO;

namespace SilverlightOne {
	public partial class ColorfulPageOne {
		List<ColorsListbox> cols;
		double delta=10.0;
		bool wheelUsed=false;
		public ColorfulPageOne() {
			InitializeComponent();
			//Clipboard.SetText(String.Empty);
			Random randomized=new Random();
			int max=A.Count;
			List<int> indice=new List<int>();
			for(int i=0;i<4;) {
				int index=randomized.Next(max);
				if(!indice.Contains(index)) {
					indice.Add(index);
					++i;
				}
			}
			cols=new List<ColorsListbox>() { A,B,C,D };
			for(int i=0;i<indice.Count;++i) {
				cols[i].SelectedIndex=indice[i];
				cols[i].Tag=true;
			}
			colorTimer.Interval=TimeSpan.FromSeconds(delta);
			colorTimer.Tick+=new EventHandler(colorTimer_Tick);
			colorTimer.Start();
		}
		delegate void ColorListTraversed();
		event ColorListTraversed listTraversed;
		DispatcherTimer colorTimer=new DispatcherTimer();
		volatile int colorCount=0;
		void colorTimer_Tick(object sender,EventArgs e) {
			foreach(ColorsListbox col in cols) {
				int index=col.SelectedIndex;
				bool forward=(bool)col.Tag;
				if(forward) {
					if(++index>=col.Count) {
						forward=false;
						col.Tag=forward;
						return;
					}
				} else {
					if(--index<0) {
						forward=true;
						col.Tag=forward;
						return;
					}
				}
				col.SelectedIndex=index;
			}
			if(listTraversed!=null){
				if(++colorCount>=A.Count){
					listTraversed();
					colorCount=0;
				}
			}
		}
		protected override void OnMouseWheel(MouseWheelEventArgs e) {
			base.OnMouseWheel(e);
			wheelUsed=true;
			if(e.Delta<0) {
				colorTimer.Interval=colorTimer.Interval.Add(TimeSpan.FromSeconds(-0.1));
			} else {
				colorTimer.Interval=colorTimer.Interval.Add(TimeSpan.FromSeconds(0.1));
			}
			double sx=this.LayoutBase.RenderSize.Width/118.0;//this.info.RenderSize.Width;
			double sy=this.LayoutBase.RenderSize.Height/105.0;//this.info.RenderSize.Height;
			double scale=Math.Min(sx,sy);
			string text=String.Format("{0:0.0}",colorTimer.Interval.TotalSeconds);
			StartShowMessage(text,scale,0.1);
			e.Handled=true;
		}
		void StartShowMessage(string text,double scale,double speed,Point? pos) {
			this.info.Text=text;
			this.infoScale.ScaleX=
			this.infoScale.ScaleY=scale;
			if(pos!=null){
				this.infoTrans.X=pos.Value.X;
				this.infoTrans.Y=pos.Value.Y;
			}else{
				this.infoTrans.X=
				this.infoTrans.Y=0;
			}
			this.info.Opacity=1.0;
			this.info.Visibility=Visibility.Visible;
			DispatcherTimer infoTimer=new DispatcherTimer();
			infoTimer.Interval=TimeSpan.FromSeconds(speed);
			infoTimer.Tick+=new EventHandler(infoTimer_Tick);
			infoTimer.Start();
		}
		void StartShowMessage(string text,double scale,double speed) {
			StartShowMessage(text,scale,speed,null);
		}
		delegate void MessageTimerCompleted();
		event MessageTimerCompleted messageTimerCompleted;
		void infoTimer_Tick(object sender,EventArgs e) {
			DispatcherTimer timer=sender as DispatcherTimer;
			this.info.Opacity-=0.1;
			if(this.info.Opacity<=0.0) {
				this.info.Visibility=Visibility.Collapsed;
				timer.Stop();
				if(messageTimerCompleted!=null){
					messageTimerCompleted();
				}
			}
		}
		protected override void OnNavigatedTo(NavigationEventArgs e) {
		}

		private void LayoutBase_Loaded(object sender,RoutedEventArgs e) {
			DispatcherTimer delay=new DispatcherTimer();
			delay.Interval=TimeSpan.FromSeconds(0.8);
			delay.Tick+=new EventHandler(delay_Tick);
			delay.Start();
		}
		void delay_Tick(object sender,EventArgs e) {
			DispatcherTimer timer=sender as DispatcherTimer;
			timer.Stop();
			this.Focus();
			if(!wheelUsed) {
				//StartShowMessage("You can use\r\nMouseWheel\r\nto adjust speed.",1.0,0.7);
				ChildWindow cw=new ChildWinOne();
				cw.Show();
			}
		}

		private void Rectangle_MouseLeftButtonDown(object sender,MouseButtonEventArgs e) {
			if(sender is Grid){
				e.Handled=false;
				StartShowMessage(String.Format("Clicked!!{0}",e.GetPosition(null).ToString()),1.0,0.1);
				return;
			}
			Rectangle rec=sender as Rectangle;
			ColorItems items=A.List.ItemsSource as ColorItems;
			string text=String.Empty;
			using(StringWriter sw=new StringWriter()) {
				string xaml=Clipboard.GetText();
				if(xaml.StartsWith("<")&&xaml.EndsWith(">\r\n")) {
					sw.Write(xaml);
					string currentTime=DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
					sw.WriteLine("<!---------- {0} ------------>",currentTime);
					sw.WriteLine(@"<Rectangle Height=""10"" />");
					sw.WriteLine("<!---------- {0} ------------>",currentTime);
				}
				sw.WriteLine(@"<Rectangle Width=""{0}"" Height=""{1}"" StrokeThickness=""{2}"">",rec.RenderSize.Width,rec.RenderSize.Height,rec.StrokeThickness);
				sw.WriteLine("<Rectangle.Stroke>");
				if(rec.Stroke is LinearGradientBrush) {
					sw.WriteLine("<LinearGradientBrush>");
					LinearGradientBrush lgb=rec.Stroke as LinearGradientBrush;
					foreach(GradientStop Gs in lgb.GradientStops) {
						sw.WriteLine(@"<GradientStop Color=""{0}"" Offset=""{1}"" />",items.NameOf(Gs.Color),Gs.Offset);
					}
					sw.WriteLine("</LinearGradientBrush>");
				} else if(rec.Stroke is SolidColorBrush) {
					sw.Write("<SolidColorBrush ");
					SolidColorBrush scb=rec.Stroke as SolidColorBrush;
					sw.WriteLine(@"Color=""{0}"" />",items.NameOf(scb.Color));
				} else {
				}
				sw.WriteLine("</Rectangle.Stroke>");
				sw.WriteLine("<Rectangle.Fill>");
				if(rec.Fill is LinearGradientBrush) {
					sw.WriteLine("<LinearGradientBrush>");
					LinearGradientBrush lgb=rec.Fill as LinearGradientBrush;
					foreach(GradientStop Gs in lgb.GradientStops) {
						sw.WriteLine(@"<GradientStop Color=""{0}"" Offset=""{1}"" />",items.NameOf(Gs.Color),Gs.Offset);
					}
					sw.WriteLine("</LinearGradientBrush>");
				} else if(rec.Fill is RadialGradientBrush) {
					sw.WriteLine("<RadialGradientBrush>");
					RadialGradientBrush rgb=rec.Fill as RadialGradientBrush;
					foreach(GradientStop Gs in rgb.GradientStops) {
						sw.WriteLine(@"<GradientStop Color=""{0}"" Offset=""{1}"" />",items.NameOf(Gs.Color),Gs.Offset);
					}
					sw.WriteLine("</RadialGradientBrush>");
				} else if(rec.Fill is SolidColorBrush) {
					SolidColorBrush scb=rec.Fill as SolidColorBrush;
					sw.WriteLine("{0}",items.NameOf(scb.Color));
				} else {
				}
				sw.WriteLine("</Rectangle.Fill>");
				sw.WriteLine("</Rectangle>");
				text=sw.ToString();
			}
			Clipboard.SetText(text);
			//StartShowMessage(String.Format("Clipboard'ed!!{0}",e.GetPosition(null).ToString()),1.0,0.1);
			StartShowMessage("Clipboard'ed!!",1.0,0.1);
		}

		void DropMenu() {
			#if false
			DispatcherTimer Menu=new DispatcherTimer();
			Menu.Interval=TimeSpan.FromSeconds(0.01);
			Menu.Tick+=new EventHandler(Menu_Tick);
			Canvas.SetTop(this.LRmenu,0.0);
			TranslateTransform tt=new TranslateTransform();
			tt.Y=Canvas.GetTop(this.LRmenu);
			this.LRmenu.RenderTransform=tt;
			Menu.Start();
			#endif
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
			this.Content=new Page();
		}
		private void goR_Click(object sender,RoutedEventArgs e) {
			this.Content=new Test_Patlite();
		}

		private void LayoutRoot_Loaded(object sender,RoutedEventArgs e) {
			DispatcherTimer slow=new DispatcherTimer();
			slow.Interval=TimeSpan.FromSeconds(1.5);
			slow.Tick+=new EventHandler(slow_Tick);
			slow.Start();
		}
		void slow_Tick(object sender,EventArgs e) {
			DispatcherTimer timer=sender as DispatcherTimer;
			timer.Stop();
			this.LRmenu.Visibility=Visibility.Visible;
			DropMenu();
		}

		bool colorTimerStopped=false;
		private void Page_KeyDown(object sender,KeyEventArgs e) {
			if(e.Key==Key.Shift){
				e.Handled=true;
				ColorsListbox.ByNumber=!ColorsListbox.ByNumber;
				listTraversed+=new ColorListTraversed(ColorfulPageOne_listTraversed);
				lastInterval=colorTimer.Interval;
				colorTimer.Interval=TimeSpan.FromMilliseconds(0.1);
				StartShowMessage(ColorsListbox.ByNumber?"By Number":"By Name",1.0,0.3);
			}else if(e.Key==Key.Escape){
				e.Handled=true;
				colorTimerStopped=!colorTimerStopped;
				if(colorTimerStopped){
					colorTimer.Stop();
				}else{
					colorTimer.Start();
				}
				StartShowMessage(colorTimerStopped?"Stopped":"Started",2.0,0.1);
			}
		}
		TimeSpan lastInterval;
		void ColorfulPageOne_listTraversed() {
			listTraversed-=new ColorListTraversed(ColorfulPageOne_listTraversed);
			listTraversed=null;
			colorTimer.Interval=lastInterval;
		}
	}
}
