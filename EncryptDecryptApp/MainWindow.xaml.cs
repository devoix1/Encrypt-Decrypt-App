using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace EncryptDecryptApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isPause;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }




        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            var fdlg = new System.Windows.Forms.OpenFileDialog();
            fdlg.Title = "Browse";
            fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "All files (*.*)|*.*";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                directoryTxtBox.Text = fdlg.FileName;
            }
            else
            {
                return;
            }
            var extension = Path.GetExtension(directoryTxtBox.Text);
            if (extension == ".slave")
            {
                decryptButton.IsEnabled = true;
            }
            else
            {
                encryptButton.IsEnabled = true;
            }

            

        }

        private async void encryptButton_Click(object sender, RoutedEventArgs e)
        {
            pauseButton.IsEnabled = true;
            progressBar.Visibility = Visibility.Visible;
            decryptButton.IsEnabled = false;
            encryptButton.IsEnabled = false;
            percentageTextBlock.Visibility = Visibility.Visible;
            await Encryption(directoryTxtBox.Text);

        }

        private async void decryptButton_Click(object sender, RoutedEventArgs e)
        {
            pauseButton.IsEnabled = true;
            decryptButton.IsEnabled = false;
            encryptButton.IsEnabled = false;
            progressBar.Visibility = Visibility.Visible;
            percentageTextBlock.Visibility = Visibility.Visible;
            await Decryption(directoryTxtBox.Text);

        }

        private void pauseButton_Click(object sender, RoutedEventArgs e)
        {
            isPause = true;
            resumeButton.IsEnabled = true;
            pauseButton.Visibility = Visibility.Hidden;
            resumeButton.Visibility = Visibility.Visible;
        }

        private void resumeButton_Click(object sender, RoutedEventArgs e)
        {
            isPause = false;
            resumeButton.Visibility = Visibility.Hidden;
            pauseButton.IsEnabled = true;
            pauseButton.Visibility = Visibility.Visible;
        }

        private async Task Encryption(string filePath)
        {

            await Task.Run(() => {
                FileStream readFile = null;
                FileStream writeFile = null;

                readFile = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
                string newFile = filePath + ".slave";
                writeFile = new FileStream(newFile, FileMode.Create, FileAccess.ReadWrite);
                for (int i = 0; i < readFile.Length; i++)
                {
                    while (isPause)
                        Task.Delay(100);
                    byte b = (byte)readFile.ReadByte();
                    b = (byte)~b;
                    writeFile.WriteByte(b);
                    Dispatcher.Invoke(() =>
                    {
                        progressBar.Value = ((int)(readFile.Position * 100 / readFile.Length));
                    });
                    

                }

                readFile.Close();
                writeFile.Close();
                decryptButton.IsEnabled = true;
                encryptButton.IsEnabled = true;
            });
            

        }



        private async Task Decryption(string filePath)
        {

            await Task.Run(() => {
                FileStream readFile = null;
                FileStream writeFile = null;

                readFile = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
                string newFile = filePath.Substring(0, filePath.Length - 5);

                writeFile = new FileStream(newFile, FileMode.Create, FileAccess.ReadWrite); 
                for (int i = 0; i < readFile.Length; i++)
                {
                    while (isPause)
                        Task.Delay(100);
                    byte b = (byte)readFile.ReadByte();
                    b = (byte)~b;
                    writeFile.WriteByte(b);
                    Dispatcher.Invoke(() =>
                    {
                        progressBar.Value = ((int)(readFile.Position * 100 / readFile.Length));
                    });
                }
                readFile.Close();
                writeFile.Close();
                decryptButton.IsEnabled = true;
                encryptButton.IsEnabled = true;
            });
            
        }

    }
}
