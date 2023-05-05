using System.Numerics;
using MathUtils;

namespace DH
{

    public class Client
    {
        private int n;
        private int g;
        private int x;

        public BigInteger Y { private get; set; }
        public BigInteger k {  get; private set; }

	    public void SetPublic(int n, int g)
        {
            this.n = n;
            this.g = g;
        }

        public BigInteger GeneratePrivateKey()
        {
            x = PrimeGenerator.RandomPrime();
            var X = BigInteger.ModPow(g, x, n);
            return X;
        }

        public void GenerateSessionKey()
        {
            k = BigInteger.ModPow(Y, x, n);
        }


        public static void KeyExchange(Client a, Client b)
        {
			var n = PrimeGenerator.RandomPrime();
			var g = GFG.FindPrimitive(n);

			a.SetPublic(n, g);
			b.SetPublic(n, g);


			var a_X = a.GeneratePrivateKey();
			var b_X = b.GeneratePrivateKey();

			a.Y = b_X;
			b.Y = a_X;

            a.GenerateSessionKey();
            b.GenerateSessionKey();

			Console.WriteLine("A session key: " + a.k);
			Console.WriteLine("B session key: " + b.k);
		}
    }

    
}