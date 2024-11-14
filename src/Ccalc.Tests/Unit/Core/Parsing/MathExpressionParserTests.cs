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
        var parser = new MathExpressionParser();

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
        var parser = new MathExpressionParser();

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
        var parser = new MathExpressionParser();

        // Act
        var actualResult = parser.IsClosingParenthesis(token);

        // Assert
        Assert.Equal(expectedResult, actualResult);
    }
}