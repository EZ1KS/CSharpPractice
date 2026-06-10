using System;

/// <summary>
/// Класс для рисования ромба (диаманта) из символов X.
/// </summary>
public static class DiamondDrawer
{
    /// <summary>
    /// Выводит на консоль ромб заданного размера.
    /// </summary>
    /// <param name="n">Длина диагонали (положительное нечётное число).</param>
    /// <exception cref="ArgumentException">
    /// Выбрасывается, если n не является положительным нечётным числом.
    /// </exception>
    public static void Draw(int n)
    {
        // Проверка входных данных
        if (n <= 0)
        {
            throw new ArgumentException("Размер диагонали должен быть положительным.", nameof(n));
        }

        if (n % 2 == 0)
        {
            throw new ArgumentException("Размер диагонали должен быть нечётным.", nameof(n));
        }

        int center = n / 2; // Центральная строка (индекс)

        for (var i = 0; i < n; i++)
        {
            var offset = Math.Abs(center - i); // Отступ слева и справа

            // Создаём строку как массив символов
            var lineChars = new char[n];

            // Заполняем пробелами
            for (var j = 0; j < n; j++)
            {
                lineChars[j] = ' ';
            }

            // Ставим первый X (левый)
            lineChars[offset] = 'X';

            // Если это не крайняя строка (верх/низ) – ставим второй X
            if (offset != center)
            {
                lineChars[n - 1 - offset] = 'X';
            }

            Console.WriteLine(new string(lineChars));
        }
    }
}