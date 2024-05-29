using System.Windows;
using System.Windows.Controls;

namespace CartridgeManagementSystem.Views
{
    public partial class AddUserWindow : Window
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Role { get; private set; }

        public AddUserWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Username = UsernameTextBox.Text;
            Password = PasswordTextBox.Text;
            Role = ((ComboBoxItem)RoleComboBox.SelectedItem).Content.ToString();
            DialogResult = true;
        }
    }
}
