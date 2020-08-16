using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace GLaDOS
{
    class GLaDOSCalc
    {
        public static void CalculatorActionHandler(string Formula)
        {
            try
            {
                switch (Formula)
                {
                    case string Case when Case.Contains("+"):
                        string[] Addition = Case.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
                        GLaDOSMisc.GLaDOSAnswer(Addition[0].Trim() + " + " + Addition[1].Trim() + " = " + Convert.ToString((double)BigInteger.Parse(Addition[0]) + (double)BigInteger.Parse(Addition[1])));
                        break;
                    case string Case when Case.Contains("-"):
                        string[] Substraction = Case.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                        GLaDOSMisc.GLaDOSAnswer(Substraction[0].Trim() + " - " + Substraction[1].Trim() + " = " + Convert.ToString((double)BigInteger.Parse(Substraction[0]) - (double)BigInteger.Parse(Substraction[1])));
                        break;
                    case string Case when Case.Contains("*"):
                        string[] Multiplication = Case.Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
                        GLaDOSMisc.GLaDOSAnswer(Multiplication[0].Trim() + " * " + Multiplication[1].Trim() + " = " + Convert.ToString((double)BigInteger.Parse(Multiplication[0]) * (double)BigInteger.Parse(Multiplication[1])));
                        break;
                    case string Case when Case.Contains("/"):
                        string[] Division = Case.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                        GLaDOSMisc.GLaDOSAnswer(Division[0].Trim() + " / " + Division[1].Trim() + " = " + Convert.ToString((double)BigInteger.Parse(Division[0]) / (double)BigInteger.Parse(Division[1])));
                        break;
                    case string Case when Case.Contains("!"):
                        string Factorial = Case.Remove(Case.IndexOf("!"), 1);
                        GLaDOSMisc.GLaDOSAnswer(Convert.ToString(FactThreadingTree(Convert.ToInt32(Factorial))));
                        break;
                }
            }
            catch (Exception CalculationFailure)
            {
                GLaDOSMisc.GLaDOSAnswer(CalculationFailure.Message);
            }
        }

        static BigInteger ProdTree(int l, int r)
        {
            if (l > r)
                return 1;
            if (l == r)
                return l;
            if (r - l == 1)
                return (BigInteger)l * r;
            int m = (l + r) / 2;
            return ProdTree(l, m) * ProdTree(m + 1, r);
        }

        const int threadCount = 2;

        static Task<BigInteger> AsyncProdTree(int l, int r)
        {
            return Task<BigInteger>.Run(() => ProdTree(l, r));
        }

        static BigInteger FactThreadingTree(int n)
        {
            if (n < 0)
                return 0;
            if (n == 0)
                return 1;
            if (n == 1 || n == 2)
                return n;
            if (n < threadCount + 1)
                return ProdTree(2, n);

            Task<BigInteger>[] tasks = new Task<BigInteger>[threadCount];

            tasks[0] = AsyncProdTree(2, n / threadCount);
            for (int i = 1; i < threadCount; i++)
            {
                tasks[i] = AsyncProdTree(((n / threadCount) * i) + 1, (n / threadCount) * (i + 1));
            }

            Task<BigInteger>.WaitAll(tasks);

            BigInteger result = 1;
            for (int i = 0; i < threadCount; i++)
            {
                result *= tasks[i].Result;
            }

            return result;
        }
    }
}
