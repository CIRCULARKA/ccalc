namespace Ccalc.Tests.Unit.Core.Parsing;

[Trait("type", "unit")]
[Trait("category", "parsing")]
public class MathExpressionParserTests
{
    [Theory]
    [InlineData("(", true)]
    [InlineData("( ", true)]
    [InlineData(" (", true)]
    [InlineData(" ( ", true)]
    [InlineData("", false)]
    [InlineData(")", false)]
    [InlineData("x", false)]
    [InlineData(null, false)]
    public void IsOpeningParenthesis_AnyToken_ExpectedOutput(string token, bool expectedResult)
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
    [InlineData(")", false)]
    [InlineData("x", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsClosingParenthesis_AnyToken_ExpectedOutput(string token, bool expectedResult)
    {
        // Arrange
        var parser = new MathExpressionParser();

        // Act
        var actualResult = parser.IsOpeningParenthesis(token);

        // Assert
        Assert.Equal(expectedResult, actualResult);
    }
}