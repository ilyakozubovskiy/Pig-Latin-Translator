using Microsoft.Win32;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace MyWordPad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetF1CommandBinding();
        }
        private void SetF1CommandBinding()
        {
            CommandBinding helpBinding = new CommandBinding(ApplicationCommands.Help);
            helpBinding.CanExecute += CanHelpExecute;
            helpBinding.Executed += HelpExecuted;
            CommandBindings.Add(helpBinding);
        }
        private void CanHelpExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // Here, you can set CanExecute to false if you want to prevent the
            // command from executing.
            e.CanExecute = true;
        }

        private void HelpExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Look, it is not that difficult. Just type something!",
                "Help!");
        }
        private void OpenCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SaveCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void OpenCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // Create an open file dialog box and only show XAML files.
            var openDlg = new OpenFileDialog { Filter = "Text Files |*.txt" };
     
            // Did they click on the OK button?
            if (true == openDlg.ShowDialog())
            {
                // Load all text of selected file.
                string dataFromFile = File.ReadAllText(openDlg.FileName);

                // Show string in TextBox.
                txtSource.Text = dataFromFile;
            }
        }

        private void SaveCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var saveDlg = new SaveFileDialog { Filter = "Text Files |*.txt" };

            // Did they click on the OK button?
            if (true == saveDlg.ShowDialog())
            {
                // Save data in the TextBox to the named file.
                File.WriteAllText(saveDlg.FileName,"SOURCE TEXT: " + txtSource.Text + "\n" + "RESULT TEXT: " + txtResult.Text);
            }
        }

        protected void FileExit_Click(object sender, RoutedEventArgs args)
        {
            // Close this window.
            this.Close();
        }

        protected void MouseEnterExitArea(object sender, RoutedEventArgs args)
        {
            statBarText.Text = "Exit Application";
        }
        protected void MouseEnterHelpArea(object sender, RoutedEventArgs args)
        {
            statBarText.Text = "Help area";
        }

        protected void MouseLeaveArea(object sender, RoutedEventArgs args)
        {
            statBarText.Text = string.Empty;
        }
        private void MouseClickDeveloperArea(object sender, RoutedEventArgs e)
        {
            statBarText.Text = "Developer area";

            var destinationurl = "https://www.linkedin.com/in/ilyakozubovskiy/";
            var sInfo = new System.Diagnostics.ProcessStartInfo(destinationurl)
            {
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(sInfo);
        }
        private void MouseClickTranslateArea(object sender, RoutedEventArgs e)
        {
            statBarText.Text = "Process has been ended";
            txtResult.Text = TranslateToPigLatin(txtSource.Text);
        }

        private void MouseEnterTranslateArea(object sender, MouseEventArgs e)
        {
            statBarText.Text = "Start Process";
            
        }
        private void MouseEnterFileArea(object sender, MouseEventArgs e)
        {
            statBarText.Text = "File area";
        }
        private void MouseEnterEditArea(object sender, MouseEventArgs e)
        {
            statBarText.Text = "Edit area";

        }
        private void MouseEnterDeveloperArea(object sender, MouseEventArgs e)
        {
            statBarText.Text = "Developer area";

        }
        private void EnterClicked(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                statBarText.Text = "Process has been ended";
                txtResult.Text = TranslateToPigLatin(txtSource.Text);
                e.Handled = true;
            }
        }
        public static string TranslateToPigLatin(string phrase)
        {

            StringBuilder result = new StringBuilder(phrase);

            for (int i = 0; i < result.Length; i++)
            {
                if (!char.IsLetter(result[i]))
                {
                    continue;
                }

                int beginIndex = i;
                int endIndex = FindIndex(result.ToString(), beginIndex, new  char[] { ' ', '-', '?', '.', ',', '!' });

                string word = result.ToString(beginIndex, endIndex - beginIndex).ToLower(CultureInfo.CurrentCulture);
                int vowelIndex = FindIndex(word, 0, new char[] { 'a', 'e', 'i', 'o', 'u' });

                char firstLetter = result[beginIndex];
                result.Remove(beginIndex, word.Length);

                if (vowelIndex == 0)
                {
                    result.Insert(beginIndex, word + "yay");
                    beginIndex += word.Length + 3;
                }
                else if (vowelIndex > 0)
                {
                    result.Insert(beginIndex, word[vowelIndex..word.Length] + word[..vowelIndex] + "ay");
                    beginIndex += word.Length + 2;
                }

                if (char.IsUpper(firstLetter))
                {
                    result[i] = char.ToUpper(result[i], CultureInfo.InvariantCulture);
                }

                i = beginIndex;
            }

            return result.ToString();
        }

        private static int FindIndex(string source, int index, char[] sourceArray)
        {
            for (int i = index; i < source.Length; i++)
            {
                for (int j = 0; j < sourceArray.Length; j++)
                {
                    if (source[i] == sourceArray[j])
                    {
                        return i;
                    }
                }
            }

            return source.Length;
        }

    }
}