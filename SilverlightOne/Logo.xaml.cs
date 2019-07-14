// $Header: /ComSpexHome.root/ComSpexHome/Visual Studio 2010/Projects/ComSpexHome/SilverlightOne/Logo.xaml.cs 1     11/07/13 7:24p Yosuke $
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
	public delegate void AnimateCharacterProc(Path pa,object sender);
	public partial class Logo:UserControl {
		public static readonly DependencyProperty FpdOpacityProperty
			=DependencyProperty.Register("FpdOpacity",typeof(double),typeof(Logo),new PropertyMetadata(1.0));
		public static readonly DependencyProperty DesignModeProperty
			=DependencyProperty.Register("DesignMode",typeof(bool),typeof(Logo),new PropertyMetadata(false));
		protected Path[] chars;
		public Logo() {
			InitializeComponent();
			chars=new Path[]{Cchar,Ochar,Mchar,Schar,Pchar,Echar,Xchar};
			Point[] origins=new Point[]{
				new Point(0.132,0.378),
				new Point(0.238,0.556),
				new Point(0.362,0.556),
				new Point(0.531,0.383),
				new Point(0.657,0.639),
				new Point(0.774,0.561),
				new Point(0.898,0.528),
			};
			TagMatch<Path>.DesignMode=this.DesignMode;
			for(int i=0;i<chars.Length;++i){
				Path @char=chars[i];
				@char.Tag=new TagMatch<Path>(@char,origins[i]);
			}
			fpdBase.Opacity=0;
		}
		private void Report(string format,params object[] args) {
			System.Diagnostics.Debug.WriteLine(String.Format(format,args),DateTime.Now.ToString("HH:mm:ss.fff"));
		}
		public void StartAnimation(){
			IsStopped=false;
			foreach(Path @char in chars){
				((TagMatch<Path>)@char.Tag).Stopping=false;
				((TagMatch<Path>)@char.Tag).Timer.Start();
			}
			fpdBase.Opacity=1.0;
		}
		int index=0;
		public bool IsStopped{get;set;}
		public void StopAnimation(){
			DispatcherTimer Stopper=new DispatcherTimer();
			Stopper.Tick+=new EventHandler(Stopper_Tick);
			Stopper.Interval=TimeSpan.FromSeconds(1);
			index=-1;
			Stopper.Start();
		}
		bool isStopCompleted{
			get{
				foreach(Path @char in chars) {
					if(!((TagMatch<Path>)@char.Tag).Stopped) {
						return false;
					}
				}
				return true;
			}
		}
		void Stopper_Tick(object sender,EventArgs e) {
			DispatcherTimer timer=sender as DispatcherTimer;
			if(isStopCompleted){
				timer.Stop();
				fpdBase.Opacity=0.0;
			}
			if(++index>=chars.Length){
				IsStopped=true;
				return;
			}
			((TagMatch<Path>)chars[index].Tag).Stopping=true;
			((TagMatch<Path>)chars[index].Tag).Timer.Start();
		}
		public double FpdOpacity{
			get{return (double)GetValue(FpdOpacityProperty);}
			set{
				SetValue(FpdOpacityProperty,value);
				fpd.Opacity=
				fpdSlit.Opacity=value;
			}
		}
		public bool DesignMode{
			get{return (bool)GetValue(DesignModeProperty);}
			set{
				SetValue(DesignModeProperty,value);
				TagMatch<Path>.DesignMode=value;
			}
		}
		private void workShop_MouseMove(object sender,MouseEventArgs e) {
			if(DesignMode){
				Point p=e.GetPosition(workShop);
				horiHair.Y1=
				horiHair.Y2=p.Y;
				vertHair.X1=
				vertHair.X2=p.X;
				posX.Text=String.Format("{0}:{1:0.000}",p.X.ToString(),p.X/workShop.ActualWidth);
				posY.Text=String.Format("{0}:{1:0.000}",p.Y.ToString(),p.Y/workShop.ActualHeight);
				
				posXPos.X=p.X-posX.ActualWidth/2;
				posXPos.Y=-posX.ActualHeight;
				posYPos.X=-posY.ActualWidth;
				posYPos.Y=p.Y-posY.ActualHeight/2;
			}
		}
		public void AnimationCompleted(){
			if(DesignMode){
				horiHair.X2=workShop.ActualWidth;
				vertHair.Y2=workShop.ActualHeight;
			}
		}
	}
}
