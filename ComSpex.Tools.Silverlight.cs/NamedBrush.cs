// $Header: /ComSpexHome.root/ComSpexHome/Visual Studio 2010/Projects/ComSpexHome/ComSpex.Tools.Silverlight.cs/NamedBrush.cs 2     13/11/04 14:16 Yosuke $
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
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Collections;

namespace ComSpex{namespace Tools{namespace Silverlight {
	public class NamedBrush:Control {
		public static readonly DependencyProperty BrushNameProperty;
		public static readonly DependencyProperty BrushProperty;
		#if UseArray
		static NamedBrush[] nbrushes;
		#endif
		string str;

		// Static constructor.
		static NamedBrush() {
			BrushNameProperty=DependencyProperty.Register("BrushName",typeof(string),typeof(NamedBrush),new PropertyMetadata(BrushNameChangedCallback));
			BrushProperty=DependencyProperty.Register("Brush",typeof(Brush),typeof(NamedBrush),new PropertyMetadata(BrushChangedCallback));
			
			#if UseArray
			PropertyInfo[] props=typeof(Brushes).GetProperties();
			nbrushes=new NamedBrush[props.Length];
			for(int i=0;i<props.Length;i++){
				nbrushes[i]=new NamedBrush(props[i].Name,(Brush)props[i].GetValue(null,null));
			}
			#endif
		}

		public NamedBrush(){
		}
		#if UseArray
		public NamedBrush this[int index]{
			get{return All[index];}
		}
		#endif
		
		// Private constructor.
		private NamedBrush(string str,Brush brush) {
			this.str=str;
			this.Brush=brush;
			this.BrushName=Text;
		}

		#if UseArray
		// Static read-only property.
		public static NamedBrush[] All {
			get { return nbrushes; }
		}
		#endif

		// Read-only properties.
		public Brush Brush {
			get { return (Brush)GetValue(BrushProperty);}
			set { SetValue(BrushProperty,value);}
		}
		public string Text {
			get {
				if(String.IsNullOrEmpty(str)) {
					Match Ma=Regex.Match(BrushName,"[#][0-9A-Fa-f]{6}.(?<name>[A-Za-z ]+)");
					if(Ma.Success) {
						str=Ma.Groups["name"].Value;
					}
				}
				Color c=(Color)Brush.GetValue(SolidColorBrush.ColorProperty);
				string strSpaced=String.Format("#{0:X2}{1:X2}{2:X2}",c.R,c.G,c.B);
				strSpaced+=" "+str[0].ToString();
				for(int i=1;i<str.Length;i++){
					strSpaced+=(char.IsUpper(str[i])?" ":"")+str[i].ToString();
				}
				return strSpaced;
			}
		}
		public string BrushName{
			get{return (string)GetValue(BrushNameProperty);}
			set{SetValue(BrushNameProperty,value);}
		}
		// Override of ToString method.
		public override string ToString() {
			return str;
		}
		public string ClipboardName{
			get {
				string[] names=Text.Split(' ');
				string colorName=String.Join("",names,1,names.Length-1);
				return colorName;
			}
		}
		public string LegibleName{
			get{
				string[] names=Text.Split(' ');
				string colorName=String.Join(" ",names,1,names.Length-1);
				return colorName;
			}
		}
		private static void BrushNameChangedCallback(DependencyObject dob,DependencyPropertyChangedEventArgs e) {
			NamedBrush nb=dob as NamedBrush;
		}
		private static void BrushChangedCallback(DependencyObject dob,DependencyPropertyChangedEventArgs e) {
			NamedBrush nb=dob as NamedBrush;
		}
	}

	#if false
	public class NamedBrushes:ObservableCollection<NamedBrush>{
		public NamedBrushes(){
			PropertyInfo[] props=typeof(Brushes).GetProperties();
			for(int i=0;i<props.Length;i++) {
				Add(new NamedBrush(props[i].Name,(Brush)props[i].GetValue(null,null)));
			}
		}
	}
	#endif

}}}
