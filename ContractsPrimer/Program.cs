// $Header: /ComSpexHome.root/ComSpexHome/Visual Studio 2010/Projects/ComSpexHome/ContractsPrimer/Program.cs 1     11/07/13 7:23p Yosuke $
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ComSpex;

namespace ContractsPrimer {
	class Program {
		static void Main(string[] args) {
			using(ContractTypes cont=new ContractTypes()){
				using(XmlPersist<ContractTypes> xp=new XmlPersist<ContractTypes>(cont)){
					using(StreamWriter sw=new StreamWriter("test.xml")){
						xp.Save(sw);
					}
				}
			}
		}
	}
}
