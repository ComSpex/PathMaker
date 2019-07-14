// $Header: /ComSpexHome.root/ComSpexHome/Visual Studio 2010/Projects/ComSpexHome/SilverlightOne/TagMatch.cs 1     11/07/13 7:24p Yosuke $
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SilverlightOne {
	public class TagMatch<T> where T:UIElement {
		public static volatile bool DesignMode;
		protected int ZIndex;
		public DispatcherTimer Timer;
		public T Parent;
		protected bool stopping;
		public bool Stopped;
		public bool Stopping {
			get { return stopping; }
			set {
				stopping=value;
				if(!stopping) {
					Stopped=false;
				}
			}
		}
		public TagMatch(T owner)
			: this(new DispatcherTimer(),owner) {
				Parent.MouseEnter+=new MouseEventHandler(Parent_MouseEnter);
				Parent.MouseLeave+=new MouseEventHandler(Parent_MouseLeave);
				Timer.Tick+=new EventHandler(Timer_Tick);
		}
		public TagMatch(T owner,Point origin)
			: this(owner) {
			Parent.RenderTransformOrigin=origin;
			TransformGroup tg=new TransformGroup();
			tg.Children.Add(new ScaleTransform());
			tg.Children.Add(new RotateTransform());
			tg.Children.Add(new TranslateTransform());
			tg.Children.Add(new SkewTransform());
			Parent.RenderTransform=tg;
			ZIndex=Canvas.GetZIndex(Parent);
		}
		void Parent_MouseEnter(object sender,MouseEventArgs e) {
			if(DesignMode){
				return;
			}
			TransformGroup Tg=Parent.RenderTransform as TransformGroup;
			ScaleTransform Sca=Tg.Children[0] as ScaleTransform;
			RotateTransform Rot=Tg.Children[1] as RotateTransform;
			Sca.ScaleX=Sca.ScaleY=4.0;
			Canvas.SetZIndex(Parent,10);
		}
		void Parent_MouseLeave(object sender,MouseEventArgs e) {
			if(DesignMode) {
				return;
			}
			DispatcherTimer Back=new DispatcherTimer();
			Back.Interval=TimeSpan.FromMilliseconds(5);
			Back.Tick+=new EventHandler(Back_Tick);
			Back.Start();
			Timer.Interval=TimeSpan.FromMilliseconds(5);
			Timer.Start();
		}
		public TagMatch(DispatcherTimer dt) {
			Timer=dt;
		}
		public TagMatch(DispatcherTimer dt,T owner)
			: this(dt) {
			Parent=owner;
		}
		void Timer_Tick(object sender,EventArgs e) {
			if(DesignMode) {
				return;
			}
			ScaleTransform st=((TransformGroup)Parent.RenderTransform).Children[0] as ScaleTransform;
			RotateTransform rt=((TransformGroup)Parent.RenderTransform).Children[1] as RotateTransform;
			double angle=rt.Angle;
			bool stopping=Stopping;
			if(stopping) {
				angle+=10;
			} else {
				angle-=10;
			}
			rt.Angle=angle;
			if(stopping) {
				//Report("{0}={1}",pa.Name,Convert.ToInt32(angle));
				if(Convert.ToInt32(angle)%720==0) {
					Stopped=true;
					Timer.Stop();
				}
			} else {
				if(Convert.ToInt32(angle)%1125==0) {
					Stopping=true;
				}
			}
		}
		void Back_Tick(object sender,EventArgs e) {
			TransformGroup tg=Parent.RenderTransform as TransformGroup;
			ScaleTransform Sca=tg.Children[0] as ScaleTransform;
			RotateTransform Rot=tg.Children[1] as RotateTransform;
			Sca.ScaleX=Sca.ScaleY-=0.1;
			if(Sca.ScaleX<=1.0) {
				Sca.ScaleX=Sca.ScaleY=1.0;
				((DispatcherTimer)sender).Stop();
				Canvas.SetZIndex(Parent,ZIndex);
			}
		}
	}
}
