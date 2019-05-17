using System;
using Bl;
using Bl.Method;

namespace FunctionCalculation
{
    class Program
    {
        private static double startPoint = 0, h = 0.5;

        private static double FunctionD1(double x) => 4 * (x - 3) + 0.5 * Math.Pow(Math.E, 0.5 * x);

        private static double FunctionD2(double x) => 4 + 0.25 * Math.Pow(Math.E, 0.5 * x);

        private static double MinimizationFunction(double x) => 2 * Math.Pow(x - 3, 2) + Math.Pow(Math.E, 0.5 * x);

        static void Main(string[] args)
        {
            double eps = 0.1;

            Console.WriteLine("Начальная точка X0 = {0}; h = {1}", startPoint, h);

            var optimization = new UnconditionalOptimization(MinimizationFunction);
            optimization.OnIteration += Print_OnIteration;
            var (leftBound, rightBound) = optimization.GetBoundForMinimization(startPoint, h);
            PrintBoundaries(leftBound,rightBound, optimization.Iteration);

            Console.ReadKey();
            Console.WriteLine("\n///Уточнение границ методом деления пополам///");

            var halvingMethod = new HalvingMethod(MinimizationFunction);
            halvingMethod.OnIteration += Print_OnIteration;
            var resultHalving = halvingMethod.Calculation(leftBound, rightBound, eps);
           // PrintBoundaries(halvingMethod.LeftBound, halvingMethod.RightBound, halvingMethod.Iteration);
            PrintFunction(resultHalving, MinimizationFunction);
            Console.ReadKey();

            // Метод золотого сечения:
            Console.WriteLine("\n > Уточнение методом золотого сечения:");
            var goldenSection = new GoldenSection(MinimizationFunction);
            goldenSection.OnIteration += Print_OnIteration;
            var (leftBound1, rightBound1, iterations) = goldenSection.FindMin(leftBound, rightBound, eps);
            Console.WriteLine("Границы: [{0:f3}; {1:f3}]\nИтераций = {2}", leftBound1,
                                                                           rightBound1,
                                                                           iterations);
            Console.WriteLine("x = {0:f3}\nF(x) = {1:f3}", (leftBound1 + rightBound1) / 2,
                                                           MinimizationFunction((leftBound1 + rightBound1) / 2));

            Console.ReadKey();
            Console.WriteLine("\n///Уточнение метом квадратичной аппроксимации///");

            var approximationMethod = new ApproximationMethod(MinimizationFunction);
            approximationMethod.OnIteration += Print_OnIteration;
            var resultApproximation = approximationMethod.Calculation(leftBound, rightBound, eps);
            PrintBoundaries(approximationMethod.LeftBound, approximationMethod.RightBound, approximationMethod.Iteration);
            PrintFunction(resultApproximation, MinimizationFunction);

            Console.ReadKey();
            Console.WriteLine("\n///Метод Ньютона///");
            var newtonMethod = new NewtonMethod(FunctionD1, FunctionD2);
            newtonMethod.OnIteration += Print_OnIteration;
            var resultNewtonMethod = newtonMethod.Calculation(startPoint, eps);
            PrintFunction(resultNewtonMethod, MinimizationFunction);

            Console.ReadKey();
        }

        private static void PrintFunction(double x, SingleVariableFunctionDelegate function) 
            => Console.WriteLine("x = {0:f3}\tЗначение функции {1:f3}", x, function(x));

        private static void PrintBoundaries(double leftBound, double rightBound, int iteration) 
            => Console.WriteLine("Результат: [{0:f3}; {1:f3}] Итераций = {2}", leftBound, rightBound, iteration);

        private static void Print_OnIteration(object sender, IterationInfoEventArgs infoEventArgs) 
            => Console.WriteLine("Границы: [{0:f6}; {1:f6}]\tИтерация = {2}", infoEventArgs.LeftBound,
                                                              infoEventArgs.RightBound, infoEventArgs.Iteration);
    }
}
