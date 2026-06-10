-- 1. Выборка с фильтрацией и сортировкой
SELECT FullName, Email, RegistrationDate
FROM Members
WHERE IsActive = true
ORDER BY RegistrationDate DESC;

-- 2. Обновление данных
UPDATE Workouts
SET Price = 450.00
WHERE WorkoutName = 'Кардио';

-- 3. Удаление данных
DELETE FROM Members WHERE Email = 'alex@mail.com';

-- 4. Выборка с группировкой (сумма платежей по участникам)
SELECT 
    m.FullName,
    COUNT(p.PaymentID) AS PaymentCount,
    SUM(p.Amount) AS TotalSpent
FROM Members m
LEFT JOIN Payments p ON m.MemberID = p.MemberID
GROUP BY m.MemberID, m.FullName;

-- 5. Левое соединение (LEFT JOIN)
SELECT m.FullName, p.Amount, p.PaymentDate
FROM Members m
LEFT JOIN Payments p ON m.MemberID = p.MemberID;

-- 6. Правое соединение (RIGHT JOIN)
SELECT m.FullName, p.Amount
FROM Members m
RIGHT JOIN Payments p ON m.MemberID = p.MemberID;

-- 7. Пересечение (INNER JOIN)
SELECT DISTINCT m.FullName
FROM Members m
INNER JOIN Payments p ON m.MemberID = p.MemberID;