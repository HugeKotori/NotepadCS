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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.Windows.Forms;
using System.IO;

namespace Notepad
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.ZoomScale = 100;
            this.FontSizeNotZoomed = NotepadTextBox.FontSize;
            this.IsEdited = false;
            this.FilePath = "";
            this.FileName = "无标题";
            UpdateTitleBar();
            MBStatusBarVisibility.IsChecked = true;
            if(IsStartFromExplorer == true)
            {
                this.StartUpFromExplorer();
            }
        }
        static public bool IsStartFromExplorer { get; set; }
        static public string StartUpFromExplorerFilePath { get; set; }
        public int ZoomScale { get; set; }
        public double FontSizeNotZoomed { get; set; }
        public bool IsEdited { get; set; }
        public String FilePath { get; set; }
        public String FileName { get; set; }
        private void UpdateStatusBar(object sender, RoutedEventArgs e)      //更新状态栏
        {
            //行列
            int row = NotepadTextBox.GetLineIndexFromCharacterIndex(NotepadTextBox.CaretIndex);
            int col = NotepadTextBox.CaretIndex - NotepadTextBox.GetCharacterIndexFromLineIndex(row);
            CursorPosition.Text = "第 " + (row+1) + " 行，" + "第 " + (col+1) + " 列";
            //缩放值
            ZoomScaleDisplayer.Text = ZoomScale.ToString() + "%";

        }

        public void UpdateTitleBar()                                        //更新标题栏
        {
            if(IsEdited == true)
            {
                NotepadWindow.Title = "*" + FileName + " - " + "记事本";
            }
            else
            {
                NotepadWindow.Title = FileName + " - " + "记事本";
            }
        }

        private void TextBoxChanged(object sender, TextChangedEventArgs e)          //文件文本内容改变
        {
            this.IsEdited = true;
            UpdateTitleBar();
        }

        private void OpenFile(object sender, RoutedEventArgs e)             //菜单-文件-打开
        {
            if (IsEdited == true)
            {
                SavePopWindow savePopWindow = new SavePopWindow(this);
                savePopWindow.ShowDialog();
                if (savePopWindow.Chosen == SaveOrNot.Cancel)
                {
                    return;
                }
                else if (savePopWindow.Chosen == SaveOrNot.Save)
                {
                    SaveFile(this, null);
                }
            }
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "文本文档|*.txt|所有文件|*.*";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FilePath = openFileDialog.FileName;
                FileName = openFileDialog.SafeFileName;
            }
            else
            {
                return;
            }
            FileStream file = new FileStream(FilePath, FileMode.Open, FileAccess.ReadWrite);
            StreamReader fileReader = new StreamReader(file, System.Text.Encoding.Default);
            NotepadTextBox.Text = fileReader.ReadToEnd();
            this.IsEdited = false;
            UpdateTitleBar();
        }

        private void StartUpFromExplorer()
        {
            this.FilePath = StartUpFromExplorerFilePath;
            this.FileName = StartUpFromExplorerFilePath.Substring(FilePath.LastIndexOf("\\") + 1);
            FileStream file = new FileStream(FilePath, FileMode.Open, FileAccess.ReadWrite);
            StreamReader fileReader = new StreamReader(file, System.Text.Encoding.Default);
            NotepadTextBox.Text = fileReader.ReadToEnd();
            this.IsEdited = false;
            UpdateTitleBar();
        }

        private void SaveFile(object sender, RoutedEventArgs e)                //菜单-文件-保存
        {
            if (FilePath.Length == 0)
            {
                SaveAs(this, null);
                return;
            }
            FileStream fileStream = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter(fileStream, System.Text.Encoding.Default);
            streamWriter.Write(NotepadTextBox.Text);
            streamWriter.Close();
            IsEdited = false;
            UpdateTitleBar();
            return;
        }

        private void SaveAs(object sender, RoutedEventArgs e)                   //菜单-文件-另存为
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "文本文档|*.txt|所有文件|*.*";
            saveFileDialog.AddExtension = true;
            saveFileDialog.FileName = FileName + ".txt";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FilePath = saveFileDialog.FileName;
                FileName = saveFileDialog.FileName.Substring(FilePath.LastIndexOf("\\") + 1);
                FileStream fileStream = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
                StreamWriter streamWriter = new StreamWriter(fileStream, System.Text.Encoding.Default);
                streamWriter.Write(NotepadTextBox.Text);
                streamWriter.Close();
                IsEdited = false;
                UpdateTitleBar();
                return;
            }
        }

        private void NewFile(object sender, RoutedEventArgs e)                  //菜单-文件-新建
        {
            if (IsEdited == true)
            {
                SavePopWindow savePopWindow = new SavePopWindow(this);
                savePopWindow.ShowDialog();
                if (savePopWindow.Chosen == SaveOrNot.Cancel)
                {
                    return;
                }
                else if (savePopWindow.Chosen == SaveOrNot.Save)
                {
                    SaveFile(this, null);
                }
            }
            this.NotepadTextBox.Text = "";
            this.FileName = "无标题";
            this.FilePath = "";
            this.IsEdited = false;
            this.UpdateTitleBar();
        }


        private void StartNewWindow(object sender, RoutedEventArgs e)           //菜单-文件-新窗口
        {
            System.Diagnostics.Process.Start(this.GetType().Assembly.Location);
        }

        private void ExitNotepad(object sender, RoutedEventArgs e)              //菜单-文件-退出
        {
            this.Close();
        }

        private void Undo(object sender, RoutedEventArgs e)                     //菜单-编辑-撤销
        {
            NotepadTextBox.Undo();
        }

        private void TextCut(object sender, RoutedEventArgs e)                  //菜单-编辑-剪切
        {
            NotepadTextBox.Cut();
        }

        private void TextCopy(object sender, RoutedEventArgs e)                 //菜单-编辑-复制
        {
            NotepadTextBox.Copy();
        }

        private void TextPaste(object sender, RoutedEventArgs e)                //菜单-编辑-粘贴
        {
            NotepadTextBox.Paste();
        }

        private void TextDelete(object sender, RoutedEventArgs e)               //菜单-编辑-删除
        {
            int delNum = NotepadTextBox.SelectedText.Length;
            if (delNum == 0) return;
            string result = NotepadTextBox.Text;
            int cursorPos = NotepadTextBox.SelectionStart;
            result = result.Remove(cursorPos, delNum);
            NotepadTextBox.Text = result;
            NotepadTextBox.SelectionStart = cursorPos;
            NotepadTextBox.Focus();
        }

        private void SearchWithBing(object sender, RoutedEventArgs e)           //菜单-编辑-使用Bing搜索
        {
            if (NotepadTextBox.SelectionLength == 0)
                return;
            System.Diagnostics.Process.Start("https://cn.bing.com/search?q=" + NotepadTextBox.SelectedText);
        }

        private void JumpTo(object sender, RoutedEventArgs e)                   //菜单-编辑-转到
        {
            JumpToWindow jumpToWindow = new JumpToWindow();
            jumpToWindow.TextBoxLineCount = NotepadTextBox.LineCount;
            jumpToWindow.MainWindow = this;
            jumpToWindow.ShowDialog();
        }               


        private void SelectAll(object sender, RoutedEventArgs e)        //菜单-编辑-全选
        {
            NotepadTextBox.SelectAll();
        }

        private void InsertDateTime(object sender, RoutedEventArgs e)         //菜单-编辑-时间日期
        {
            if(NotepadTextBox.SelectionLength > 0)                              //若选中文本
            {
                this.TextDelete(this, null);
            }
            string result = NotepadTextBox.Text;                                //复制原编辑区副本
            int cursorPos = NotepadTextBox.SelectionStart;                      //获取光标位置
            string nowTime = DateTime.Now.ToString();                           //获取当前时间
            result = result.Insert(cursorPos, nowTime);                         //计算结果
            NotepadTextBox.Text = result;                                       //将结果放入编辑区
            NotepadTextBox.SelectionStart = cursorPos + nowTime.Length;         //重新定位光标
            NotepadTextBox.Focus();
        }

        private void EnableLineFeed(object sender, RoutedEventArgs e)           //菜单-格式-自动换行-勾选
        {
            NotepadTextBox.TextWrapping = TextWrapping.Wrap;
        }

        private void DisableLineFeed(object sender, RoutedEventArgs e)          //菜单-格式-自动换行-取消勾选
        {
            NotepadTextBox.TextWrapping = TextWrapping.NoWrap;
        }

        private void UnhideStatusBar(object sender, RoutedEventArgs e)          //菜单-显示-状态栏-勾选
        {
            BottomStatusBar.Visibility = Visibility.Visible;
        }

        private void HideStatusBar(object sender, RoutedEventArgs e)            //菜单-查看-状态栏-取消勾选
        {
            BottomStatusBar.Visibility = Visibility.Collapsed;
        }

        private void ViewHelp(object sender, RoutedEventArgs e)                 //菜单-帮助-查看帮助
        {
            System.Diagnostics.Process.Start("http://hugekotori.cn");
        }

        private void SendFeedBack(object sender, RoutedEventArgs e)             //菜单-帮助-发送反馈
        {
            System.Diagnostics.Process.Start("http://hugekotori.cn");
        }

        private void OpenAboutWindow(object sender, RoutedEventArgs e)          //菜单-帮助-关于
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        private void OpenFontsWindow(object sender, RoutedEventArgs e)          //菜单-格式-字体
        {
            //FontSelectWindow fontSelectWindow = new FontSelectWindow();
            //fontSelectWindow.NotepadMainWindow = this;
            //fontSelectWindow.GetFontsFromSystem();
            //fontSelectWindow.ShowDialog();
            FontDialog fontDialog = new FontDialog();
            fontDialog.AllowScriptChange = true;
            fontDialog.ShowColor = false;
            fontDialog.ShowHelp = false;
            fontDialog.ShowEffects = false;
            fontDialog.Font = new System.Drawing.Font("微软雅黑",(float)NotepadTextBox.FontSize);
            if (fontDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //获取选择的颜色给控件                
                //var color = fontDialog.Color;
                //NotepadTextBox.Foreground = new SolidColorBrush(Color.FromRgb(color.R, color.G, color.B));
                //获取选择的字体给控件，                
                var font = fontDialog.Font;
                NotepadTextBox.FontFamily = new FontFamily(font.Name);
                this.FontSizeNotZoomed = font.Size;
                NotepadTextBox.FontSize = this.FontSizeNotZoomed * this.ZoomScale;
                NotepadTextBox.FontWeight = FontWeights.DemiBold;
            }
               
        }

        private void ZoomIn(object sender, RoutedEventArgs e)                   //菜单-查看-放大
        {
            if(this.ZoomScale >= 500)
            {
                return;
            }
            this.ZoomScale += 10;
            NotepadTextBox.FontSize = FontSizeNotZoomed * (ZoomScale/100.0);
            UpdateStatusBar(this, null);
        }

        private void ZoomOut(object sender, RoutedEventArgs e)                  //菜单-查看-缩小
        {
            if (this.ZoomScale <= 0)
            {
                return;
            }
            if (FontSizeNotZoomed * (ZoomScale / 100.0) >= 3)
            {
                this.ZoomScale -= 10;
                NotepadTextBox.FontSize = FontSizeNotZoomed * (ZoomScale/100.0);
                UpdateStatusBar(this, null);
            }
        }

        private void MainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)  //主窗口关闭
        {
            if(IsEdited == true)
            {
                SavePopWindow savePopWindow = new SavePopWindow(this);
                savePopWindow.ShowDialog();
                if (savePopWindow.Chosen == SaveOrNot.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
                else if (savePopWindow.Chosen == SaveOrNot.Save)
                {
                    SaveFile(this, null);
                }
            }
        }

        private void OpenDropedInFile(object sender, System.Windows.DragEventArgs e)            //文件拖入窗口
        {
            if(IsEdited == true)
            {
                SavePopWindow savePopWindow = new SavePopWindow(this);
                savePopWindow.ShowDialog();
                if (savePopWindow.Chosen == SaveOrNot.Cancel)
                {
                    return;
                }
                else if (savePopWindow.Chosen == SaveOrNot.Save)
                {
                    SaveFile(this, null);
                }
            }
            FilePath = ((System.Array)e.Data.GetData(System.Windows.DataFormats.FileDrop)).GetValue(0).ToString();
            //System.Windows.MessageBox.Show(FilePath);
            FileName = FilePath.Substring(FilePath.LastIndexOf("\\") + 1);
            FileStream file = new FileStream(FilePath, FileMode.Open, FileAccess.ReadWrite);
            StreamReader fileReader = new StreamReader(file, System.Text.Encoding.Default);
            NotepadTextBox.Text = fileReader.ReadToEnd();
            this.IsEdited = false;
            UpdateTitleBar();
        }

        private void ActivateTextBoxDroppingFunction(object sender, System.Windows.DragEventArgs e)     //激活编辑区的文件拖入功能
        {
            e.Effects = System.Windows.DragDropEffects.Copy;
            e.Handled = true;
        }

        private void Find(object sender, RoutedEventArgs e)                 //菜单-编辑-查找
        {
            FindStringWindow findStringWindow = new FindStringWindow();
            findStringWindow.mainWindow = this;
            findStringWindow.WayUpRadioBox.IsChecked = true;
            findStringWindow.Show();
        }
    }


    //以下部分实现 Ctrl + 鼠标滚轮缩放
    public enum MouseWheelDirection { Up, Down }
    class MouseWheelGesture : MouseGesture                                      //实现鼠标滚轮的类
    {
        public MouseWheelDirection Direction { get; set; }
        public MouseWheelGesture(ModifierKeys keys, MouseWheelDirection direction) : base(MouseAction.WheelClick, keys)
        {
            Direction = direction;
        }
        public override bool Matches(object targetElement, InputEventArgs inputEventArgs)
        {
            var args = inputEventArgs as MouseWheelEventArgs;
            if (args == null)
                return false;
            if (!base.Matches(targetElement, inputEventArgs))
                return false;
            if (Direction == MouseWheelDirection.Up && args.Delta > 0
                || Direction == MouseWheelDirection.Down && args.Delta < 0)
            {
                inputEventArgs.Handled = true;
                return true;
            }

            return false;
        }
    }
    public class MouseWheel : MarkupExtension
    {
        public MouseWheelDirection Direction { get; set; }
        public ModifierKeys Keys { get; set; }

        public MouseWheel()
        {
            Keys = ModifierKeys.None;
            Direction = MouseWheelDirection.Down;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new MouseWheelGesture(Keys, Direction);
        }
    }

}
