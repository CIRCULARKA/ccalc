namespace Ccalc.Core.Evaluation;

/// <summary>
/// Результат операции вычисления математического выражения
/// </summary>
public class EvaluationResult : OperationResult
{
    /// <summary>
    /// Результат вычисления
    /// </summary>
    public double? Result { get; private set; }

    /// <summary>
    /// Создаёт результат, сигнализирующий об успешности операции
    /// </summary>
    /// <param name="result">Результат операции</param>
    public static EvaluationResult CreateSuccess(double result)
    {
        return new EvaluationResult
        {
            Result = result,
            IsSuccessful = true,
            ErrorMessage = null
        };
    }

    /// <summary>
    /// Создаёт объект, сигнализирующий об ошибке операции
    /// </summary>
    /// <param name="message">Сообщение об ошибке</param>
    public static EvaluationResult CreateError(string message)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(message);

        return new EvaluationResult
        {
            Result = null,
            IsSuccessful = false,
            ErrorMessage = message
        };
    }
}