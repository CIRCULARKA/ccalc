namespace Ccalc.Tests.Infrastructure;

public class ShuntingYardNotationConverterTests
{
    [Theory]
    [InlineData("1 + 2", "1 2 +")]
    [InlineData("1 - 2 / 3", "1 2 3 / -")]
    [InlineData("(1 - 1) * (2 + 1) / 3", "1 1 2 1 + - * 3 /")]
    public void ToPostfix_ValidInfixExpression_ValidPostfixExpression(string infixExpression, string expected)
    {
        // Arrange
        var converter = CreateConverter();

        // Act
        var actual = converter.ToPostfix(infixExpression);

        // Assert
        Assert.Equal(expected, actual);
    }

    private ShuntingYardNotationConverter CreateConverter()
    {
        return new ShuntingYardNotationConverter();
    }
}