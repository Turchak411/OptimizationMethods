namespace Bl.Method
{
    public class NewtonMethod
    {
        private readonly SingleVariableFunctionDelegate _fd1;
        private readonly SingleVariableFunctionDelegate _fd2;

        private IterationInfoEventArgs _iterationInfoEventArgs;

        public NewtonMethod(SingleVariableFunctionDelegate functionD1, SingleVariableFunctionDelegate functionD2)
        {
            _fd1 = functionD1;
            _fd2 = functionD2;
        }

        /// <summary>
        /// Вычисление методом Ньютона
        /// </summary>
        /// <param name="x1">Начальная точка</param>
        /// <param name="eps">Значение eps</param>
        /// <returns></returns>
        public IterationInfoEventArgs Calculation(double x1, double eps = 0.001)
        {
            double x2;
            int count = 1;
            do
            {
                x2 = x1 - _fd1(x1) / _fd2(x1);
                count++;
            }
            while (_fd1(x2) <= eps);
            return new IterationInfoEventArgs(x1, x2, count);
        }
    }
}
