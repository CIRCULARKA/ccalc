namespace Ccalc.Core.Parsing;

/// <summary>
/// Интерфейс объекта, который имеет функции по
/// парсингу математических выражений
/// </summary>
public interface IMathExpressionParser
{
    /// <summary>
    /// Определяет, является ли токен операндом, т.е. числом
    /// </summary>
    /// <param name="token">Токен, который нужно проверить</param>
    public bool IsOperand(string token);
    
    /// <summary>
    /// Создаёт стэк, в котором самый последний токен математического выражения находится на дне, 
    /// а самый первый - на вершине
    /// </summary>
    /// <param name="expression">Выражение, содержащее токены</param>
    public Stack<string> GetTokenStack(string expression);
}