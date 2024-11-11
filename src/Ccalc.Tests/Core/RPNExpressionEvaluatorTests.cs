namespace Ccalc.Tests.Core;

public class RPNExpressionEvaluatorTests
{
    [Theory]
    // Generated with ChatGPT, THANKS. Verified
    [InlineData("5 3 +", 8)]
    [InlineData("6 2 -", 4)]
    [InlineData("7 3 *", 21)]
    [InlineData("12 4 /", 3)]
    [InlineData("4 2 3 * +", 10)]
    [InlineData("8 3 2 + *", 40)]
    [InlineData("10 5 2 - /", 3.333)]
    [InlineData("9 2 + 3 *", 33)]
    [InlineData("15 7 1 1 + - /", 3)]
    public void Evaluate_ValidRPNExpression_ExpectedResult(string rpnExpression, double expected)
    {
        // Arrange
        var evaluator = CreateEvaluator();
        
        // Act
        var actual = evaluator.Evaluate(rpnExpression);
        
        // Assert
        Assert.Equal(expected, actual);
    }

    private RPNExpressionEvaluator CreateEvaluator()
    {
        return new RPNExpressionEvaluator();
    }
}
