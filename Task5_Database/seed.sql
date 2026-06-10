-- Заполнение тестовыми данными

-- Участники
INSERT INTO Members (FullName, Email, BirthDate, IsActive) VALUES
('Иван Петров', 'ivan@mail.com', '1990-05-12', true),
('Мария Сидорова', 'maria@mail.com', '1985-08-23', true),
('Алексей Иванов', 'alex@mail.com', '2000-01-30', false);

-- Тренеры
INSERT INTO Trainers (FullName, Specialization, HireDate) VALUES
('Ольга Смирнова', 'Йога', '2020-03-01'),
('Дмитрий Козлов', 'Силовой тренинг', '2021-06-15');

-- Виды тренировок
INSERT INTO Workouts (WorkoutName, DurationMinutes, Price) VALUES
('Йога утро', 60, 500.00),
('Силовая', 90, 700.00),
('Кардио', 45, 400.00);

-- Связи участников с тренировками (используем подзапросы для получения UUID)
INSERT INTO MemberWorkout (MemberID, WorkoutID, AttendanceDate)
VALUES
((SELECT MemberID FROM Members WHERE Email='ivan@mail.com'), (SELECT WorkoutID FROM Workouts WHERE WorkoutName='Йога утро'), '2026-06-01 10:00:00'),
((SELECT MemberID FROM Members WHERE Email='ivan@mail.com'), (SELECT WorkoutID FROM Workouts WHERE WorkoutName='Силовая'), '2026-06-03 18:00:00'),
((SELECT MemberID FROM Members WHERE Email='maria@mail.com'), (SELECT WorkoutID FROM Workouts WHERE WorkoutName='Кардио'), '2026-06-02 09:30:00');

-- Платежи
INSERT INTO Payments (MemberID, Amount, PaymentDate, Description)
VALUES
((SELECT MemberID FROM Members WHERE Email='ivan@mail.com'), 1200.00, '2026-06-01 12:00:00', 'Абонемент на 2 занятия'),
((SELECT MemberID FROM Members WHERE Email='maria@mail.com'), 400.00, '2026-06-02 10:00:00', 'Разовое занятие кардио');