using Analytics.Functions;
using Analytics.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemModel
{
    public sealed class MinusUnary : MinusOperator
    {
        protected override Type GetOperandType()
        {
            return typeof(double);
        }

        protected override Type GetReturnType()
        {
            return typeof(double);
        }

        protected override object Operation(object operand)
        {
            return -(double)operand;
        }
    }
}
