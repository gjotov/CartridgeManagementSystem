using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Input;
using CartridgeManagementSystem.Models;
using CartridgeManagementSystem.Views;

namespace CartridgeManagementSystem.ViewModels
{
    public class MainViewModel : BaseViewModel
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

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private string _role;
        public string Role
        {
            get { return _role; }
            set
            {
                _role = value;
                OnPropertyChanged(nameof(Role));
            }
        }

        public ICommand LoginCommand { get; set; }
        public ICommand GuestLoginCommand { get; set; }

        public MainViewModel()
        {
            Cartridges = new ObservableCollection<Cartridge>();
            LoginCommand = new RelayCommand(Login);
            GuestLoginCommand = new RelayCommand(LoginAsGuest);
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

        private void Login(object parameter)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=cartridge_management.db"))
                {
                    conn.Open();
                    string query = "SELECT Role FROM Users WHERE Username=@username AND Password=@password";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", Username);
                        cmd.Parameters.AddWithValue("@password", Password);
                        var result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            Role = result.ToString().Trim();  // Trim spaces
                            MessageBox.Show($"Вход разрешён. Роль: {Role}");
                            OpenRoleSpecificWindow(Role);
                        }
                        else
                        {
                            MessageBox.Show("Неверный логин или пароль.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void LoginAsGuest(object parameter)
        {
            OpenGuestWindow();
        }

        private void OpenRoleSpecificWindow(string role)
        {
            Window window = null;
            if (role == "Administrator")
            {
                MessageBox.Show("Открытие окна администратора");
                window = new AdminWindow();
                ((AdminViewModel)window.DataContext).LoadCartridges(); 
            }
            else if (role == "Editor")
            {
                MessageBox.Show("Открытие окна редактора");
                window = new EditorWindow();
                ((EditorViewModel)window.DataContext).LoadCartridges();
            }
            else if (role == "Guest")
            {
                MessageBox.Show("Открытие окна гостя");
                OpenGuestWindow();
            }
            else
            {
                MessageBox.Show($"Нет доступных окон для роли: {role}");
            }

            if (window != null)
            {
                window.Show();
                Application.Current.MainWindow.Close();
            }
        }

        private void OpenGuestWindow()
        {
            var guestWindow = new GuestWindow();
            ((GuestViewModel)guestWindow.DataContext).LoadCartridges();
            guestWindow.Show();
        }
    }
}
