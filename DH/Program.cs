using System.Numerics;
using MathUtils;

namespace DH
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var a = new Client();
            var b = new Client();

            var n = BigPrimeGenerator.RandomPrime();
            var g = GFG.FindPrimitive(n);

            a.SetPublic(n, g);
            b.SetPublic(n, g);


            var X = a.Generate();
            var Y = b.Generate();

            
        }
    }

    class Client
    {
        private int n;
        private int g;

        public BigInteger Y { private get; set; }
        private BigInteger k;

        public void SetPublic(int n, int g)
        {
            this.n = n;
            this.g = g;
        }

        public BigInteger Generate()
        {
            var x = BigPrimeGenerator.RandomPrime();
            var X = BigInteger.ModPow(g, x, n);
            return X;
        }

        public void GenerateSessionKey()
        {
            //k = BigInteger.ModPow();
        }
    }

    
}