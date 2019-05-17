namespace Bl.Method
{
    public class HalvingMethod
    {
        private readonly SingleVariableFunctionDelegate _f;

        private IterationInfoEventArgs _iterationInfoEventArgs; 

        public delegate void IterationInfoDelegate(object sender, IterationInfoEventArgs iterationInfoEventArgs);
        /// <summary>
        /// Левая граница
        /// </summary>
        public double LeftBound => _iterationInfoEventArgs.LeftBound;

        /// <summary>
        /// Правая граница
        /// </summary>
        public double RightBound => _iterationInfoEventArgs.RightBound;

        /// <summary>
        /// Кол-во итераций
        /// </summary>
        public int Iteration => _iterationInfoEventArgs.Iteration-1;

        public double MiddleInterval(double leftBound, double rightBound) => (leftBound + rightBound) / 2;

        public double IntervalLength(double leftBound, double rightBound) => rightBound - leftBound;

        public double X1(double leftBound, double intervalLength) => leftBound + intervalLength / 4;

        public double X2(double rightBound, double intervalLength) => rightBound - intervalLength / 4;

        public HalvingMethod(SingleVariableFunctionDelegate function)
        {
            _f = function;
        }

        /// <summary>
        /// Вычисление методом деления пополам
        /// </summary>
        /// <param name="leftBound">Значение левой границы</param>
        /// <param name="rightBound">Значение правой границы</param>
        /// <returns>Средение значение x между границами</returns>
        public double Calculation(double leftBound, double rightBound)
        {
            _iterationInfoEventArgs = new IterationInfoEventArgs(leftBound, rightBound, 1);
            var middleInterval = MiddleInterval(leftBound, rightBound);
            return RunIterations(_iterationInfoEventArgs, ref middleInterval);
        }


        public double Calculation(double leftBound, double rightBound, double eps)
        {
            double a = leftBound, b = rightBound;
            var middleInterval = MiddleInterval(leftBound, rightBound);
            var len = b-a;
            var err = len;
            var iteration = 0;
            while (err >= eps)
            {
                var dx = IntervalLength(a, b);
                var x1 = X1(a, dx);
                var x2 = X2(b, dx);

                if (_f(x1) < _f(middleInterval))
                {
                    b = middleInterval;
                    middleInterval = x1;
                }
                else if(_f(x1) >= _f(middleInterval))
                {
                    if (_f(x2) < _f(middleInterval))
                    {
                        a = middleInterval;
                        middleInterval = x2;
                    }
                    else if (_f(x2) >= _f(middleInterval))
                    {
                        a = x1;
                        b = x2;
                    }
                }  

                err = (b-a) / len;
                iteration++;
                OnIteration?.Invoke(this, new IterationInfoEventArgs(a, b, iteration));
            }

            return middleInterval;
        }

        public event IterationInfoDelegate OnIteration;

        /// <summary>
        /// Выполнение итераций
        /// </summary>
        /// <param name="iterationInfo">Информация об итерациях</param>
        /// <param name="middleInterval">Средение значение x</param>
        /// <returns>Средение значение x между границами</returns>
        private double RunIterations(IterationInfoEventArgs iterationInfo, ref double middleInterval)
        {
            var dx = IntervalLength(iterationInfo.LeftBound, iterationInfo.RightBound);
            var x1 = X1(iterationInfo.LeftBound, dx);
            var x2 = X2(iterationInfo.RightBound, dx);
           
            if (_f(x1) < _f(middleInterval))
            {
                iterationInfo.RightBound = middleInterval;
                middleInterval = x1;
                OnIteration?.Invoke(this, iterationInfo);
                iterationInfo.Iteration++;
                RunIterations(iterationInfo, ref middleInterval);
            }
            else if (_f(x2) < _f(middleInterval))
            {
                iterationInfo.LeftBound = middleInterval;
                middleInterval = x2;
                OnIteration?.Invoke(this, iterationInfo);
                iterationInfo.Iteration++;
                RunIterations(iterationInfo, ref middleInterval);
            }

            return middleInterval;
        }

    }
}
