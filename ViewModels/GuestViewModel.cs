using CartridgeManagementSystem.Models;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Windows.Input;

namespace CartridgeManagementSystem.ViewModels
{
    public class GuestViewModel : BaseViewModel
    {
        public ObservableCollection<Cartridge> Cartridges { get; set; }
        public ObservableCollection<Cartridge> FilteredCartridges { get; set; }

        private string _searchQuery;
        public string SearchQuery
        {
            get { return _searchQuery; }
            set
            {
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));
                SearchCartridges();
            }
        }

        public ICommand SearchCommand { get; set; }

        public GuestViewModel()
        {
            Cartridges = new ObservableCollection<Cartridge>();
            FilteredCartridges = new ObservableCollection<Cartridge>();

            LoadCartridges();

            SearchCommand = new RelayCommand(SearchCartridges);
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
                            var cartridge = new Cartridge
                            {
                                Id = reader.GetInt32(0),
                                Type = reader.GetString(1),
                                Model = reader.GetString(2),
                                SerialNumber = reader.GetString(3),
                                InstallationDate = reader.GetString(4),
                                Status = reader.GetString(5),
                                Comment = reader.GetString(6)
                            };
                            Cartridges.Add(cartridge);
                            FilteredCartridges.Add(cartridge);
                        }
                    }
                }
            }
        }

        private void SearchCartridges(object parameter = null)
        {
            FilteredCartridges.Clear();
            foreach (var cartridge in Cartridges.Where(c => c.Type.Contains(SearchQuery) || c.Model.Contains(SearchQuery) || c.SerialNumber.Contains(SearchQuery)))
            {
                FilteredCartridges.Add(cartridge);
            }
        }
    }
}
