namespace Ccalc.Tests.Unit.Core.Evaluation;

[Trait("type", "unit")]
[Trait("category", "rpn")]
public class RPNExpressionEvaluatorTests : MathExpressionUnitTests
{
    [Theory]
    [InlineData("1 2 +", "1", "2", "+")]
    [InlineData("1 2 -", "1", "2", "-")]
    public void Evaluate_ValidRPNWithUnknownOperators_ErrorResult(string rpnExpression, params string[] tokens)
    {
        // Arrange
        var parserStub = new Mock<IMathExpressionParser>();
        parserStub.Setup(p => p.GetTokenStack(rpnExpression)).Returns(new Stack<string>(tokens));

        var evaluator = CreateEvaluator(parser: parserStub.Object, availableOperators: CreateBasicOperations());
        
        // Act
        var result = evaluator.Evaluate(rpnExpression);
        
        // Assert
        CheckIfResultIsError(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Evaluate_EmptyExpression_ErrorResult(string emptyRpn)
    {
        // Arrange
        var parserStub = new Mock<IMathExpressionParser>();

        var evaluator = CreateEvaluator(parser: parserStub.Object, availableOperators: CreateBasicOperations());
        
        // Act
        var result = evaluator.Evaluate(emptyRpn);
        
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
    /// Создаёт объект для вычисления выражений в формате RPN
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
            availableOperators,
            parser);
    }
}