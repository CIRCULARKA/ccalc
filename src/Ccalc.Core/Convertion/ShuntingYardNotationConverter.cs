namespace Ccalc.Core.Convertion;

/// <summary>
/// Класс для преобразования математических выражений в инфиксной
/// нотации в постфиксную (польскую) с помощью алгоритма shunting yard
/// </summary>
public class ShuntingYardNotationConverter : INotationConverter
{
    private const string InvalidExpressionErrorMessage = "Invalid expression";

    private const string ParenthesesMismatchErrorMessage = "Parentheses mismatch";

    private readonly IMathExpressionParser _parser;

    private readonly OperatorFactory _operatorFactory;

    public ShuntingYardNotationConverter(IMathExpressionParser parser, OperatorFactory operatorFactory)
    {
        ArgumentNullException.ThrowIfNull(parser);
        ArgumentNullException.ThrowIfNull(operatorFactory);

        _parser = parser;
        _operatorFactory = operatorFactory;
    }

    public ConvertionResult ToPostfix(string infixExpression)
    {
        if (string.IsNullOrWhiteSpace(infixExpression))
            return ConvertionResult.CreateError("Expression can not be empty");

        var tokensStack = _parser.GetTokenStack(infixExpression);

        var operatorsStack = new Stack<string>();

        string result = string.Empty;

        while (tokensStack.Any())
        {
            var token = tokensStack.Pop();

            if (_parser.IsOperand(token))
            {
                result = AppendToken(result, token);
                continue;
            }

            if (_parser.IsOpeningParenthesis(token))
            {
                operatorsStack.Push(token);
                continue;
            }

            if (_parser.IsClosingParenthesis(token))
            {
                var enclosedOperators = GetOperatorsInParentheses(operatorsStack);
                if (enclosedOperators is null)
                    return ConvertionResult.CreateError(ParenthesesMismatchErrorMessage);

                result = AppendToken(result, enclosedOperators);
                continue;
            }

            var currentOperator = _operatorFactory.GetOperator(token);
            if (currentOperator is null)
                return ConvertionResult.CreateError($"Operator \"{token}\" is not supported");

            string? previousOperatorToken = null;
            operatorsStack.TryPeek(out previousOperatorToken);

            var previousOperator = _operatorFactory.GetOperator(previousOperatorToken ?? string.Empty);

            // Условие построено из алгоритма, описанного на Wiki вот тут: https://en.wikipedia.org/wiki/Shunting_yard_algorithm
            if (previousOperator is not null && ((previousOperator.HasBiggerPrecendenceThan(currentOperator)) ||
                currentOperator.HasSamePrecendenceThan(previousOperator) && currentOperator.Associativity == OperatorAssociativity.Left))
            {
                var poppedPreviousOperator = operatorsStack.Pop();
                result = AppendToken(result, poppedPreviousOperator);
            }

            operatorsStack.Push(token);
        }

        if (IsThereOpeningParenthesisAtTheTopOf(operatorsStack))
            return ConvertionResult.CreateError(ParenthesesMismatchErrorMessage);

        if (result == string.Empty)
            return ConvertionResult.CreateError("Invalid expression");
        
        while (operatorsStack.Any())
            result = AppendToken(result, operatorsStack.Pop());

        // Если результат содержит скобки, чего не может быть в выражении RPN,
        // то это также означает, что инфиксное выражение на входе содержит неправильные скобки
        if (ContainsParentheses(result))
            return ConvertionResult.CreateError(ParenthesesMismatchErrorMessage);

        result = result.Trim();

        return ConvertionResult.CreateSuccess(result);
    }

    /// <summary>
    /// Проверяет строку на наличие скобок
    /// </summary>
    /// <param name="value">Строка, которую надо проверить</param>
    private bool ContainsParentheses(string value) =>
        value.Contains('(') || value.Contains(')');

    /// <summary>
    /// Проверяет, есть ли на вершине стека открывающая скобка
    /// </summary>
    /// <param name="operatorsStack">Стек</param>
    /// <returns>
    /// <see langword="true" />, если на вершине стека лежит открывающая скобка, иначе <see langword="false" />
    /// </returns>
    private bool IsThereOpeningParenthesisAtTheTopOf(Stack<string> operatorsStack)
    {
        string? topElement = null;
        operatorsStack.TryPeek(out topElement);

        return topElement == "(";
    }

    /// <summary>
    /// Прикрепляет токен к строке. Оставляет один пробел после токена
    /// </summary>
    /// <param name="initialString">Строка, к которой нужно прикрепить токен</param>
    /// <param name="token">Токен, который нужно прикрепить</param>
    /// <returns>
    /// Возвращает изначальную строку <paramref name="initialString" /> с добавленным в конец токеном <paramref name="token" />
    /// </returns>
    private string AppendToken(string initialString, string token)
    {
        token = token.Trim();

        if (token.Count(c => c == ' ') > 0)
            throw new InvalidOperationException("Token can not contain whitespaces");

        return initialString + token + " ";
    }

    /// <summary>
    /// Возвращает список операторов из стека операторов через пробел, которые предшествуют открывающейся скобке
    /// </summary>
    /// <param name="operatorsStack">Стек операторов</param>
    /// <returns>
    /// Списоко операторов через пробел или <see langword="null" />, если скобки в выражении
    /// были расставлены неверно
    /// </returns>
    /// <remarks>
    /// В случае успешного выполнения отбрасывает найденную открывающую скобку из стека операторов
    /// </remakrs>
    private string? GetOperatorsInParentheses(Stack<string> operatorsStack)
    {
        if (operatorsStack.Any() is false)
            return null;

        var result = string.Empty;

        while (operatorsStack.Any())
        {
            var @operator = operatorsStack.Pop();

            if (_parser.IsClosingParenthesis(@operator))
                return null;

            if (_parser.IsOpeningParenthesis(@operator))
                return result;

            result = AppendToken(result, @operator);
        }

        return null;
    }
}