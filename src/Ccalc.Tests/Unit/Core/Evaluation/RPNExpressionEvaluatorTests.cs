namespace Ccalc.Tests.Unit.Core.Evaluation;

[Trait("type", "unit")]
[Trait("category", "rpn")]
public class RPNExpressionEvaluatorTests : MathExpressionUntiTests
{
    /// <summary>
    /// Сколько цифр после запятой должно совпадать у ожидаемого и получившегося значений
    /// </summary>
    private static int MaxPrecision = 6;

    [Theory]
    // Generated with ChatGPT, THANKS. Verified
    [InlineData("5 3 +   ", 8)]
    [InlineData("6 2   -", 4)]
    [InlineData("7 3 *", 21)]
    [InlineData("12   4 /", 3)]
    [InlineData("4 2 3 * +", 10)]
    [InlineData("8 3 2 + *", 40)]
    [InlineData("10 5 2 - /", 3.3333333)]
    [InlineData("9 2 +   3 *", 33)]
    [InlineData("15 7 1 1 + - /", 3)]
    public void Evaluate_ValidRPNExpression_ExpectedResult(string rpnExpression, double expectedResult)
    {
        // Arrange
        var parser = CreateDefaultMathExpressionParser();
        var evaluator = CreateEvaluator(parser: parser, availableOperators: CreateBasicOperations());
        
        // Act
        var result = evaluator.Evaluate(rpnExpression);
        
        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Result);
        Assert.True(result.IsSuccessful);
        Assert.Null(result.ErrorMessage);

        Assert.Equal(
            Math.Round((double)(result?.Result ?? 0), MaxPrecision),
            Math.Round(expectedResult, MaxPrecision));
    }

    [Theory]
    [InlineData("")]
    [InlineData("             ")]
    [InlineData("*")]
    [InlineData("/")]
    [InlineData("1 1")]
    [InlineData("1 1 1")]
    public void Evaluate_InvalidRPNWithKnownOperatorsExpression_ErrorResult(string rpnExpression)
    {
        // Arrange
        var parser = CreateDefaultMathExpressionParser();
        var evaluator = CreateEvaluator(parser: parser, availableOperators: CreateBasicOperations());
        
        // Act
        var result = evaluator.Evaluate(rpnExpression);
        
        // Assert
        CheckIfResultIsError(result);
    }

    [Theory]
    [InlineData("1 2 ^")]
    [InlineData("1 2 + 4 3 - 3 1 ^")]
    [InlineData("1 1 1 &")]
    [InlineData("1 1 !")]
    public void Evaluate_ValidRPNWithUnknownOperators_ErrorResult(string rpnExpression)
    {
        // Arrange
        var parser = CreateDefaultMathExpressionParser();
        var evaluator = CreateEvaluator(parser: parser, availableOperators: CreateBasicOperations());
        
        // Act
        var result = evaluator.Evaluate(rpnExpression);
        
        // Assert
        CheckIfResultIsError(result);
    }

    /// <summary>
    /// Проверяет, что результат похож на ошибочный
    /// </summary>
    /// <param name="result">Объект результата, который нужно проверить</param>
    private void CheckIfResultIsError(EvaluationResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        Assert.NotNull(result);
        Assert.Null(result.Result);
        Assert.False(result.IsSuccessful);
        Assert.True(string.IsNullOrWhiteSpace(result.ErrorMessage) is false);
    }

    /// <summary>
    /// </summary>
    /// <param name="availableOperators">
    /// Список доступных для вычисления операторов. Если не указан, то
    /// будут использованы операторы по умолчанию
    /// </param>
    /// <param name="parser">Парсер. Если не указан, то будет использована заглушка</param>
    private RPNExpressionEvaluator CreateEvaluator(
        List<Operator> availableOperators,
        IMathExpressionParser? parser = null)
    {
        ArgumentNullException.ThrowIfNull(availableOperators);

        if (parser is null)
            parser = new Mock<IMathExpressionParser>().Object;

        return new RPNExpressionEvaluator(
            new OperatorFactory(availableOperators),
            parser);
    }
}