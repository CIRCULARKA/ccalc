namespace Ccalc.Tests.Core;

public class RPNExpressionEvaluatorTests
{
    [Theory]
    [InlineData("1 2 +", 3)]
    public void Evaluate(string rpnExpression, double expected)
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
