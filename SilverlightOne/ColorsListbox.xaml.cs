// $Header: /ComSpexHome.root/ComSpexHome/Visual Studio 2010/Projects/ComSpexHome/SilverlightOne/ColorsListbox.xaml.cs 2     11/08/13 5:16p Yosuke $
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
using System.Reflection;
using System.Collections.ObjectModel;
using ComSpex.Tools.Silverlight;

namespace SilverlightOne {
	public class ColorItem {
		public ColorItem(string name,Brush brush) {
			Name=name;
			Brush=brush;
		}
		string _name;
		public string Name { 
			get{
				if(ColorsListbox.ByNumber){
					SolidColorBrush solid=Brush as SolidColorBrush;
					return solid.Color.ToString();
				}
				return _name;
			}
			set{_name=value;}
		}
		public Brush Brush { get; set; }
	}
	public class ColorItems:ObservableCollection<ColorItem> {
		public ColorItems() {
			PropertyInfo[] props=typeof(Brushes).GetProperties();
			for(int i=0;i<props.Length;i++) {
				Add(new ColorItem(props[i].Name,(Brush)props[i].GetValue(null,null)));
			}
		}
		public string NameOf(Color color){
			foreach(ColorItem item in this){
				SolidColorBrush solid=item.Brush as SolidColorBrush;
				if(solid.Color==color){
					return item.Name;
				}
			}
			return String.Empty;
		}
	}
	public partial class ColorsListbox:UserControl {
		public event SelectionChangedEventHandler SelectionChanged;
		public static readonly DependencyProperty SelectedItemProperty=
				DependencyProperty.Register("SelectedItem",typeof(ColorItem),typeof(ColorsListbox),
												new PropertyMetadata(new PropertyChangedCallback(SelectedItemChangedCallback)));
		private static void SelectedItemChangedCallback(DependencyObject owner,DependencyPropertyChangedEventArgs args) {
			ColorsListbox clb=owner as ColorsListbox;
			ColorItem item=args.NewValue as ColorItem;
			clb.SelectedItem=item;
		}
		public static readonly DependencyProperty SelectedIndexProperty=
				DependencyProperty.Register("SelectedIndex",typeof(int),typeof(ColorsListbox),
												new PropertyMetadata(new PropertyChangedCallback(SelectedIndexChangedCallback)));
		private static void SelectedIndexChangedCallback(DependencyObject owner,DependencyPropertyChangedEventArgs args) {
			ColorsListbox clb=owner as ColorsListbox;
			int index=(int)args.NewValue;
			clb.SelectedIndex=index;
		}
		public ColorsListbox() {
			InitializeComponent();
		}
		static public volatile bool ByNumber;
		public ColorItem SelectedItem {
			get { return (ColorItem)GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty,value); }
		}
		public int SelectedIndex {
			get { return (int)GetValue(SelectedIndexProperty); }
			set { 
				SetValue(SelectedIndexProperty,value); 
				this.List.SelectedIndex=value;
				this.List.ScrollIntoView(this.List.Items[value]);
			}
		}
		private void List_SelectionChanged(object sender,SelectionChangedEventArgs e) {
			ListBox listBox=sender as ListBox;
			SelectedItem=listBox.SelectedItem as ColorItem;
			SelectedIndex=listBox.SelectedIndex;
			if(SelectionChanged!=null){
				SelectionChanged(sender,e);
			}
		}
		public int Count { 
			get{
				return this.List.Items.Count;
			}
		}
	}
}
