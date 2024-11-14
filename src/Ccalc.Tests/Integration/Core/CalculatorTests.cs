namespace Ccalc.Tests.Integration.Core;

[Trait("type", "integration")]
[Trait("category", "calculator")]
public class CalculatorTests : MathExpressionIntegrationTests
{
    private const int MaxPrecision = 6;

    [Theory]
    [InlineData("1 + 2", 3)]
    [InlineData("-1 + -2", -3)]
    [InlineData("1 - 2", -1)]
    [InlineData("-1 - -2", 1)]
    [InlineData("1 * 2", 2)]
    [InlineData("-1 * -2", 2)]
    [InlineData("1 / 2", 0.5)]
    [InlineData("-1 / -2", 0.5)]
    [InlineData("-0.5 * (4 / 2) - 123.5 + (20 - 3)", -107.5)]
    [InlineData("((20 - 10) / ((2 + 1) * (4 - 2))) * 10", 16.666666666)]
    public void Evaluate_ValidExpression_ReturnsSuccessfulValidResult(string mathExpression, double expectedResult)
    {
        // Arrange
        var supportedOperations = CreateBasicOperations();

        var parser = CreateDefaultMathExpressionParser();

        var calculator = CreateCalculator(
            evaluator: CreateRPNEvaluator(parser, supportedOperations),
            converter: CreateShuntingYardConverter(parser, supportedOperations));

        // Act
        var actualResult = calculator.Evaluate(mathExpression);

        // Assert
        Assert.NotNull(actualResult);
        Assert.True(actualResult.IsSuccessful);
        Assert.NotNull(actualResult.Result);
        Assert.Equal(Math.Round(expectedResult, MaxPrecision), Math.Round(actualResult.Result ?? 0, MaxPrecision));
        Assert.Null(actualResult.ErrorMessage);
    }

    [Theory]
    [InlineData("12_")]
    [InlineData("-1 + --2")]
    [InlineData("-1 ^ -2")]
    [InlineData("(1 * 2")]
    [InlineData("-1 * -2)")]
    [InlineData("1 / 2+")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("((20 - 10) / ((2 + 1) * (4 - 2))) * 10(")]
    public void Evaluate_InvalidExpression_ReturnsErrorResult(string mathExpression)
    {
        // Arrange
        var supportedOperations = CreateBasicOperations();

        var parser = CreateDefaultMathExpressionParser();

        var calculator = CreateCalculator(
            evaluator: CreateRPNEvaluator(parser, supportedOperations),
            converter: CreateShuntingYardConverter(parser, supportedOperations));

        // Act
        var actualResult = calculator.Evaluate(mathExpression);

        // Assert
        Assert.NotNull(actualResult);
        Assert.Null(actualResult.Result);
        Assert.False(actualResult.IsSuccessful);
        Assert.False(string.IsNullOrWhiteSpace(actualResult.ErrorMessage));
    }

    /// <summary>
    /// Создаёт объект калькулятора
    /// </summary>
    private Calculator CreateCalculator(
        INotationConverter converter,
        IExpressionEvaluator evaluator)
    {
        ArgumentNullException.ThrowIfNull(converter);
        ArgumentNullException.ThrowIfNull(evaluator);

        return new Calculator(notationConverter: converter, evaluator: evaluator);
    }

    /// <summary>
    /// Создаёт объект, который вычисляет результат выражения, записанного в обратной польской нотации
    /// </summary>
    private RPNExpressionEvaluator CreateRPNEvaluator(
        IMathExpressionParser parser, 
        List<Operator> supportedOperations) =>
        new RPNExpressionEvaluator(supportedOperations, parser);

    /// <summary>
    /// Создаёт объект, который преобразовывает инфиксное математическое выражение в постфиксное
    /// </summary>
    private ShuntingYardNotationConverter CreateShuntingYardConverter(
        IMathExpressionParser parser, 
        List<Operator> supportedOperations) =>
        new ShuntingYardNotationConverter(parser, supportedOperations);
}