using System.Collections.ObjectModel;
using CartridgeManagementSystem.Models;
using System.Data.SQLite;
using System.Windows.Input;
using System.Windows;

namespace CartridgeManagementSystem.ViewModels
{
    public class EditorViewModel : BaseViewModel
    {
        public ObservableCollection<Cartridge> Cartridges { get; set; }
        public ICommand AddCartridgeCommand { get; set; }
        public ICommand DeleteCartridgeCommand { get; set; }

        public EditorViewModel()
        {
            Cartridges = new ObservableCollection<Cartridge>();
            AddCartridgeCommand = new RelayCommand(AddCartridge);
            DeleteCartridgeCommand = new RelayCommand(DeleteCartridge);

            LoadCartridges();
        }

        public void LoadCartridges()
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=cartridge_management.db"))
            {
                conn.Open();
                string query = "SELECT * FROM Cartridges";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Cartridges.Add(new Cartridge
                            {
                                Id = reader.GetInt32(0),
                                Type = reader.GetString(1),
                                Model = reader.GetString(2),
                                SerialNumber = reader.GetString(3),
                                InstallationDate = reader.GetString(4),
                                Status = reader.GetString(5),
                                Comment = reader.GetString(6)
                            });
                        }
                    }
                }
            }
        }

        private void AddCartridge(object parameter)
        {
            var newCartridge = new Cartridge
            {
                Type = "Новый тип", // Эти значения должны быть заменены на реальные данные из интерфейса
                Model = "Новая модель",
                SerialNumber = "12345",
                InstallationDate = "2024-01-01",
                Status = "На складе",
                Comment = "Комментарий"
            };

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=cartridge_management.db"))
            {
                conn.Open();
                string query = "INSERT INTO Cartridges (Type, Model, SerialNumber, InstallationDate, Status, Comment) VALUES (@Type, @Model, @SerialNumber, @InstallationDate, @Status, @Comment)";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Type", newCartridge.Type);
                    cmd.Parameters.AddWithValue("@Model", newCartridge.Model);
                    cmd.Parameters.AddWithValue("@SerialNumber", newCartridge.SerialNumber);
                    cmd.Parameters.AddWithValue("@InstallationDate", newCartridge.InstallationDate);
                    cmd.Parameters.AddWithValue("@Status", newCartridge.Status);
                    cmd.Parameters.AddWithValue("@Comment", newCartridge.Comment);

                    cmd.ExecuteNonQuery();
                }
            }

            Cartridges.Add(newCartridge);
        }

        private void DeleteCartridge(object parameter)
        {
            if (parameter is Cartridge cartridge)
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=cartridge_management.db"))
                {
                    conn.Open();
                    string query = "DELETE FROM Cartridges WHERE Id=@Id";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", cartridge.Id);
                        cmd.ExecuteNonQuery();
                    }
                }

                Cartridges.Remove(cartridge);
            }
            else
            {
                MessageBox.Show("Выберите картридж для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
