namespace Ccalc.Tests.Infrastructure;

[Trait("type", "unit")]
[Trait("category", "yard")]
public class ShuntingYardNotationConverterTests
{
    [Theory]
    [InlineData("1 + 2", "1 2 +")]
    [InlineData("1 - 2 / 3.001", "1 2 3.001 / -       ")]
    [InlineData("   (1 - 1) * (2.33 + 1) / 3", "1 1 2.33    1 + - * 3 /")]
    public void ToPostfix_ValidInfixExpression_ValidPostfixExpression(string infixExpression, string expectedResult)
    {
        // Arrange
        var converter = CreateConverter();

        // Act
        var result = converter.ToPostfix(infixExpression);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccessful);
        Assert.True(string.IsNullOrWhiteSpace(result.ErrorMessage));
        Assert.Equal(expectedResult, result.Result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("()")]
    [InlineData("1 2 +")]
    [InlineData("1 + 2 + 3 * 4 / 1 -")]
    [InlineData("+")]
    [InlineData("2 1")]
    [InlineData("5.015 + 1 - (1 + 1) * 2 2")]
    [InlineData("15.01 / 3 * (2 + 2")]
    [InlineData("15.01 / 3 * 2 + 2)")]
    [InlineData("15.01 / 3 * (2 + 2))")]
    public void ToPostfix_InvalidInfixExpression_ErrorResult(string infixExpession)
    {
        // Arrange
        var converter = CreateConverter();

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
        var converter = CreateConverter();

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

    private ShuntingYardNotationConverter CreateConverter()
    {
        return new ShuntingYardNotationConverter();
    }
}