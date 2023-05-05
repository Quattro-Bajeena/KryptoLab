using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathUtils;

namespace DH
{
    public static class GFG
    {


        /* Iterative Function to calculate (x^n)%p in
        O(logy) */
        static int ModularPow(int x, int y, int p)
        {
            int res = 1;     // Initialize result

            x = x % p; // Update x if it is more than or
                       // equal to p

            while (y > 0)
            {
                // If y is odd, multiply x with result
                if (y % 2 == 1)
                {
                    res = (res * x) % p;
                }

                // y must be even now
                y = y >> 1; // y = y/2
                x = (x * x) % p;
            }
            return res;
        }

        // Utility function to store prime factors of a number
        static void FindPrimefactors(HashSet<int> s, int n)
        {
            // Print the number of 2s that divide n
            while (n % 2 == 0)
            {
                s.Add(2);
                n = n / 2;
            }

            // n must be odd at this point. So we can skip
            // one element (Note i = i +2)
            for (int i = 3; i <= Math.Sqrt(n); i = i + 2)
            {
                // While i divides n, print i and divide n
                while (n % i == 0)
                {
                    s.Add(i);
                    n = n / i;
                }
            }

            // This condition is to handle the case when
            // n is a prime number greater than 2
            if (n > 2)
            {
                s.Add(n);
            }
        }

        // Function to find smallest primitive root of n
        public static int FindPrimitive(int n)
        {
            HashSet<int> s = new HashSet<int>();

            // Check if n is prime or not
            if ( Formulas.IsPrime(n) == false)
            {
                return -1;
            }

            // Find value of Euler Totient function of n
            // Since n is a prime number, the value of Euler
            // Totient function is n-1 as there are n-1
            // relatively prime numbers.
            int phi = n - 1;

            // Find prime factors of phi and store in a set
            FindPrimefactors(s, phi);

            // Check for every number from 2 to phi
            for (int r = 2; r <= phi; r++)
            {
                // Iterate through all prime factors of phi.
                // and check if we found a power with value 1
                bool flag = false;
                foreach (int a in s)
                {

                    // Check if r^((phi)/primefactors) mod n
                    // is 1 or not
                    if (ModularPow(r, phi / (a), n) == 1)
                    {
                        flag = true;
                        break;
                    }
                }

                // If there was no power with value 1.
                if (flag == false)
                {
                    return r;
                }
            }

            // If no primitive root found
            return -1;
        }
    }
}
