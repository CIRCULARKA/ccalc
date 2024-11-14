namespace Ccalc.Tests.Integration.Core.Convertion;

[Trait("type", "integration")]
[Trait("category", "yard")]
public class ShuntingYardNotationConverterTests : MathExpressionIntegrationTests
{
    [Theory]
    [InlineData("1 + -2", "1 -2 +")]
    [InlineData("1 - 2 / 3.001", "1 2 3.001 / -")]
    [InlineData("   (1 - 1   ) * (2.33 + 1) /   3", "1 1 - 2.33 1 + * 3 /")]
    public void ToPostfix_ValidInfixExpression_ValidPostfixExpression(string infixExpression, string expectedResult)
    {
        // Arrange
        var converter = CreateConverter(CreateBasicOperations(), CreateDefaultMathExpressionParser());

        // Act
        var result = converter.ToPostfix(infixExpression);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccessful);
        Assert.True(string.IsNullOrWhiteSpace(result.ErrorMessage));
        Assert.Equal(expectedResult, result.Result);
    }

    [Theory]
    // Алгоритм Shunting Yard не подразумевает в своей реализации проверку подобных вариантов. Так написано в Wiki и с этим же я столкнулся на практике.
    // Эти выражения хоть и не вызовут ошибку, но они попадут в Evaluator, который точно не сможет его вычислить, поэтому в любом случае пользователь получит
    // ошибку вместо неправильного результата. Это мы также проверим в интеграционных тестах
    // [InlineData("()")]
    // [InlineData("1 2 +")]
    // [InlineData("+ 1 2")]
    // [InlineData("2 1")] 
    // [InlineData("1 + 2 + 3 * 4 / 1 -")]
    // [InlineData("5.015 + 1 - (1 + 1) * 2 2")]
    // [InlineData("15.01 / 3 * (2 + 2")]

    [InlineData("")]
    [InlineData("   ")]
    [InlineData("+")]
    [InlineData("(1 + 1")]
    [InlineData("1 + 1)")]
    [InlineData(")1 + 1)")]
    [InlineData("(1 + 1(")]
    [InlineData("(1 + 1))")]
    [InlineData("((1 + 1)")]
    [InlineData("15.01 / 3 * 2 + 2)")]
    [InlineData("15.01 / 3 * (2 + 2))")]
    public void ToPostfix_InvalidInfixExpression_ErrorResult(string infixExpession)
    {
        // Arrange
        var converter = CreateConverter(CreateBasicOperations(), CreateDefaultMathExpressionParser());

        // Act
        var result = converter.ToPostfix(infixExpession);

        // Assert
        Assert.NotNull(result);
        CheckIfError(result);
    }

    [Theory]
    [InlineData("1 ^ 2")]
    [InlineData("1 - 2 _ 3.001")]
    [InlineData("(1 - 1) * (2.33 $ 1) / 3")]
    public void ToPostfix_ValidInfixExpressionWithUnsupportedOperators_ErrorResult(string infixExpession)
    {
        // Arrange
        var converter = CreateConverter(CreateBasicOperations(), CreateDefaultMathExpressionParser());

        // Act
        var result = converter.ToPostfix(infixExpession);

        // Assert
        Assert.NotNull(result);
        CheckIfError(result);
    }

    /// <summary>
    /// Проверяет, является ли результат преобразования ошибочным
    /// </summary>
    /// <param name="result">Результат, который нужно проверить</param>
    private void CheckIfError(ConvertionResult result)
    {
        Assert.Null(result.Result);
        Assert.False(result.IsSuccessful);
        Assert.False(string.IsNullOrWhiteSpace(result.ErrorMessage));
    }

    /// <summary>
    /// Создаёт конвертер
    /// </summary>
    /// <param name="parser">Парсер математических выражений. Заглушка, если <see langword="null" /></param>
    /// <param name="operators">Список доступных математических операций</param>
    private ShuntingYardNotationConverter CreateConverter(
        List<Operator> operators,
        IMathExpressionParser parser)
    {
        ArgumentNullException.ThrowIfNull(operators);
        ArgumentNullException.ThrowIfNull(parser);

        return new ShuntingYardNotationConverter(parser: parser, operators: operators);
    }
}