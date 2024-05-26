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
