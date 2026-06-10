-- Создание таблиц для спортивного клуба

CREATE TABLE Members (
    MemberID UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    FullName VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    BirthDate DATE NOT NULL,
    IsActive BOOLEAN DEFAULT TRUE,
    RegistrationDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE Trainers (
    TrainerID UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    FullName VARCHAR(100) NOT NULL,
    Specialization VARCHAR(100),
    HireDate DATE DEFAULT CURRENT_DATE
);

CREATE TABLE Workouts (
    WorkoutID UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    WorkoutName VARCHAR(100) NOT NULL,
    DurationMinutes INT NOT NULL,
    Price DECIMAL(10,2) NOT NULL CHECK (Price > 0)
);

-- Связь многие-ко-многим между Members и Workouts
CREATE TABLE MemberWorkout (
    MemberID UUID REFERENCES Members(MemberID) ON DELETE CASCADE,
    WorkoutID UUID REFERENCES Workouts(WorkoutID) ON DELETE CASCADE,
    AttendanceDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (MemberID, WorkoutID, AttendanceDate)
);

-- Связь один-ко-многим: Members -> Payments
CREATE TABLE Payments (
    PaymentID UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    MemberID UUID NOT NULL REFERENCES Members(MemberID) ON DELETE CASCADE,
    Amount DECIMAL(10,2) NOT NULL CHECK (Amount > 0),
    PaymentDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    Description VARCHAR(255)
);

-- Уникальный индекс на Email уже создан через UNIQUE.
-- Дополнительный индекс для ускорения (не обязателен, но полезен)
CREATE INDEX idx_payments_date ON Payments(PaymentDate);