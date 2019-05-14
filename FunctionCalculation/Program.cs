﻿using System;
using Bl;
using Bl.Method;

namespace FunctionCalculation
{
    class Program
    {
        private static double startPoint = 6, h = 0.5;

        private static double FunctionD1(double x) => 4 * (x - 3) + 0.5 * Math.Pow(Math.E, 0.5 * x);

        private static double FunctionD2(double x) => 4 + 0.25 * Math.Pow(Math.E, 0.5 * x);

        private static double MinimizationFunction(double x) => 2 * Math.Pow(x - 3, 2) + Math.Pow(Math.E, 0.5 * x);

        static void Main(string[] args)
        {
            double eps = 0.001;

            // Установление первичных границ:

            var optimization = new UnconditionalOptimization(MinimizationFunction);
            var (leftBound, rightBound) = optimization.GetBoundForMinimization(startPoint, h);
            Console.WriteLine("Первичные границы: [{0:f3}; {1:f3}]", leftBound, rightBound);

            // Метод Золотого сечения
            Console.WriteLine("///Уточнение методом золотого сечения///");
            var goldenSection = new GoldenSection(MinimizationFunction);
            (leftBound, rightBound) = goldenSection.FindMin(leftBound, rightBound, eps);

            Console.WriteLine("///Уточнение метом квадратичной апроксимации///");
            var approximationMethod = new ApproximationMethod(MinimizationFunction);
            var r = approximationMethod.Calculation(leftBound, rightBound);
            Console.WriteLine("Границы: [{0:f3}; {1:f3}] Итераций = {2}", approximationMethod.LeftBound, 
                                    approximationMethod.RightBound, approximationMethod.Iteration);
            Console.WriteLine("x= {0:f3}\nЗначение функции {1:f3}", r, MinimizationFunction(r));

            Console.WriteLine("///Метод Ньютона///");
            var newtonMethod = new NewtonMethod(FunctionD1, FunctionD2);
            var resultNewtonMethod = newtonMethod.Calculation(startPoint);
            Console.WriteLine("x = {0:f3}\nЗначение функции {1:f3} Итераций = {2}",
                resultNewtonMethod.RightBound, MinimizationFunction(resultNewtonMethod.RightBound), resultNewtonMethod.Iteration);

            Console.ReadKey();
        }
    }
}
