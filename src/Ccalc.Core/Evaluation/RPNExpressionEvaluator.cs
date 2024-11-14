namespace Ccalc.Core.Evaluation;

/// <summary>
/// Объект для расчёта математических выражений из обратной польской нотации
/// </summary>
public class RPNExpressionEvaluator : IExpressionEvaluator
{
    private const string WrongExpressionErrorMessage = "The expression is in wrong format";

    private readonly List<Operator> _supportedOperators;

    private readonly IMathExpressionParser _parser;

    /// <summary>
    /// Создаёт экземпляр класса
    /// </summary>
    /// <param name="parser">Парсер, разбивающий выражения на токены</param>
    /// <param name="supportedOperators">Список поддерживаемых математических операций</param>
    public RPNExpressionEvaluator(List<Operator> supportedOperators, IMathExpressionParser parser)
    {
        ArgumentNullException.ThrowIfNull(supportedOperators);
        ArgumentNullException.ThrowIfNull(parser);

        if (supportedOperators.Any() is false)
            throw new ArgumentException("Supported operators list is empty", nameof(supportedOperators));

        _supportedOperators = supportedOperators;
        _parser = parser;
    }

    public EvaluationResult Evaluate(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
            return EvaluationResult.CreateError("Expression can not be empty");

        var tokenStack = _parser.GetTokenStack(expression.Trim());

        var operandsStack = new Stack<double>();

        while (tokenStack.Any())
        {
            var token = tokenStack.Pop();

            if (_parser.IsOperand(token))
            {
                operandsStack.Push(double.Parse(token));
                continue;
            }

            var @operator = _supportedOperators.FirstOrDefault(o => o.IsOperator(token));
            if (@operator is null)
                return EvaluationResult.CreateError($"Operation \"{token}\" is not supported");

            // Т.к. любой оператор работает только с двумя операндами,
            // то ситуация, когда мы находим оператор, но в стеке нет
            // минимум ДВУХ операндов означает то, что выражение составлено неверно
            if (operandsStack.Count < 2)
                return EvaluationResult.CreateError(WrongExpressionErrorMessage);

            var secondOperand = operandsStack.Pop();
            var firstOperand = operandsStack.Pop();

            var operationResult = @operator.Execute(firstOperand, secondOperand);

            operandsStack.Push(operationResult);
        }

        if (operandsStack.Count == 0 || operandsStack.Count > 1)
            return EvaluationResult.CreateError(WrongExpressionErrorMessage);

        return EvaluationResult.CreateSuccess(operandsStack.Pop());
    }
}