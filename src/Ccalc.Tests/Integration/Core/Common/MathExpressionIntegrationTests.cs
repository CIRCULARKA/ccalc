namespace Ccalc.Tests.Integration.Core.Common;

public abstract class MathExpressionIntegrationTests : MathExpressionTests
{
    /// <summary>
    /// Создаёт рабочий парсер математических выражений
    /// </summary>
    protected MathExpressionParser CreateDefaultMathExpressionParser() =>
        new MathExpressionParser();
}