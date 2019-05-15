using System;

namespace Bl.Method
{
    public class NewtonMethod
    {
        private readonly SingleVariableFunctionDelegate _fd1;
        private readonly SingleVariableFunctionDelegate _fd2;

        private IterationInfoEventArgs _iterationInfoEventArgs;

        public delegate void IterationInfoDelegate(object sender, IterationInfoEventArgs iterationInfoEventArgs);

        public NewtonMethod(SingleVariableFunctionDelegate functionD1, SingleVariableFunctionDelegate functionD2)
        {
            _fd1 = functionD1;
            _fd2 = functionD2;
        }

        public event IterationInfoDelegate OnIteration;

        /// <summary>
        /// Вычисление методом Ньютона
        /// </summary>
        /// <param name="start">Начальная точка</param>
        /// <param name="eps">Значение eps</param>
        /// <returns></returns>
        public double Calculation(double start, double eps = 0.001)
        {
            double x1, dx;
            int iteration = 0;
            double x0 = start;
            do
            {
                iteration++;
                x1 = x0 - _fd1(x0) / _fd2(x0);
                dx = Math.Abs(x1 - x0);
                x0 = x1;
                OnIteration?.Invoke(this, new IterationInfoEventArgs(x0, x1, iteration));
            }
            while (dx > eps);
            return x1;
        }
    }
}
