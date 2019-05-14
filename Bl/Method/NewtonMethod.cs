/// Метод Ньютона

namespace Bl.Method
{
    public class NewtonMethod
    {
        private readonly SingleVariableFunctionDelegate m_FuncD1;
        private readonly SingleVariableFunctionDelegate m_FuncD2;

        private IterationInfoEventArgs _iterationInfoEventArgs;

        public NewtonMethod(SingleVariableFunctionDelegate functionD1, SingleVariableFunctionDelegate functionD2)
        {
            m_FuncD1 = functionD1;
            m_FuncD2 = functionD2;
        }

        public IterationInfoEventArgs Calculation(double x1, double eps = 0.001)
        {
            int iter = 1;
            double x2;

            do
            {
                x2 = x1 - m_FuncD1(x1) / m_FuncD2(x1);
                iter++;
            }
            while (m_FuncD1(x2) <= eps);

            return new IterationInfoEventArgs(x1, x2, iter);
        }
    }
}
