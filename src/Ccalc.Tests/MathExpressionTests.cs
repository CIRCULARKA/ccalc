namespace Ccalc.Tests;

public abstract class MathExpressionTests
{
    /// <summary>
    /// Создаёт базовый набор из четырёх базовых арифметических
    /// операций: "+", "-", "*", "/"
    /// </summary>
    protected List<Operator> CreateBasicOperations() =>
        new List<Operator>
        {
            new Operator(
                "+",
                1,
                OperatorAssociativity.Left,
                (a, b) => a + b),
            new Operator(
                "-",
                1,
                OperatorAssociativity.Left,
                (a, b) => a - b),
            new Operator(
                "*",
                2,
                OperatorAssociativity.Left,
                (a, b) => a * b),
            new Operator(
                "/",
                2,
                OperatorAssociativity.Left,
                (a, b) => a / b),
        };
}