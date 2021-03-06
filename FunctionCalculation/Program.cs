﻿using System;
using Bl;
using Bl.Method;

namespace FunctionCalculation
{
    class Program
    {
        private static double startPoint = 0, h = 0.5;

        private static double MinimizationFunction(double x) => 2 * Math.Pow(x - 3, 2) + Math.Pow(Math.E, 0.5 * x);

        static void Main(string[] args)
        {
            var optimization = new UnconditionalOptimization(MinimizationFunction);
            var (leftBound, rightBound) = optimization.GetBoundForMinimization(startPoint, h);
            Console.WriteLine("Первичные границы: [{0}; {1}]", leftBound, rightBound);

            Console.WriteLine("///Уточнение границ методом деления пополам///");
            var halvingMethod = new HalvingMethod(MinimizationFunction);
            var result = halvingMethod.Calculation(leftBound, rightBound);
            Console.WriteLine("Границы: [{0}; {1}] Итераций = {2}", halvingMethod.LeftBound,
                                               halvingMethod.RightBound, halvingMethod.Iteration);
            Console.WriteLine("x = {0:f3}\nЗначение функции {1:f3}", result, MinimizationFunction(result));
            Console.ReadKey();
        }
    }
}
