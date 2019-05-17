using System;

namespace Bl
{
    public class GoldenSection
    {
        private readonly double PHI = (1 + Math.Sqrt(5)) / 2;
        private readonly SingleVariableFunctionDelegate _function;

        public delegate void IterationInfoDelegate(object sender, IterationInfoEventArgs iterationInfoEventArgs);
        public event IterationInfoDelegate OnIteration;

        public GoldenSection(SingleVariableFunctionDelegate function)
        {
            _function = function;
        }

        public (double LeftBound, double RightBound, int iteration) FindMin(double a, double b, double e)
        {
            int iter = 0;

            double x1, x2;
            while (true)
            {
                iter++;

                x1 = b - (b - a) / PHI;
                x2 = a + (b - a) / PHI;
                if (_function(x1) >= _function(x2))
                    a = x1;
                else
                    b = x2;
                if (Math.Abs(b - a) < e)
                    break;

                OnIteration?.Invoke(this, new IterationInfoEventArgs(a, b, iter));
            }
            return (LeftBound: a, RightBound: b, iteration: iter);
        }
    }
}
