using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Notepad
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void StartFromWindows(object sender, StartupEventArgs e)
        {
            if(e.Args.Length == 1)
            {
                FileInfo file = new FileInfo(e.Args[0]);
                if(file.Exists)
                {
                    Notepad.MainWindow.StartUpFromExplorerFilePath = file.FullName;
                    Notepad.MainWindow.IsStartFromExplorer = true;
                }
            }
        }
    }
}
