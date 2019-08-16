using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// JumpToWindow.xaml 的交互逻辑
    /// </summary>
    public partial class JumpToWindow : Window
    {

        private MainWindow mainWindow;
        public int TextBoxLineCount { get; set; }
        public MainWindow MainWindow { get => mainWindow; set => mainWindow = value; }

        public JumpToWindow()
        {
            InitializeComponent();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)        //限制输入为数字
        {
            Regex re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void JumpToLine(object sender, RoutedEventArgs e)
        {
            int jumpInput;
            if(!int.TryParse(JumpToLineInput.Text,out jumpInput))
            {
                MessageBox.Show("输入的行数过大！");
                return;
            }
            if(jumpInput > TextBoxLineCount)
            {
                MessageBox.Show("输入的行数超过总行数");
                return;
            }
            MainWindow.NotepadTextBox.SelectionStart = MainWindow.NotepadTextBox.GetCharacterIndexFromLineIndex(jumpInput-1);
            MainWindow.NotepadTextBox.SelectionLength = 0;
            MainWindow.NotepadTextBox.Focus();
            this.Close();
        }
    }
}
