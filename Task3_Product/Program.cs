using System;

/// <summary>
/// Класс, представляющий товар с основными характеристиками.
/// </summary>
public class Product
{
    // Поля (приватные, инкапсуляция)
    private string _name;
    private string _manufacturer;
    private decimal _price;
    private int _expirationDays;
    private DateTime _productionDate;

    /// <summary>
    /// Конструктор для создания товара.
    /// </summary>
    /// <param name="name">Наименование товара (не пустое).</param>
    /// <param name="manufacturer">Производитель (не пустой).</param>
    /// <param name="price">Цена (неотрицательная).</param>
    /// <param name="expirationDays">Срок годности в днях (положительный).</param>
    /// <param name="productionDate">Дата производства (не будущая).</param>
    public Product(string name, string manufacturer, decimal price, int expirationDays, DateTime productionDate)
    {
        Name = name;
        Manufacturer = manufacturer;
        Price = price;
        ExpirationDays = expirationDays;
        ProductionDate = productionDate;
    }

    // Публичные свойства с валидацией (инкапсуляция)

    public string Name
    {
        get => _name;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Наименование товара не может быть пустым.", nameof(value));
            _name = value;
        }
    }

    public string Manufacturer
    {
        get => _manufacturer;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Производитель не может быть пустым.", nameof(value));
            _manufacturer = value;
        }
    }

    public decimal Price
    {
        get => _price;
        private set
        {
            if (value < 0)
                throw new ArgumentException("Цена не может быть отрицательной.", nameof(value));
            _price = value;
        }
    }

    public int ExpirationDays
    {
        get => _expirationDays;
        private set
        {
            if (value <= 0)
                throw new ArgumentException("Срок годности должен быть положительным числом дней.", nameof(value));
            _expirationDays = value;
        }
    }

    public DateTime ProductionDate
    {
        get => _productionDate;
        private set
        {
            // Проверяем, что дата производства не в будущем
            if (value.Date > DateTime.Now.Date)
                throw new ArgumentException("Дата производства не может быть в будущем.", nameof(value));
            _productionDate = value;
        }
    }

    /// <summary>
    /// Вычисляет дату истечения срока годности.
    /// </summary>
    public DateTime GetExpirationDate()
    {
        return ProductionDate.AddDays(ExpirationDays);
    }

    /// <summary>
    /// Показывает, истёк ли срок годности на текущий момент.
    /// </summary>
    public bool IsExpired
    {
        get
        {
            return DateTime.Now.Date > GetExpirationDate().Date;
        }
    }

    /// <summary>
    /// Переопределённый метод ToString для удобного вывода информации о товаре.
    /// </summary>
    public override string ToString()
    {
        return $"Наименование: {Name}\n" +
               $"Производитель: {Manufacturer}\n" +
               $"Цена: {Price:F2} руб.\n" +
               $"Дата производства: {ProductionDate:dd.MM.yyyy}\n" +
               $"Срок годности: {ExpirationDays} дн.\n" +
               $"Годен до: {GetExpirationDate():dd.MM.yyyy}\n" +
               $"Статус: {(IsExpired ? "ПРОСРОЧЕН" : "ГОДЕН")}";
    }
}