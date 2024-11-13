namespace Ccalc.Core.Parsing;

/// <summary>
/// Объект для парсинга токенов в математических выражениях.
/// Не поддерживает математические переменные
/// </summary>
public class MathExpressionParser : IMathExpressionParser
{
    public bool IsOperand(string token)
    {
        double result = default;
        return double.TryParse(token, out result);
    }
    
    public Stack<string> GetTokenStack(string expression)
    {
        var tokens = expression.Split(' ', StringSplitOptions.RemoveEmptyEntries).Reverse();

        var result = new Stack<string>();

        foreach (var token in tokens)
            result.Push(token);

        return result;
    }

    public bool IsOpeningParenthesis(string token)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(token);
        return token == "(";
    }

    public bool IsClosingParenthesis(string token)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(token);
        return token == ")";
    }
}