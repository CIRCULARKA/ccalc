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
        expression = NormalizeParentheses(expression);

        var tokens = expression.Split(' ', StringSplitOptions.RemoveEmptyEntries).Reverse();

        var result = new Stack<string>();

        foreach (var token in tokens)
            result.Push(token);

        return result;
    }

    public bool IsOpeningParenthesis(string token)
    {
        if (token is null) return false;
        return token.Trim() == "(";
    }

    public bool IsClosingParenthesis(string token)
    {
        if (token is null) return false;
        return token.Trim() == ")";
    }

    /// <summary>
    /// Добавляет пробел после открывающих скобок и пере закрывающими,
    /// чтобы их можно было в последствии правильно токенизировать
    /// </summary>
    /// <param name="expression">Математическое выражение</param>
    private string NormalizeParentheses(string expression)
    {
        expression = expression.Replace("(", "( ");
        expression = expression.Replace(")", " )");

        return expression;
    }
}