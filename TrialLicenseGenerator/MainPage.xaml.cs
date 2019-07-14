// $Header: /Silverlight/SilverlightOne.root/SilverlightOne/TrialLicenseGenerator/MainPage.xaml.cs 1     10/02/06 15:59 Yosuke $
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
using System.Xml;
using System.Text;
using System.IO;

namespace TrialLicenseGenerator {
	public partial class MainPage:UserControl {
		public MainPage() {
			InitializeComponent();
			dateFrom.SelectedDate=DateTime.Today;
			dateFrom.DisplayDateStart=DateTime.Today;
			dateUpto.SelectedDate=DateTime.Today.AddDays(14);
			dateUpto.DisplayDateEnd=DateTime.Today.AddMonths(3);
		}
		private void Button_Click(object sender,RoutedEventArgs e) {
			MessageBox.Show(
				"WebService and WebRequest are both prohibited to execute\n"+
				"so you can't use Silverlight for this purpose.\n\n"+
				"As for Silverlight, you have to write code first before\n"+
				"you will have finished GUI design...","A Lesson from Silverlight 3",
				MessageBoxButton.OK
			);
		}
	}
}
