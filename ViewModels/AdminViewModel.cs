using System.Collections.ObjectModel;
using System.Windows.Input;
using CartridgeManagementSystem.Models;
using System.Data.SQLite;

namespace CartridgeManagementSystem.ViewModels
{
    public class AdminViewModel : BaseViewModel
    {
        private ObservableCollection<Cartridge> _cartridges;
        public ObservableCollection<Cartridge> Cartridges
        {
            get { return _cartridges; }
            set
            {
                _cartridges = value;
                OnPropertyChanged(nameof(Cartridges));
            }
        }

        public ICommand AddCartridgeCommand { get; }
        public ICommand DeleteCartridgeCommand { get; }

        public AdminViewModel()
        {
            Cartridges = new ObservableCollection<Cartridge>();
            LoadCartridges();
            AddCartridgeCommand = new RelayCommand(AddCartridge);
            DeleteCartridgeCommand = new RelayCommand(DeleteCartridge, CanDeleteCartridge);
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
                Type = "Новый тип",
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

        private bool CanDeleteCartridge(object parameter)
        {
            return parameter != null;
        }

        private void DeleteCartridge(object parameter)
        {
            if (parameter is Cartridge cartridge)
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=cartridge_management.db"))
                {
                    conn.Open();
                    string query = "DELETE FROM Cartridges WHERE Id = @id";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", cartridge.Id);
                        cmd.ExecuteNonQuery();
                    }
                }
                Cartridges.Remove(cartridge);
            }
        }
    }
}
