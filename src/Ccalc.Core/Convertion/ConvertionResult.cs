namespace Ccalc.Core.Convertion;

/// <summary>
/// Результат преобразования математического выражения
/// </summary>
public class ConvertionResult : OperationResult
{
    /// <summary>
    /// Результат преобразования
    /// </summary>
    public string? Result { get; private set; }

    /// <summary>
    /// Создаёт результат, сигнализирующий об успешности операции
    /// </summary>
    /// <param name="result">Результат операции</param>
    public static ConvertionResult CreateSuccess(string result)
    {
        return new ConvertionResult
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
    public static ConvertionResult CreateError(string message)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(message);

        return new ConvertionResult
        {
            Result = null,
            IsSuccessful = false,
            ErrorMessage = message
        };
    }
}