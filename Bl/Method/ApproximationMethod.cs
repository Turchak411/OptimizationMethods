using System;
using System.Collections.Generic;
using System.Linq;

namespace Bl.Method
{
    public class ApproximationMethod
    {
        private readonly SingleVariableFunctionDelegate _f;

        private IterationInfoEventArgs _iterationInfoEventArgs;

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
        public int Iteration => _iterationInfoEventArgs.Iteration;

        private double A0(double x1) => _f(x1);

        private double A1(double f2, double f1, double x2, double x1) => (f2 - f1) / (x2 - x1);

        private static double A2(double f3, double f1, double a1, double x3, double x2, double x1) 
            => 1 / (x3 - x2) * ((f3 - f1) / (x3 - x1) - a1);

        private static double MiddleX(double x1, double x2, double a1, double a2) => (x1 + x2) / 2 - a1 / (2 * a2);

        public delegate void IterationInfoDelegate(object sender, IterationInfoEventArgs iterationInfoEventArgs);

        public ApproximationMethod(SingleVariableFunctionDelegate function)
        {
            _f = function;
        }

        public event IterationInfoDelegate OnIteration;

        /// <summary>
        /// Вычисление методом квадратичной апроксимации
        /// </summary>
        /// <param name="leftBound">Значение левой границы</param>
        /// <param name="rightBound">Значение правой границы</param>
        /// <returns>Средение значение x между границами</returns>
        public double Calculation(double leftBound, double rightBound, double eps = 0.001)
        {
            var functionDatas = new List<FunctionData>
            {
                new FunctionData(leftBound, _f(leftBound)),
                new FunctionData(rightBound, _f(rightBound))
            };

            var iteration = 0;
            double middleX;
            OnIteration?.Invoke(this, new IterationInfoEventArgs(leftBound, rightBound, iteration));
            do
            {
                var x1 = functionDatas[0].Value;
                var x3 = functionDatas[1].Value;

                var x2 = (x3 + x1) / 2;
                functionDatas.Insert(1, new FunctionData(x2, _f(x2)));

                var f1 = functionDatas[0].FunctionValue;
                var f2 = functionDatas[1].FunctionValue;
                var f3 = functionDatas[2].FunctionValue;

                var a1 = A1(f2, f1, x2, x1);
                var a2 = A2(f3, f1, a1, x3, x2, x1);

                middleX = MiddleX(x1, x2, a1, a2);
                functionDatas.Remove(functionDatas.Max());

                iteration++;
                OnIteration?.Invoke(this, new IterationInfoEventArgs(functionDatas[0].Value, functionDatas[1].Value, iteration));
            } while (eps < Math.Abs((functionDatas.Min(f => f.FunctionValue) - _f(middleX)) / _f(middleX)));

            _iterationInfoEventArgs = new IterationInfoEventArgs(functionDatas[0].Value, functionDatas[1].Value, iteration);

            return middleX;
        }
    }
}
