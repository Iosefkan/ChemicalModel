using Analytics.Operators;

namespace ChemModel
{
    public sealed class MultiplyBinary : MultiplyOperator
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
            return (double)operand1 * (double)operand2;
        }
    }
}
