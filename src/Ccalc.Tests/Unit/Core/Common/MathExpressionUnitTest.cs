namespace Ccalc.Tests.Unit.Core.Common;

public abstract class MathExpressionUntiTests
{
    /// <summary>
    /// Создаёт рабочий парсер математических выражений
    /// </summary>
    protected MathExpressionParser CreateDefaultMathExpressionParser() =>
        new MathExpressionParser();

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