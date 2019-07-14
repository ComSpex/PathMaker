// $Header: /ComSpexHome.root/ComSpexHome/Visual Studio 2010/Projects/ComSpexHome/SilverlightOne/ChildWinOne.xaml.cs 1     11/16/13 8:58a Yosuke $
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
	public partial class ChildWinOne:ChildWindow {
		public ChildWinOne() {
			InitializeComponent();
		}
		private void OKButton_Click(object sender,RoutedEventArgs e) {
			this.DialogResult=true;
		}
	}
}
