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

namespace SilverlightOne {
	public partial class ButtonContentOne:UserControl {
		public ButtonContentOne() {
			InitializeComponent();
			this.IsRight=true;
		}
		protected override Size ArrangeOverride(Size finalSize) {
			double cx=finalSize.Width/18.0;
			double cy=finalSize.Height/24.0;
			//this.LorR.ScaleX=cx;
			//this.LorR.ScaleY=cy;
			if(IsRight) {
				this.mover.X=0;
			}else{
				this.mover.X=finalSize.Width;
			}
			return base.ArrangeOverride(finalSize);
		}
		public static readonly DependencyProperty IsRightProperty=
				DependencyProperty.Register("IsRight",typeof(bool),typeof(ButtonContentOne),
												new PropertyMetadata(new PropertyChangedCallback(IsRightChangedCallback)));
		private static void IsRightChangedCallback(DependencyObject obj,DependencyPropertyChangedEventArgs args) {
			ButtonContentOne ctl=obj as ButtonContentOne;
			bool isRight=(bool)args.NewValue;
			if(isRight){
				ctl.LorR.ScaleX=1.0;
				ctl.mover.X=0;
			}else{
				ctl.LorR.ScaleX=-1.0;
				ctl.mover.X=ctl.ActualWidth;
			}
		}
		public bool IsRight {
			get { return (bool)GetValue(IsRightProperty); }
			set { SetValue(IsRightProperty,value); }
		}
	}
}
