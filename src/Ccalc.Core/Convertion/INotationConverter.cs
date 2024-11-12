namespace Ccalc.Core.Convertion;

/// <summary>
/// Интерфейс объекта, который преобразует математическое выражение в инфиксной
/// нотации в постфиксную (польскую)
/// </summary>
public interface INotationConverter
{
    /// <summary>
    /// Преобразует математическое выражение из инфиксной нотации в постфиксную
    /// </summary>
    /// <param name="infixNotation">Инфиксная нотация</param>
    /// <returns>Постфиксная нотация</returns>
    public ConvertionResult ToPostfix(string infixNotation);
}