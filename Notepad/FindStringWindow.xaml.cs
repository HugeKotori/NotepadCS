using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Notepad
{
    /// <summary>
    /// FindStringWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FindStringWindow : Window
    {

        public MainWindow mainWindow { get; set; }


        public FindStringWindow()
        {
            InitializeComponent();
            this.WayDownRadioBox.IsChecked = true;
        }

        private void CancelFind(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void FindNextOne(object sender, RoutedEventArgs e)
        {
            if(this.WayDownRadioBox.IsChecked == true)
            {
                int TextBoxIndex = (mainWindow.NotepadTextBox.Text.Substring(mainWindow.NotepadTextBox.SelectionStart + mainWindow.NotepadTextBox.SelectionLength)).IndexOf(FindTextBox.Text);
                if (TextBoxIndex == -1)
                {
                    MessageBox.Show($"找不到\"{FindTextBox.Text}\"", "查找", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                mainWindow.NotepadTextBox.SelectionStart = mainWindow.NotepadTextBox.SelectionStart + mainWindow.NotepadTextBox.SelectionLength + TextBoxIndex;
                mainWindow.NotepadTextBox.SelectionLength = FindTextBox.Text.Length;
                mainWindow.NotepadTextBox.Focus();
                return;
            }
            else
            {
                int TextBoxIndex = mainWindow.NotepadTextBox.Text.IndexOf(FindTextBox.Text);
                int temp = 0;
                for (; TextBoxIndex < mainWindow.NotepadTextBox.SelectionStart;)
                {
                    temp = TextBoxIndex;
                    try
                    {
                        TextBoxIndex = temp + this.FindTextBox.Text.Length + mainWindow.NotepadTextBox.Text.Substring(temp + this.FindTextBox.Text.Length).IndexOf(FindTextBox.Text);
                    }
                    catch
                    {
                        break;
                    }
                }
                if(TextBoxIndex > mainWindow.NotepadTextBox.SelectionStart)
                {
                    MessageBox.Show($"找不到\"{FindTextBox.Text}\"", "查找", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                mainWindow.NotepadTextBox.SelectionStart = temp;
                mainWindow.NotepadTextBox.SelectionLength = FindTextBox.Text.Length;
                mainWindow.NotepadTextBox.Focus();
                return;
            }
        }

        private void WayUpRadioBoxChecked(object sender, RoutedEventArgs e)
        {
            WayDownRadioBox.IsChecked = false;
        }

        private void WayDownRadioBoxChecked(object sender, RoutedEventArgs e)
        {
            WayUpRadioBox.IsChecked = false;
        }

        private void TextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (FindTextBox.Text.Length > 0)
            {
                FindButton.IsEnabled = true;
                return;
            }
            FindButton.IsEnabled = false;
        }
    }
}
