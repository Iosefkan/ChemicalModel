using Analytics.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemModel
{
    public sealed class PowerBinary : PowerOperator
    {
        protected override Type GetOperand1Type()
        {
            return typeof(double);
        }

        protected override Type GetOperand2Type()
        {
            return typeof(double);
        }

        protected override Type GetReturnType()
        {
            return typeof(double);
        }

        protected override object Operation(object operand1, object operand2)
        {
            return Math.Pow((double)operand1, (double)operand2);
        }
    }
}
