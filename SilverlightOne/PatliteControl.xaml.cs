// $Header: /ComSpexHome.root/ComSpexHome/Visual Studio 2010/Projects/ComSpexHome/SilverlightOne/PatliteControl.xaml.cs 1     11/07/13 7:24p Yosuke $
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
using ComSpex.Tools.Silverlight;

//http://samples.msdn.microsoft.com/Silverlight/SampleBrowser/index.htm#/?sref=objectanimationusingkeyframes
//http://samples.msdn.microsoft.com/Silverlight/SampleBrowser/sourcewindow.htm

namespace SilverlightOne {
	public partial class PatliteControl:UserControl {
		public PatliteControl() {
			InitializeComponent();

			Scales.Add("rEnv",rScale);
			Scales.Add("yEnv",yScale);
			Scales.Add("gEnv",gScale);
		}
		Dictionary<string,ScaleTransform> Scales=new Dictionary<string,ScaleTransform>();
		Dictionary<FrameworkElement,Storyboard> Stories=new Dictionary<FrameworkElement,Storyboard>();
		protected bool _red { get; set; }
		protected bool _yellow { get; set; }
		protected bool _green { get; set; }
		public bool Red {
			get { return _red; }
			set {
				_red=value;
				if(value) {
					rGrid.Background=/*Brushes.Crimson;*/
					rEnv.Fill=Resources["R1"] as Brush;
					FlashLamp(Scales[rEnv.Name]);
				} else {
					rGrid.Background=
					rEnv.Fill=Brushes.Transparent;
				}
			}
		}
		public bool Yellow {
			get { return _yellow; }
			set {
				_yellow=value;
				if(value) {
					yGrid.Background=/*Brushes.Gold;*/
					yEnv.Fill=Resources["Y1"] as Brush;
					FlashLamp(Scales[yEnv.Name]);
				} else {
					yGrid.Background=
					yEnv.Fill=Brushes.Transparent;
				}
			}
		}
		public bool Green {
			get { return _green; }
			set {
				_green=value;
				if(value) {
					gGrid.Background=/*Brushes.Lime;*/
					gEnv.Fill=Resources["G1"] as Brush;
					FlashLamp(Scales[gEnv.Name]);
				} else {
					gGrid.Background=
					gEnv.Fill=Brushes.Transparent;
				}
			}
		}

		public void SetEnabled(bool flag) {
			this.Red=this.Yellow=this.Green=flag;
		}
		public void Off() {
			SetEnabled(false);
		}
		public void On() {
			SetEnabled(true);
		}
		private void FlashLamp(ScaleTransform scaler) {
			DoubleAnimation dax=new DoubleAnimation();
			DoubleAnimation day=new DoubleAnimation();
			//dax.From=day.From=1.0;
			dax.To=day.To=15.0;
			dax.Duration=day.Duration=new Duration(TimeSpan.FromSeconds(0.15));
			dax.FillBehavior=day.FillBehavior=FillBehavior.Stop;
			Storyboard.SetTarget(dax,scaler);
			Storyboard.SetTarget(day,scaler);
			Storyboard.SetTargetProperty(dax,new PropertyPath("(ScaleTransform.ScaleX)"));
			Storyboard.SetTargetProperty(day,new PropertyPath("(ScaleTransform.ScaleY)"));
			Storyboard story=new Storyboard();
			story.Children.Add(dax);
			story.Children.Add(day);
			story.Begin();

			scaler.ScaleX=1.5;
			scaler.ScaleY=1.3;
		}
		private void BlinkLamp(FrameworkElement elem,bool value) {
#if false
			Storyboard story=Stories[elem];
			if(value) {
				story.Begin(elem,true);
			} else {
				story.Stop(elem);
			}
#endif
		}
	}
}
