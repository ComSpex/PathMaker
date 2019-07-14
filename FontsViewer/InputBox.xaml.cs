// $Header: /ComSpexHome.root/ComSpexHome/Visual Studio 2010/Projects/ComSpexHome/FontsViewer/InputBox.xaml.cs 1     11/07/13 7:23p Yosuke $
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
using System.Windows.Shapes;

namespace FontsViewer {
	/// <summary>
	/// Interaction logic for InputBox.xaml
	/// </summary>
	public partial class InputBox:Window {
		TextBlock target;
		public InputBox() {
			InitializeComponent();
		}
		public InputBox(TextBlock block,Window boss)
			:this(){
			target=block;
			this.samplet.Text=block.Text;
			this.Owner=boss;
		}
		private void Button_Click(object sender,RoutedEventArgs e) {
			FireButton_Click();
		}
		private void samplet_KeyDown(object sender,KeyEventArgs e) {
			if(e.Key==Key.Enter){
				FireButton_Click();
			}
		}
		void FireButton_Click() {
			this.DialogResult=!String.IsNullOrEmpty(this.samplet.Text);
			if(this.DialogResult.Value) {
				target.Text=this.samplet.Text;
			}
			this.Close();
		}
		private void samplet_Loaded(object sender,RoutedEventArgs e) {
			this.samplet.Focus();
			this.samplet.SelectAll();
		}
		private void Window_PreviewKeyDown(object sender,KeyEventArgs e) {
			if(e.Key==Key.Escape){
				this.Close();
			}
		}
		private void Window_Loaded(object sender,RoutedEventArgs e) {
			double diffW=this.ActualWidth-this.samplet.ActualWidth;
			double width=target.ActualWidth+diffW-okay.ActualWidth;
			this.Width=width;
			this.Top=this.Owner.Top;
			this.Left=this.Owner.Left+this.Owner.ActualWidth-this.ActualWidth;
		}
	}
}
