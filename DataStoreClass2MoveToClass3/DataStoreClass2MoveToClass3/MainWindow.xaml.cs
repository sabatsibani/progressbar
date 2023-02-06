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

namespace DataStoreClass2MoveToClass3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> sourceFolderPaths;
        private IDataStoreFileMover fileMover = new DataStoreFileMover(new Logger(), "Class2", "Class3");

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnListFiles_Click(object sender, RoutedEventArgs e)
        {
            string Datapath = txtPath.Text;

            if (sourceFolderPaths == null) { sourceFolderPaths = new List<string>(); }
            
            if (sourceFolderPaths.Count == 0) { sourceFolderPaths = fileMover.GetSourceFolderList(Datapath); }

            StringBuilder builder = new StringBuilder();

            foreach(string path in sourceFolderPaths)
            {
                builder.AppendLine(path);
            }

            txtData.Text = builder.ToString();
        }

        private void btnMoveFiles_Click(object sender, RoutedEventArgs e)
        {
            string Datapath = txtPath.Text;

            if (sourceFolderPaths == null) { sourceFolderPaths = new List<string>(); }
            
            if (sourceFolderPaths.Count == 0) { sourceFolderPaths = fileMover.GetSourceFolderList(Datapath); }

            foreach (string path in sourceFolderPaths)
            {
                Task.Run(() =>
                {
                    path.MoveFiles(path.Replace("Class2", "Class3"), x => progressBar.Dispatcher.BeginInvoke(new Action(() => { progressBar.Value = x; lblPercent.Content = x.ToString() + "%"; })));
                }).GetAwaiter().OnCompleted(() => progressBar.Dispatcher.BeginInvoke(new Action(() => { progressBar.Value = 100; lblPercent.Content = "100%"; })));


                // fileMover.MoveFiles(path, path.Replace("Class2", "Class3"));
            }
            MessageBox.Show("You have successfully moved the file !", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnPath_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();
            if (result.ToString() != string.Empty)
            {
                txtPath.Text = openFileDlg.SelectedPath;
            }
            
        }
    }
}
