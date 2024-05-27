using System.Collections.ObjectModel;
using System.Windows.Input;
using CartridgeManagementSystem.Models;
using CartridgeManagementSystem.Views;
using System.Data.SQLite;


namespace CartridgeManagementSystem.ViewModels
{
    public class AdminViewModel : BaseViewModel
    {
        public ObservableCollection<Cartridge> Cartridges { get; set; }
        public ObservableCollection<User> Users { get; set; }

        private User _selectedUser;
        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }

        public ICommand AddUserCommand { get; set; }
        public ICommand DeleteUserCommand { get; set; }
        public ICommand ChangeUserRoleCommand { get; set; }

        public AdminViewModel()
        {
            Cartridges = new ObservableCollection<Cartridge>();
            Users = new ObservableCollection<User>();

            LoadCartridges();
            LoadUsers();

            AddUserCommand = new RelayCommand(AddUser);
            DeleteUserCommand = new RelayCommand(DeleteUser);
            ChangeUserRoleCommand = new RelayCommand(ChangeUserRole);
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

        private void LoadUsers()
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=cartridge_management.db"))
            {
                conn.Open();
                string query = "SELECT * FROM Users";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Users.Add(new User
                            {
                                Id = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                Password = reader.GetString(2),
                                Role = reader.GetString(3)
                            });
                        }
                    }
                }
            }
        }

        private void AddUser(object parameter)
        {
            var addUserWindow = new AddUserWindow();
            if (addUserWindow.ShowDialog() == true)
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=cartridge_management.db"))
                {
                    conn.Open();
                    string query = "INSERT INTO Users (Username, Password, Role) VALUES (@username, @password, @role)";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", addUserWindow.UsernameTextBox);
                        cmd.Parameters.AddWithValue("@password", addUserWindow.PasswordTextBox);
                        cmd.Parameters.AddWithValue("@role", addUserWindow.RoleComboBox);
                        cmd.ExecuteNonQuery();
                    }
                }
                LoadUsers();
            }
        }

        private void DeleteUser(object parameter)
        {
            if (SelectedUser != null)
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=cartridge_management.db"))
                {
                    conn.Open();
                    string query = "DELETE FROM Users WHERE Id=@id";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", SelectedUser.Id);
                        cmd.ExecuteNonQuery();
                    }
                }
                Users.Remove(SelectedUser);
            }
        }

        private void ChangeUserRole(object parameter)
        {
            if (SelectedUser != null)
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=cartridge_management.db"))
                {
                    conn.Open();
                    string query = "UPDATE Users SET Role=@role WHERE Id=@id";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@role", SelectedUser.Role);
                        cmd.Parameters.AddWithValue("@id", SelectedUser.Id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
