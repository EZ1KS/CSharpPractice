using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Демонстрация SmartStack<T> ===\n");

        // 1. Конструктор без параметров (ёмкость 4)
        var stack1 = new SmartStack<int>();
        Console.WriteLine("Создан пустой стек (Capacity=4).");
        ShowStats(stack1);

        // 2. Push
        Console.WriteLine("Добавляем 1, 2, 3, 4, 5 (5 элементов, ёмкость увеличится вдвое):");
        for (int i = 1; i <= 5; i++)
            stack1.Push(i);
        ShowStats(stack1);

        // 3. Peek и Pop
        Console.WriteLine($"Peek: {stack1.Peek()}");
        Console.WriteLine($"Pop: {stack1.Pop()}");
        ShowStats(stack1);

        // 4. Contains
        Console.WriteLine($"Стек содержит 3? {stack1.Contains(3)}");
        Console.WriteLine($"Стек содержит 5? {stack1.Contains(5)}");

        // 5. Конструктор с указанием ёмкости 2
        var stack2 = new SmartStack<string>(2);
        Console.WriteLine("\nСоздан стек строк с ёмкостью 2.");
        stack2.Push("A");
        stack2.Push("B");
        Console.WriteLine($"После двух Push: Count={stack2.Count}, Capacity={stack2.Capacity}");
        stack2.Push("C"); // должно расшириться до 4
        Console.WriteLine($"После третьего Push: Count={stack2.Count}, Capacity={stack2.Capacity}");

        // 6. Конструктор от коллекции
        var list = new List<int> { 10, 20, 30, 40 }; // вершиной должен быть 40
        var stack3 = new SmartStack<int>(list);
        Console.WriteLine("\nСтек из коллекции {10,20,30,40}:");
        Console.WriteLine($"Count={stack3.Count}, Capacity={stack3.Capacity}");
        Console.Write("Содержимое (от вершины к основанию): ");
        foreach (var item in stack3)
            Console.Write(item + " ");
        Console.WriteLine(); // ожидаем: 40 30 20 10

        // 7. PushRange
        var range = new[] { 100, 200, 300 };
        Console.WriteLine("\nДобавляем PushRange {100,200,300} в стек stack1 (текущая вершина должна стать 300):");
        stack1.PushRange(range);
        ShowStats(stack1);
        Console.Write("Содержимое стека: ");
        foreach (var item in stack1)
            Console.Write(item + " ");
        Console.WriteLine();

        // 8. Проверка индексатора (если реализован)
        Console.WriteLine($"\nЭлемент на глубине 0 (вершина) = {stack1[0]}");
        Console.WriteLine($"Элемент на глубине 2 = {stack1[2]}");

        // 9. Перечисление foreach
        Console.WriteLine("\nПеребор foreach (от вершины к основанию):");
        foreach (var item in stack1)
        {
            Console.Write(item + " ");
        }
        Console.WriteLine();

        // 10. Обработка исключений
        Console.WriteLine("\nПопытка Pop из пустого стека:");
        var emptyStack = new SmartStack<double>();
        try
        {
            emptyStack.Pop();
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Исключение: {ex.Message}");
        }

        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }

    private static void ShowStats<T>(SmartStack<T> stack)
    {
        Console.WriteLine($"Count = {stack.Count}, Capacity = {stack.Capacity}");
    }
}