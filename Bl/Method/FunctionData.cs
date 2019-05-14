using System;

namespace Bl.Method
{
    internal class FunctionData : IComparable
    {
        public readonly double Value;
        public readonly double FunctionValue;

        public FunctionData(double value, double functionValue)
        {
            Value = value;
            FunctionValue = functionValue;
        }

        public int CompareTo(object sender)
        {
            if (sender is FunctionData functionData)
                return FunctionValue.CompareTo(functionData.FunctionValue);
            throw new Exception("Невозможно сравнить два объекта");
        }
    }
}