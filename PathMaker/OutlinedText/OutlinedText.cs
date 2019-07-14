// $Header: /Greetings.root/Greetings/OutlinedText/OutlinedText.cs 14    13/02/02 16:29 Yosuke $
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using System.Xml;
using System.IO;
using System.Windows.Markup;
using System.Reflection;

namespace ComSpex {
	/// <summary>
	/// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
	///
	/// Step 1a) Using this custom control in a XAML file that exists in the current project.
	/// Add this XmlNamespace attribute to the root element of the markup file where it is 
	/// to be used:
	///
	///     xmlns:MyNamespace="clr-namespace:OutlinedText"
	///
	///
	/// Step 1b) Using this custom control in a XAML file that exists in a different project.
	/// Add this XmlNamespace attribute to the root element of the markup file where it is 
	/// to be used:
	///
	///     xmlns:MyNamespace="clr-namespace:OutlinedText;assembly=OutlinedText"
	///
	/// You will also need to add a project reference from the project where the XAML file lives
	/// to this project and Rebuild to avoid compilation errors:
	///
	///     Right click on the target project in the Solution Explorer and
	///     "Add Reference"->"Projects"->[Select this project]
	///
	///
	/// Step 2)
	/// Go ahead and use your control in the XAML file.
	///
	///     <MyNamespace:CustomControl1/>
	///
	/// </summary>
	public class OutlinedText:Control {
		public static readonly DependencyProperty FillProperty;
		public static readonly DependencyProperty HighlightProperty;
		public static readonly DependencyProperty StrokeProperty;
		public static readonly DependencyProperty StrokeThicknessProperty;
		public static readonly DependencyProperty TextProperty;
		public static readonly DependencyProperty CultureProperty;
		static OutlinedText() {
			DefaultStyleKeyProperty.OverrideMetadata(typeof(OutlinedText),new FrameworkPropertyMetadata(typeof(OutlinedText)));
			{
				FrameworkPropertyMetadata meta=new FrameworkPropertyMetadata();
				meta.DefaultValue=Brushes.Transparent;
				meta.AffectsMeasure=false;
				meta.AffectsRender=true;
				meta.Inherits=true;
				meta.DefaultUpdateSourceTrigger=UpdateSourceTrigger.PropertyChanged;
				FillProperty=DependencyProperty.Register("Fill",typeof(Brush),typeof(OutlinedText),meta);
			}
			HighlightProperty=DependencyProperty.Register("Highlight",typeof(bool),typeof(OutlinedText));
			{
				FrameworkPropertyMetadata meta=new FrameworkPropertyMetadata();
				meta.DefaultValue=Brushes.Black;
				meta.AffectsMeasure=
				meta.AffectsRender=true;
				meta.Inherits=true;
				meta.DefaultUpdateSourceTrigger=UpdateSourceTrigger.PropertyChanged;
				StrokeProperty=DependencyProperty.Register("Stroke",typeof(Brush),typeof(OutlinedText),meta);
			}
			{
				FrameworkPropertyMetadata meta=new FrameworkPropertyMetadata();
				meta.DefaultValue=1.0;
				meta.AffectsMeasure=
				meta.AffectsRender=true;
				meta.Inherits=true;
				meta.DefaultUpdateSourceTrigger=UpdateSourceTrigger.PropertyChanged;
				StrokeThicknessProperty=DependencyProperty.Register("StrokeThickness",typeof(double),typeof(OutlinedText),meta);
			}
			{
				FrameworkPropertyMetadata meta=new FrameworkPropertyMetadata();
				meta.DefaultValue="Outlined Text";
				meta.AffectsMeasure=
				meta.AffectsRender=true;
				meta.Inherits=true;
				meta.DefaultUpdateSourceTrigger=UpdateSourceTrigger.PropertyChanged;
				TextProperty=DependencyProperty.Register("Text",typeof(string),typeof(OutlinedText),meta);
			}
			{
				FrameworkPropertyMetadata meta=new FrameworkPropertyMetadata();
				meta.DefaultValue="en-US";
				meta.AffectsMeasure=false;
				meta.AffectsRender=true;
				meta.Inherits=true;
				meta.DefaultUpdateSourceTrigger=UpdateSourceTrigger.PropertyChanged;
				CultureProperty=DependencyProperty.Register("Culture",typeof(string),typeof(OutlinedText),meta);
			}
		}
		
		// Class variables
		private FormattedText _formattedText;

		// Property variables
		private Geometry _textGeometry;
		private Geometry _textHighLightGeometry;

		public OutlinedText(){
		}
		public OutlinedText(string textContent) {
			// Set the default property values.
			FontFamily=new FontFamily("Verdana");
			FontStretch=FontStretches.Normal;
			FontWeight=FontWeights.Bold;
			FontSize=72;
			Text=textContent;
		}

		// Create the outline geometry based on the formatted text.
		public void CreateText() {
			// Create the formatted text based on the properties set.
			_formattedText=new FormattedText(
					Text,
					CultureInfo.GetCultureInfo(Culture),
					FlowDirection.LeftToRight,
					new Typeface(FontFamily,FontStyle,FontWeight,FontStretch),
					FontSize,
					Brushes.Black // This brush does not matter since we use the geometry of the text. 
					);

			// Build the geometry object that represents the text.
			_textGeometry=_formattedText.BuildGeometry(new Point(0,0));

			// Build the geometry object that represents the text hightlight.
			if(Highlight) {
				_textHighLightGeometry=_formattedText.BuildHighlightGeometry(new Point(0,0));
			}
		}

		// The OnRender override method allows you to draw directly into
		// the DrawingContext of the outlined text control.
		protected override void OnRender(DrawingContext drawingContext) {
			// Draw the outline based on the properties that are set.
			drawingContext.DrawGeometry(Fill,new Pen(Stroke,StrokeThickness),_textGeometry);

			// Draw the text highlight based on the properties that are set.
			if(Highlight) {
				drawingContext.DrawGeometry(Fill,new Pen(Stroke,StrokeThickness),_textHighLightGeometry);
			}
		}

		// The ArrangeOverride and MeasureOverride methods provide layout support for the control.
		//  - ArrangeOverride: Positions the object and determines size. 
		//  - MeasureOverride: Measures and determines the size in layout required for the object.

		// No calculation required -- return the value of the parameter.
		protected override Size ArrangeOverride(Size finalSize) {
			return finalSize;
		}

		// Return the size of the formatted text.
		protected override Size MeasureOverride(Size availableSize) {
			CreateText();
			return new Size(_formattedText.Width,_formattedText.Height);
		}

		// Definitions for the outlined text control properties.
		public Brush Fill {
			get { return (Brush)GetValue(FillProperty); }
			set { SetValue(FillProperty,value); }
		}
		public bool Highlight {
			get { return (bool)GetValue(HighlightProperty);}
			set { SetValue(HighlightProperty,value);}
		}
		public Brush Stroke {
			get { return (Brush)GetValue(StrokeProperty);}
			set { SetValue(StrokeProperty,value);}
		}
		public double StrokeThickness {
			get { return (double)GetValue(StrokeThicknessProperty);}
			set { SetValue(StrokeThicknessProperty,value);}
		}
		public string Text {
			get { return (string)GetValue(TextProperty);}
			set { SetValue(TextProperty,value);}
		}
		public string Culture{
			get{return (string)GetValue(CultureProperty);}
			set{SetValue(CultureProperty,value);}
		}
		// The TextGeometry property allows you to get and set the geometry of the outlined text.
		// The returned value can be used to clip images, video, etc.
		public Geometry TextGeometry {
			get {
				CreateText();
				return _textGeometry;
			}
			set { _textGeometry=value; }
		}
		protected virtual string filterSymbols(string text) {
			Dictionary<string,string> dics=new Dictionary<string,string>();
			dics.Add("~","Tilda");
			dics.Add("!","Excla");
			dics.Add("@","Atmar");
			dics.Add("#","Hashi");
			dics.Add("$","Dolla");
			dics.Add("%","Perce");
			dics.Add("^","Caret");
			dics.Add("&","Amper");
			dics.Add("*","Aster");
			dics.Add("(","LPare");
			dics.Add(")","RPare");
			dics.Add("_","Under");
			dics.Add("-","Minus");
			dics.Add("+","Pluss");
			dics.Add("=","Equal");
			dics.Add("{","LBrac");
			dics.Add("}","RBrac");
			dics.Add("[","LSBra");
			dics.Add("]","RSBra");
			dics.Add("|","Verti");
			dics.Add("\\","Backs");
			dics.Add(":","Colon");
			dics.Add(";","Semic");
			dics.Add("\"","Quote");
			dics.Add("'","Singl");
			dics.Add("<","LAngl");
			dics.Add(">","RAngl");
			dics.Add(",","Comma");
			dics.Add(".","Perio");
			dics.Add("?","Quest");
			dics.Add("/","Slash");
			foreach(KeyValuePair<string,string> pair in dics) {
				text=text.Replace(pair.Key,pair.Value);
			}
			return text;
		}
		protected virtual string XamlForSilverlight{
			get{
				OutlinedText textRequested=this;
				string text=XamlWriter.Save(textRequested);
				text=text.Replace("av:","");
				XmlDocument xd=new XmlDocument();
				xd.LoadXml(text);
				XmlNodeList data=xd.GetElementsByTagName("PathGeometry");
				using(StringWriter sw=new StringWriter()) {
					string textName=filterSymbols(textRequested.Text.Replace(" ",""));
					sw.WriteLine("<Grid>");
					sw.WriteLine("<Grid.Resources>");
					sw.WriteLine(@"<Style TargetType=""Path"" x:Key=""{0}"">",textName);
					sw.WriteLine(@"<Setter Property=""Fill"" Value=""{0}"" />",textRequested.Fill);
					sw.WriteLine(@"<Setter Property=""Stroke"" Value=""{0}"" />",textRequested.Stroke);
					sw.WriteLine(@"<Setter Property=""StrokeThickness"" Value=""{0}"" />",textRequested.StrokeThickness);
					sw.WriteLine(@"</Style>");
					for(int i=0;i<data.Count;++i) {
						sw.WriteLine(@"<Style TargetType=""Path"" x:Key=""style_{0}{2}"" BasedOn=""{{StaticResource {1}}}""></Style>",textName[i],textName,i);
					}
					sw.WriteLine("</Grid.Resources>");
					for(int i=0;i<data.Count;++i) {
						XmlNode node=data[i];
						XmlAttribute xatt=node.Attributes["Figures"];
						sw.WriteLine(@"<Path Style=""{{StaticResource style_{0}{2}}}"" Data=""{1}"" />",textName[i],xatt.Value,i);
					}
					sw.WriteLine(@"<Grid.RenderTransform>");
					sw.WriteLine(@"<TransformGroup>");
					sw.WriteLine(@"<TranslateTransform X=""0"" Y=""0"" />");
					sw.WriteLine(@"</TransformGroup>");
					sw.WriteLine(@"</Grid.RenderTransform>");
					sw.WriteLine("</Grid>");
					return sw.ToString();
				}
			}
		}
		protected virtual string XamlForWPF{
			get{
				OutlinedText textRequested=this;
				string text=XamlWriter.Save(textRequested);
				text=text.Replace("av:","");
				XmlDocument xd=new XmlDocument();
				xd.LoadXml(text);
				XmlNode data=xd.GetElementsByTagName("OutlinedText.TextGeometry")[0];
				using(StringWriter sw=new StringWriter()) {
					string xaml=data.InnerXml;
					xaml=xaml.Replace(String.Format(@" xmlns=""{0}""",data.NamespaceURI),"");
					sw.WriteLine("<!-- {0} -->",textRequested.Text);
					sw.WriteLine(@"<Path Fill=""{0}"" Stroke=""{1}"" StrokeThickness=""{2}"">",textRequested.Fill,textRequested.Stroke,textRequested.StrokeThickness);
					sw.WriteLine("<Path.Data>{0}</Path.Data>",xaml);
					sw.WriteLine("</Path>");
					return sw.ToString();
				}
			}
		}
		public string ColorByName(Brush brush){
			if(brush is SolidColorBrush){
				SolidColorBrush rhs=brush as SolidColorBrush;
				Type type=typeof(Brushes);
				PropertyInfo[] props=type.GetProperties(BindingFlags.Static|BindingFlags.Public);
				foreach(PropertyInfo prop in props){
					//System.Diagnostics.Debug.WriteLine(prop.Name);
					SolidColorBrush lhs=prop.GetValue(prop,null) as SolidColorBrush;
					if(rhs.Color==lhs.Color){
						return prop.Name;
					}
				}
			}
			return brush.ToString();
		}
		public override string ToString() {
			//<OutlinedText Fill="#FFF5F5F5" Stroke="#FF000000" Text="White Smoke" xmlns="clr-namespace:ComSpex;assembly=OutlinedText" />
			string text=String.Empty;
			using(StringWriter sw=new StringWriter()){
				sw.Write(@" Fill=""{0}""",ColorByName(this.Fill));
				sw.Write(@" Stroke=""{0}""",ColorByName(this.Stroke));
				sw.Write(@" StrokeThickness=""{0}""",this.StrokeThickness);
				sw.Write(@" FontSize=""{0}""",this.FontSize);
				sw.Write(@" FontWeight=""{0}""",this.FontWeight);
				sw.Write(@" FontStretch=""{0}""",this.FontStretch);
				sw.Write(@" FontStyle=""{0}""",this.FontStyle);
				sw.Write(@" FontFamily=""{0}""",this.FontFamily);
				sw.Write(@" Text=""{0}""",this.Text);
				string xaml=XamlWriter.Save(this);
				xaml=xaml.Replace("av:","");
				XmlDocument xd=new XmlDocument();
				xd.LoadXml(xaml);
				XmlNode data=xd.GetElementsByTagName("OutlinedText.TextGeometry")[0];
				sw.Write(@" xmlns=""{0}""",data.NamespaceURI);
				text=String.Format("<OutlinedText {0} />\n",sw.ToString());
			}
			return text;
		}
		public virtual string ToXAML(bool isWpf){
			string xaml=String.Empty;
			if(isWpf){
				xaml=XamlForWPF;
			}else{
				xaml=XamlForSilverlight;
			}
			return xaml;
		}
		public string ToXAML() {
			return ToXAML(true);
		}
	}
}
