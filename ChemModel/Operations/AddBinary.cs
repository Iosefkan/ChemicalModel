using Analytics.Operators;
using System.Windows.Media;

namespace ChemModel
{
    public sealed class AddBinary : AddOperator
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
            return (double)operand1 + (double)operand2;
        }
    }
}
