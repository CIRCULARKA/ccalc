namespace Ccalc.Tests.Unit.Core.Evaluation;

[Trait("type", "unit")]
[Trait("category", "rpn")]
public class RPNExpressionEvaluatorTests
{
    /// <summary>
    /// Сколько цифр после запятой должно совпадать у ожидаемого и получившегося значений
    /// </summary>
    private static int MaxPrecision = 6;

    [Theory]
    // Generated with ChatGPT, THANKS. Verified
    [InlineData("5 3 +", 8)]
    [InlineData("6 2 -", 4)]
    [InlineData("7 3 *", 21)]
    [InlineData("12 4 /", 3)]
    [InlineData("4 2 3 * +", 10)]
    [InlineData("8 3 2 + *", 40)]
    [InlineData("10 5 2 - /", 3.3333333)]
    [InlineData("9 2 + 3 *", 33)]
    [InlineData("15 7 1 1 + - /", 3)]
    public void Evaluate_ValidRPNExpression_ExpectedResult(string rpnExpression, double expectedResult)
    {
        // Arrange
        var evaluator = CreateEvaluator();
        
        // Act
        var result = evaluator.Evaluate(rpnExpression);
        
        // Assert
        Assert.True(result.IsSuccessful);
        Assert.NotNull(result.Result);
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
        var evaluator = CreateEvaluator();
        
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
        var evaluator = CreateEvaluator();
        
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

        Assert.Null(result.Result);
        Assert.False(result.IsSuccessful);
        Assert.True(string.IsNullOrWhiteSpace(result.ErrorMessage) is false);
    }

    /// <summary>
    /// Создаёт <see cref="RPNExpressionEvaluator" />, по умолчанию поддерживающий четыре базовых арифметических
    /// операций: "+", "-", "*", "/"
    /// </summary>
    /// <param name="availableOperators">
    /// Список доступных для вычисления операторов. Если не указан, то
    /// будут использованы операторы по умолчанию
    /// </param>
    private RPNExpressionEvaluator CreateEvaluator(List<Operator>? availableOperators = null)
    {
        if (availableOperators is null)
            availableOperators = new List<Operator>
            {
                new Operator("+", (a, b) => a + b),
                new Operator("-", (a, b) => a - b),
                new Operator("*", (a, b) => a * b),
                new Operator("/", (a, b) => a / b)
            };

        return new RPNExpressionEvaluator(new OperatorFactory(availableOperators));
    }
}