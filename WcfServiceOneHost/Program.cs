using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace WcfServiceOne {
	class Program {
		static void Main(string[] args) {
			using(ServiceHost Host=new ServiceHost(typeof(IService1))){
				Host.Open();
			}
		}
	}
}
