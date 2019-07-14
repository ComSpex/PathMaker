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
	/*
    
			<this:Framework x:Key="dt" />

			<!-- Multiply Hour by 30 degrees and Minute by 0.5 degrees
									and add. Result is stored in angleHour.Value.OffsetX. -->
			<TransformGroup x:Key="angleHour">
				<TranslateTransform
									X="{Binding Source={StaticResource dt}, Path=Tag.Hour}"
									Y="{Binding Source={StaticResource dt}, Path=Tag.Minute}" />
				<MatrixTransform Matrix="30 0 0.5 1 0 0" />
			</TransformGroup>

			<!-- Multiply Minute by 6 degrees and Second by 0.1 degrees
									and add. Result is stored in angleMinute.Value.OffsetX. -->
			<TransformGroup x:Key="angleMinute">
				<TranslateTransform 
									X="{Binding Source={StaticResource dt}, Path=Tag.Minute}"
									Y="{Binding Source={StaticResource dt}, Path=Tag.Second}" />
				<MatrixTransform Matrix="6 0 0.1 1 0 0" />
			</TransformGroup>

			<!-- Multiply Second by 6 degrees. Result is angleSecond.Value.M11. -->
			<TransformGroup x:Key="angleSecond">
				<ScaleTransform 
									ScaleX="{Binding Source={StaticResource dt}, Path=Tag.Second}" />
				<ScaleTransform ScaleX="6" />
			</TransformGroup>
	 */
	public partial class Clock:UserControl {
		DispatcherTimer clock;
		public Clock() {
			clock=new DispatcherTimer();
			clock.Interval=TimeSpan.FromSeconds(1);
			clock.Tick+=new EventHandler(clock_Tick);
			InitializeComponent();
			this.Loaded+=new RoutedEventHandler(Clock_Loaded);
		}
		double Modulo(double value){
			return value;
		}
		void Clock_Loaded(object sender,RoutedEventArgs e) {
			//goToMondaine.NavigateUri=new Uri("http://www.mondaine.com/mondaine/");
			clock.Start();
		}
		void clock_Tick(object sender,EventArgs e) {
			UpdateTargets(DateTime.Now);
		}
		TransformGroup Hour(DateTime time){
			TranslateTransform H=new TranslateTransform();
			H.X=time.Hour;
			H.Y=time.Minute;
			MatrixTransform mH=new MatrixTransform();
			mH.Matrix=new Matrix(30,0,0.5,1,0,0);
			TransformGroup tgH=new TransformGroup();
			tgH.Children.Add(H);
			tgH.Children.Add(mH);
			return tgH;
		}
		TransformGroup Minute(DateTime time){
			TranslateTransform M=new TranslateTransform();
			M.X=time.Minute;
			M.Y=time.Second;
			MatrixTransform mM=new MatrixTransform();
			mM.Matrix=new Matrix(6,0,0.1,1,0,0);
			TransformGroup tgM=new TransformGroup();
			tgM.Children.Add(M);
			tgM.Children.Add(mM);
			return tgM;
		}
		TransformGroup Second(DateTime time){
			ScaleTransform S0=new ScaleTransform();
			S0.ScaleX=time.Second;
			ScaleTransform S1=new ScaleTransform();
			S1.ScaleX=6.0;
			TransformGroup tgS=new TransformGroup();
			tgS.Children.Add(S0);
			tgS.Children.Add(S1);
			return tgS;
		}
		void UpdateTargets(DateTime time){
			this.xformHour.Angle=Modulo(Hour(time).Value.OffsetX);
			this.xformMinute.Angle=Modulo(Minute(time).Value.OffsetX);
			this.xformSecond.Angle=Modulo(Second(time).Value.M11);
		}
		
		void Report(string format,params object[] args){
			string text=String.Format(format,args);
			System.Diagnostics.Debug.WriteLine(text);
		}
	}
}
