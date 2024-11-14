namespace Ccalc.Tests.Unit.Core;

[Trait("type", "unit")]
[Trait("category", "calculator")]
public class CalculatorTests
{
    [Fact]
    public void Evaluate_WhenCalled_ConvertsInfixToPostfix()
    {
        // Arrange
        var converterMock = new Mock<INotationConverter>();
        
        var calc = CreateCalculator(converter: converterMock.Object);

        // Act
        var result = calc.Evaluate("nevermind");

        // Assert
        converterMock.Verify(c => c.ToPostfix(It.IsAny<string>()), Times.Once());
    }

    [Fact]
    public void Evaluate_WhenCalled_EvaluatesExpressionWithEvaluator()
    {
        // Arrange
        var evaluatorMock = new Mock<IExpressionEvaluator>();
        
        var calc = CreateCalculator(evaluator: evaluatorMock.Object);

        // Act
        var result = calc.Evaluate("nevermind");

        // Assert
        evaluatorMock.Verify(c => c.Evaluate(It.IsAny<string>()), Times.Once());
    }

    [Fact]
    public void Evaluate_WhenConvertionIsFailed_ReturnsErrorWithConvertionErrorMessage()
    {
        // Arrange
        var convertionErrorResult = ConvertionResult.CreateError("Convertion error");
        
        var converterStub = new Mock<INotationConverter>();
        converterStub.Setup(c => c.ToPostfix(It.IsAny<string>())).Returns(convertionErrorResult);
        
        var calc = CreateCalculator(converter: converterStub.Object);

        // Act
        var actualResult = calc.Evaluate("nevermind");

        // Assert
        Assert.NotNull(actualResult);
        Assert.Null(actualResult.Result);
        Assert.False(actualResult.IsSuccessful);
        Assert.Equal(convertionErrorResult.ErrorMessage, actualResult.ErrorMessage);
    }

    [Fact]
    public void Evaluate_WhenEvaluationIsFailed_ReturnsErrorWithEvaluationErrorMessage()
    {
        // Arrange
        var evaluationErrorResult = EvaluationResult.CreateError("Evaluation error");
        
        var evaluatorStub = new Mock<IExpressionEvaluator>();
        evaluatorStub.Setup(c => c.Evaluate(It.IsAny<string>())).Returns(evaluationErrorResult);
        
        var calc = CreateCalculator(evaluator: evaluatorStub.Object);

        // Act
        var actualResult = calc.Evaluate("nevermind");

        // Assert
        Assert.Same(evaluationErrorResult, actualResult);
    }

    [Fact]
    public void Evaluate_ConvertionAndEvaluationSucceeded_ReturnsSuccessfulResult()
    {
        // Arrange
        var expectedResult = -123.123;
        var evaluationSuccessResult = EvaluationResult.CreateSuccess(expectedResult);

        var converterStub = new Mock<INotationConverter>();
        converterStub.Setup(c => c.ToPostfix(It.IsAny<string>())).Returns(ConvertionResult.CreateSuccess("nevermind"));
        
        var evaluatorStub = new Mock<IExpressionEvaluator>();
        evaluatorStub.Setup(c => c.Evaluate(It.IsAny<string>())).Returns(evaluationSuccessResult);
        
        var calc = CreateCalculator(evaluator: evaluatorStub.Object, converter: converterStub.Object);

        // Act
        var actualResult = calc.Evaluate("nevermind");

        // Assert
        Assert.Same(evaluationSuccessResult, actualResult);
    }

    /// <summary>
    /// Создаёт объект калькулятора
    /// </summary>
    private Calculator CreateCalculator(
        INotationConverter? converter = null,
        IExpressionEvaluator? evaluator = null)
    {
        if (converter is null)
            converter = new Mock<INotationConverter>().Object;

        if (evaluator is null)
            evaluator = new Mock<IExpressionEvaluator>().Object;

        return new Calculator(notationConverter: converter, evaluator: evaluator);
    }
}