namespace Ccalc.Tests.Unit.Core.Parsing;

[Trait("type", "unit")]
[Trait("category", "parsing")]
public class MathExpressionParserTests
{
    [Theory]
    [InlineData("1", true)]
    [InlineData("-1", true)]
    [InlineData("501249.0102492", true)]
    [InlineData("-501249.0102492", true)]
    [InlineData("-1x", false)]
    [InlineData("1 1", false)]
    [InlineData("+", false)]
    [InlineData(null, false)]
    [InlineData("", false)]
    public void IsOperand_AnyToken_ExpectedResult(string token, bool expectedResult)
    {
        // Arrange
        var parser = CreateParser();

        // Act
        var actualResult = parser.IsOperand(token);

        // Assert
        Assert.Equal(expectedResult, actualResult);
    }

    [Theory]
    [InlineData("(", true)]
    [InlineData("( ", true)]
    [InlineData(" (", true)]
    [InlineData(" ( ", true)]
    [InlineData("", false)]
    [InlineData(")", false)]
    [InlineData("x", false)]
    [InlineData(null, false)]
    public void IsOpeningParenthesis_AnyToken_ExpectedResult(string token, bool expectedResult)
    {
        // Arrange
        var parser = CreateParser();

        // Act
        var actualResult = parser.IsOpeningParenthesis(token);

        // Assert
        Assert.Equal(expectedResult, actualResult);
    }

    [Theory]
    [InlineData(")", true)]
    [InlineData(") ", true)]
    [InlineData(" )", true)]
    [InlineData(" ) ", true)]
    [InlineData("(", false)]
    [InlineData("x", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsClosingParenthesis_AnyToken_ExpectedResult(string token, bool expectedResult)
    {
        // Arrange
        var parser = CreateParser();

        // Act
        var actualResult = parser.IsClosingParenthesis(token);

        // Assert
        Assert.Equal(expectedResult, actualResult);
    }

    [Theory]
    [InlineData("1 + 2", "1", "+", "2")]
    [InlineData("1 + 2 / 2 - 1 * 4 $ -231 _ -24.12805", "1", "+", "2", "/", "2", "-", "1", "*", "4", "$", "-231", "_", "-24.12805")]
    [InlineData("(-95.0001 * (1300 / (157298471829 + -0)))", "(", "-95.0001", "*", "(", "1300", "/", "(", "157298471829", "+", "-0", ")", ")", ")")]
    public void GetTokenStack_AnyNonEmptyExpression_ExpectedResult(string expression, params string[] expectedResult)
    {
        // Arrange
        var parser = CreateParser();

        // Act
        var actualResult = parser.GetTokenStack(expression);

        // Assert
        Assert.NotNull(actualResult);
        Assert.Equal(expectedResult.Length, actualResult.Count);

        for (int i = 0; i < expectedResult.Length; i++)
            Assert.Equal(expectedResult[i], actualResult.Pop());
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GetTokenStack_EmptyExpression_ReturnsEmptyStack(string emptyExpression)
    {
        // Arrange
        var parser = CreateParser();

        // Act
        var actualResult = parser.GetTokenStack(emptyExpression);

        // Assert
        Assert.NotNull(actualResult);
        Assert.Empty(actualResult);
    }

    /// <summary>
    /// Создаёт парсер математических выражений
    /// </summary>
    private MathExpressionParser CreateParser() =>
        new MathExpressionParser();
}