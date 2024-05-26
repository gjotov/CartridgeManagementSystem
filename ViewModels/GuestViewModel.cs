using System.Collections.ObjectModel;
using CartridgeManagementSystem.Models;
using System.Data.SQLite;
using System.Windows.Input;

namespace CartridgeManagementSystem.ViewModels
{
    public class GuestViewModel : BaseViewModel
    {
        private string _searchQuery;
        public string SearchQuery
        {
            get { return _searchQuery; }
            set
            {
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));
            }
        }

        public ObservableCollection<Cartridge> Cartridges { get; set; }

        public ICommand SearchCommand { get; }

        public GuestViewModel()
        {
            Cartridges = new ObservableCollection<Cartridge>();
            SearchCommand = new RelayCommand(Search);
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

        private void Search(object parameter)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=cartridge_management.db"))
            {
                conn.Open();
                string query = "SELECT * FROM Cartridges WHERE Model LIKE @searchQuery";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@searchQuery", $"%{SearchQuery}%");
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        Cartridges.Clear();
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
    }
}

