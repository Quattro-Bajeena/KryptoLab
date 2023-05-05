using MathUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DH;

namespace TestBBS
{
	[TestClass]
	public class DHTest
	{

		public DHTest() 
		{ 

		}


		[TestMethod]
		public void DiffieHellman()
		{
			var a = new Client();
			var b = new Client();

			Client.KeyExchange(a, b);

			Assert.AreEqual(a.k, b.k);
		}
	}
}
