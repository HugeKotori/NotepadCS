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
    /// SavePopWindow.xaml 的交互逻辑
    /// </summary>
    /// 
    public enum SaveOrNot
    {
        Save,
        No,
        Cancel,
    }

    public partial class SavePopWindow : Window
    {
        public SavePopWindow(MainWindow main)
        {
            InitializeComponent();
            this.mainWindow = main;
            Chosen = SaveOrNot.Cancel;
            if (main.FilePath.Length > 0)
            {
                this.TextBlock.Text = "你想将更改保存到 " + mainWindow.FilePath + " 吗？";
            }
            else
            {
                this.TextBlock.Text = "你想将更改保存到 " + mainWindow.FileName + " 吗？";
            }
        }

        MainWindow mainWindow;
        public SaveOrNot Chosen { get; set; }

        private void SaveButtonClicked(object sender, RoutedEventArgs e)
        {
            this.Chosen = SaveOrNot.Save;
            this.Close();
        }

        private void NotSaveButtonClicked(object sender, RoutedEventArgs e)
        {
            this.Chosen = SaveOrNot.No;
            this.Close();
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            this.Chosen = SaveOrNot.Cancel;
            this.Close();
        }

    }
}
