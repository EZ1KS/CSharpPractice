using System;
using System.Text;

/// <summary>
/// Класс с решением задания "Сложные проценты".
/// </summary>
public static class CompoundInterestCalculator
{
    /// <summary>
    /// Формирует строку с расчётом сложных процентов по годам.
    /// </summary>
    /// <param name="initialDeposit">Начальный вклад (положительное число).</param>
    /// <param name="years">Количество лет (положительное целое).</param>
    /// <param name="interestRate">Годовая процентная ставка (положительное число).</param>
    /// <returns>Строка с результатами по годам, каждая строка заканчивается переводом.</returns>
    public static string CalculateCompoundInterest(double initialDeposit, int years, double interestRate)
    {
        if (initialDeposit <= 0)
        {
            throw new ArgumentException("Начальный вклад должен быть положительным.", nameof(initialDeposit));
        }

        if (years <= 0)
        {
            throw new ArgumentException("Количество лет должно быть положительным.", nameof(years));
        }

        if (interestRate <= 0)
        {
            throw new ArgumentException("Процентная ставка должна быть положительной.", nameof(interestRate));
        }

        var sb = new StringBuilder();

        for (var year = 1; year <= years; year++)
        {
            double amount = initialDeposit * Math.Pow(1 + interestRate / 100, year);
            sb.AppendLine($"Год {year}: {amount.ToString("F2")} руб.");
        }

        return sb.ToString();
    }
}