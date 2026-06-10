using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Реализация стека с дополнительными возможностями на базе обычного массива.
/// </summary>
/// <typeparam name="T">Тип элементов стека.</typeparam>
public class SmartStack<T> : IEnumerable<T>
{
    private T[] _items;       // внутренний массив
    private int _count;       // количество элементов

    // Конструктор без параметров – ёмкость 4
    public SmartStack()
        : this(4)
    {
    }

    // Конструктор с указанием начальной ёмкости
    public SmartStack(int capacity)
    {
        if (capacity < 0)
            throw new ArgumentException("Ёмкость не может быть отрицательной.", nameof(capacity));

        _items = capacity > 0 ? new T[capacity] : Array.Empty<T>();
        _count = 0;
    }

    // Конструктор, принимающий коллекцию IEnumerable<T>
    // Последний элемент коллекции становится вершиной стека.
    public SmartStack(IEnumerable<T> collection)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        var temp = Array.Empty<T>();
        int tempCount = 0;
        foreach (var item in collection)
        {
            if (tempCount == temp.Length)
            {
                // Увеличиваем ёмкость временного массива
                int newCapacity = temp.Length == 0 ? 4 : temp.Length * 2;
                var newTemp = new T[newCapacity];
                Array.Copy(temp, 0, newTemp, 0, tempCount);
                temp = newTemp;
            }
            temp[tempCount++] = item;
        }

        // Теперь temp содержит все элементы, в порядке перечисления коллекции.
        // Нам нужно, чтобы последний элемент коллекции был на вершине стека.
        // Следовательно, заполняем внутренний массив с конца.
        _items = new T[tempCount];
        _count = tempCount;
        for (int i = 0; i < tempCount; i++)
        {
            _items[i] = temp[i];
        }
        // Порядок: первый элемент коллекции - в индексе 0 (основание), последний - в индексе tempCount-1 (вершина).
        // Всё верно.
    }

    // Свойство Count
    public int Count => _count;

    // Свойство Capacity (ёмкость внутреннего массива)
    public int Capacity => _items.Length;

    // Метод Push – добавляет элемент на вершину
    public void Push(T item)
    {
        if (_count == _items.Length)
        {
            // Увеличиваем ёмкость вдвое (минимально 4, если массив пуст)
            int newCapacity = _items.Length == 0 ? 4 : _items.Length * 2;
            var newArray = new T[newCapacity];
            Array.Copy(_items, 0, newArray, 0, _count);
            _items = newArray;
        }
        _items[_count] = item;
        _count++;
    }

    // Метод PushRange – добавляет коллекцию в обратном порядке (последний элемент коллекции становится вершиной)

    public void PushRange(IEnumerable<T> collection)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));


        foreach (T item in collection)
            Push(item);
    }

    // Метод Pop – удаляет и возвращает элемент с вершины
    public T Pop()
    {
        if (_count == 0)
            throw new InvalidOperationException("Стек пуст. Операция Pop невозможна.");

        _count--;
        T item = _items[_count];
        _items[_count] = default(T); // освобождаем ссылку для GC (если T ссылочный)
        return item;
    }

    // Метод Peek – возвращает элемент с вершины без удаления
    public T Peek()
    {
        if (_count == 0)
            throw new InvalidOperationException("Стек пуст. Операция Peek невозможна.");

        return _items[_count - 1];
    }

    // Метод Contains – проверяет наличие элемента
    public bool Contains(T item)
    {
        // Линейный поиск по массиву (только заполненная часть)
        for (int i = 0; i < _count; i++)
        {
            if (Equals(_items[i], item))
                return true;
        }
        return false;
    }

    // Реализация IEnumerable<T> – обход от вершины к основанию
    public IEnumerator<T> GetEnumerator()
    {
        for (int i = _count - 1; i >= 0; i--)
        {
            yield return _items[i];
        }
    }

    // Необобщённый GetEnumerator
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    // Опциональный индексатор по глубине (0 – вершина)
    public T this[int depth]
    {
        get
        {
            if (depth < 0 || depth >= _count)
                throw new ArgumentOutOfRangeException(nameof(depth), "Глубина выходит за границы стека.");
            return _items[_count - 1 - depth];
        }
    }
}