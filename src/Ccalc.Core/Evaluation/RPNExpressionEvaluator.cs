namespace Ccalc.Core.Evaluation;

/// <summary>
/// Объект для расчёта математических выражений из обратной польской нотации
/// </summary>
public class RPNExpressionEvaluator : IExpressionEvaluator
{
    private const string WrongExpressionErrorMessage = "The expression is in wrong format";

    private readonly OperatorFactory _operators;

    public RPNExpressionEvaluator(OperatorFactory operators)
    {
        _operators = operators;
    }

    public EvaluationResult Evaluate(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
            return EvaluationResult.CreateError("Expression can not be empty");

        var tokenStack = GetTokenStack(expression);

        var operandsStack = new Stack<double>();

        while (tokenStack.Any())
        {
            var token = tokenStack.Pop();

            if (IsOperand(token))
            {
                operandsStack.Push(double.Parse(token));
                continue;
            }

            var @operator = _operators.GetOperator(token);
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

    /// <summary>
    /// Определяет, является ли токен операндом, т.е. числом
    /// </summary>
    private bool IsOperand(string token)
    {
        double result = default;
        return double.TryParse(token, out result);
    }
    
    /// <summary>
    /// Создаёт стэк, в котором самый последний токен находится на дне, 
    /// а самый первый - на вершине
    /// </summary>
    /// <param name="expression">Выражение, содержащее токены</param>
    private Stack<string> GetTokenStack(string expression)
    {
        var tokens = expression.Split(' ').Reverse();

        var result = new Stack<string>();

        foreach (var token in tokens)
            result.Push(token);

        return result;
    }
}