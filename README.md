# CartridgeManagementSystem

Система управления картриджами (Cartridge Management System, CMS) - это WPF-приложение, предназначенное для управления инвентарем картриджей, отслеживания обслуживания и ремонта, а также предоставления аналитики. Система поддерживает три роли: Администратор, Редактор и Гость, каждая из которых обладает определенными правами.

## Используемые технологии
- ![.NET Core](https://img.shields.io/badge/.NET_Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white) 
- ![WPF](https://img.shields.io/badge/WPF-0078D7?style=for-the-badge&logo=microsoft&logoColor=white) 
- ![MVVM](https://img.shields.io/badge/MVVM-512BD4?style=for-the-badge&logo=visualstudio&logoColor=white) 
- ![XML](https://img.shields.io/badge/XML-FF6600?style=for-the-badge&logo=xml&logoColor=white)
- ![SQLite](https://img.shields.io/badge/SQLite-003B57?style=for-the-badge&logo=sqlite&logoColor=white)

## Функциональные возможности

### Управление запасами
- Отслеживание картриджей, включая тип, модель, серийный номер, дату установки и статус (в использовании, пустой, в ремонте и т.д.)
- Предоставление информации о доступных картриджах, их количестве и типах

### Управление ремонтом и обслуживанием
- Запись картриджей на ремонт
- Отслеживание состояния ремонта и истории ремонтов
- Управление гарантийным обслуживанием картриджей

### Аналитика
- Предоставление аналитики по стоимости ремонта за определенный период

### Роли пользователей
 **Администратор:**
  
- Добавление, удаление и обновление картриджей
- Изменение состояния картриджей
- Добавление новых пользователей (администраторов или редакторов)
- Просмотр истории картриджей
- Отправка картриджей на ремонт
- Оставление комментариев по картриджам
- Просмотр аналитики по картриджам
  
 **Редактор:**
  
- Добавление, удаление и обновление картриджей
- Изменение состояния картриджей
- Просмотр истории картриджей
- Отправка картриджей на ремонт
- Оставление комментариев по картриджам
- Просмотр аналитики по картриджам
  
 **Гость (авторизация не требуется):**
  
- Поиск картриджей
- Просмотр истории картриджей

## Руководство пользователя: Система управления картриджами

### Начало работы

1. **Установка приложения**:
   - Клонируйте репозиторий с помощью Git.
   - Откройте проект в вашей среде разработки (например, Visual Studio).
   - Восстановите пакеты NuGet, если это необходимо.
   - Соберите проект.

2. **Запуск приложения**:
   - Запустите приложение, открыв файл решения `CartridgeManagementSystem.sln`.
   - Запустите приложение с помощью средств вашей среды разработки (например, нажмите F5 в Visual Studio).

### Авторизация

1. **Вход в систему**:
   - При первом запуске приложения вы будете перенаправлены на экран авторизации.
   - Введите ваше имя пользователя и пароль в соответствующие поля.
   - Нажмите кнопку "Войти".

2. **Выбор роли**:
   - После успешной авторизации вы увидите экран выбора роли.
   - Выберите роль, соответствующую вашим полномочиям: "Администратор", "Редактор" или "Гость".

### Использование приложения

#### Панель администратора

- **Управление картриджами**:
   - Добавление новых картриджей: нажмите кнопку "Добавить картридж" и заполните необходимые поля.
   - Удаление картриджей: выберите картридж из списка и нажмите кнопку "Удалить картридж".
   - Изменение состояния картриджей: обновите состояние выбранных картриджей.

#### Панель редактора

- **Управление картриджами**:
   - Аналогично панели администратора, но без возможности управления пользователями.

#### Панель гостя

- **Просмотр информации**:
   - Поиск картриджей: введите критерии поиска и нажмите кнопку "Поиск" для поиска картриджей.
   - Просмотр истории: просмотрите историю использования картриджей.

## Видеодемонстрация

Посмотрите видеодемонстрацию использования приложения по [этой ссылке](https://drive.google.com/drive/folders/1EGf4UhbwXmcfJeC6pGFpDTzVQXaufeNT?usp=sharing), чтобы более наглядно понять его функциональность и возможности.

# База данных
В этом проекте в качестве базы данных может быть использована SQLite.

### Схема базы данных
![](https://github.com/gjotov/CartridgeManagementSystem/blob/master/CartridgeDB.png)

### База данных состоит из следующих таблиц:

**Users**
- `Id`: Уникальный идентификатор пользователя (первичный ключ).
- `Username`: Имя пользователя.
- `Password`: Пароль пользователя.
- `RoleId:` Идентификатор роли пользователя (внешний ключ, ссылается на таблицу Roles).

**Roles**
- `Id`: Уникальный идентификатор роли (первичный ключ).
- `RoleName`: Название роли пользователя (например, "Admin", "Editor", "Guest").

**Cartridges**
- `Id`: Уникальный идентификатор картриджа (первичный ключ).
- `Type`: Тип картриджа (например, "Черный", "Цветной").
- `Model`: Модель картриджа.
- `SerialNumber`: Серийный номер картриджа.
- `InstallationDate`: Дата установки картриджа.
- `Status`: Статус картриджа (например, "В использовании", "На складе", "В ремонте").
- `Comment`: Дополнительный комментарий о картридже.
- `UserId`: Идентификатор пользователя, к которому привязан данный картридж (внешний ключ, ссылается на таблицу Users).

### Интеграция SQLite в C#
```csharp
using System.Data.SQLite;

namespace CartridgeManagementSystem
{
    public class DatabaseInitializer
    {
        public static void InitializeDatabase()
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=cartridge_management.db"))
            {
                conn.Open();

                string createUsersTableQuery = @"
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT NOT NULL,
                Password TEXT NOT NULL,
                Role TEXT NOT NULL
            )";
                using (SQLiteCommand cmd = new SQLiteCommand(createUsersTableQuery, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                string createCartridgesTableQuery = @"
            CREATE TABLE IF NOT EXISTS Cartridges (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Type TEXT NOT NULL,
                Model TEXT NOT NULL,
                SerialNumber TEXT NOT NULL,
                InstallationDate TEXT NOT NULL,
                Status TEXT NOT NULL,
                Comment TEXT
            )";
                using (SQLiteCommand cmd = new SQLiteCommand(createCartridgesTableQuery, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void SeedData(SQLiteConnection conn)
        {
            string insertCartridges = @"
                INSERT INTO Cartridges (Type, Model, SerialNumber, InstallationDate, Status, Comment)
                VALUES
                ('Laser', 'HP 123', 'SN123', '2024-01-01', 'В использовании', 'Первый картридж'),
                ('Inkjet', 'Canon 456', 'SN456', '2024-02-01', 'На полке', 'Второй картридж');";

            string insertUsers = @"
                INSERT INTO Users (Username, Password, Role)
                VALUES
                ('admin', 'adminpass', 'Administrator'),
                ('editor', 'editorpass', 'Editor');";

            string insertRepairs = @"
                INSERT INTO Repairs (CartridgeId, RepairDate, RepairStatus, RepairCost)
                VALUES
                (1, '2024-03-01', 'Отремонтировано', 100.0);";

            using (SQLiteCommand cmd = new SQLiteCommand(insertCartridges, conn))
            {
                cmd.ExecuteNonQuery();
            }

            using (SQLiteCommand cmd = new SQLiteCommand(insertUsers, conn))
            {
                cmd.ExecuteNonQuery();
            }

            using (SQLiteCommand cmd = new SQLiteCommand(insertRepairs, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}
```
