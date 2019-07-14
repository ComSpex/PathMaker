// $Header: /ComSpexHome.root/ComSpexHome/Visual Studio 2010/Projects/ComSpexHome/ContractsPrimer/HowToContract.cs 1     11/07/13 7:23p Yosuke $
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ContractsPrimer {
	public class License {
		[XmlAttribute]
		public string Name{get;set;}
		[XmlText]
		public string Description{get;set;}
		public License(){}
		public License(string name)
			:this(name,String.Empty){
		}
		public License(string name,string desc)
			:this(){
			Name=name;
			Description=desc;
		}
	}
	public class Contract{
		public List<License> Licenses;
		[XmlAttribute]
		public string Name { get; set; }
		[XmlText]
		public string Description { get; set; }
		public Contract(){
			Licenses=new List<License>();
		}
		public Contract(string name,License[] list)
			:this(name,String.Empty,list){
		}
		public Contract(string name,string desc,License[] list)
			:this(){
			Name=name;
			Description=desc;
			Licenses.AddRange(list);
		}
	}
	public class ContractTypes:IDisposable{
		public List<Contract> Contracts;
		public ContractTypes(){
			Contracts=new List<Contract>();
			Defines();
		}
		virtual protected void Defines(){
			Contracts.Add(
				new Contract("請負契約",
				new License[]{
					new License("著作権譲渡契約","Ａは、Ｂの委託により、Ｂの指揮命令に服さずにプログラムを開発し、開発プログラムに係る著作権をＢに移転する。"),
					new License("複製許諾契約",　"Ａは、Ｂの委託により、Ｂの指揮命令に服さずにプログラムを開発するが、開発プログラムに係る著作権をＢに移転せず、一定範囲で当該プログラムの複製を許諾する。"),
					new License("貸与許諾契約",　"Ａは、Ｂの委託により、Ｂの指揮命令に服さずにプログラムを開発するが、開発プログラムに係る著作権をＢに移転せず、当該プログラムの公衆への貸与を許諾する。"),
					new License("使用許諾契約",　"Ａは、Ｂの委託により、Ｂの指揮命令に服さずにプログラムを開発するが、開発プログラムに係る著作権をＢに移転せず、当該プログラムの複製及び貸与を許諾せず、使用のみを許諾する。")
				})
			);
			Contracts.Add(
				new Contract("準委任契約",
				new License[]{
					new License("基本設計契約","Ａは、Ｂの委託により、Ｂの指揮命令に服さずにプログラムを開発工程のうち、システム設計までの工程に係る役務を提供する。"),
					new License("開発援助契約","Ａは、Ｂの委託により、Ｂの指揮命令に服さずに専門的な立場からＢに対し必要な助言を行う。")
				})
			);
		}
		public void Dispose() {
			if(Contracts.Count>0){
				Contracts.Clear();
			}
		}
	}
}
