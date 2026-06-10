using System;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Программа учёта товаров ===\n");

        try
        {
            // Ввод данных с консоли
            Console.Write("Введите наименование товара: ");
            string name = Console.ReadLine();

            Console.Write("Введите производителя: ");
            string manufacturer = Console.ReadLine();

            Console.Write("Введите цену (в рублях): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                Console.WriteLine("Ошибка: неверный формат цены.");
                return;
            }

            Console.Write("Введите срок годности (в днях): ");
            if (!int.TryParse(Console.ReadLine(), out int expirationDays))
            {
                Console.WriteLine("Ошибка: неверный формат срока годности.");
                return;
            }

            Console.Write("Введите дату производства (в формате дд.мм.гггг): ");
            if (!DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime productionDate))
            {
                Console.WriteLine("Ошибка: неверный формат даты. Используйте дд.мм.гггг.");
                return;
            }

            // Создаём объект Product
            var product = new Product(name, manufacturer, price, expirationDays, productionDate);

            // Выводим информацию о товаре
            Console.WriteLine("\n--- Информация о товаре ---");
            Console.WriteLine(product.ToString());
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"\nОшибка при создании товара: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nНепредвиденная ошибка: {ex.Message}");
        }

        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}