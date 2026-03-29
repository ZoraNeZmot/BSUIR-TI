using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cipher1
{
    public partial class CipherFont : Form
    {
        protected static bool IsRussianChar(char c) { return (c >= 'А' && c <= 'я') || c == 'ё' || c == 'Ё'; }
        public static string RemoveNonAlphabetic(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            StringBuilder result = new StringBuilder();

            foreach (char c in input)
            {
                if (IsRussianChar(c))
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        public CipherFont()
        {
            InitializeComponent();
            comboAlgorithm.SelectedIndex = 0;
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select a file";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                openFileDialog.Filter = "All files (*.*)|*.*|Text files (*.txt)|*.txt|Image files (*.jpg;*.png)|*.jpg;*.png";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;
                    textFile.Text = selectedFilePath;
         
                    textIn.Text = File.ReadAllText(textFile.Text);

                }
            }

        }


        private void btnCipher_Click(object sender, EventArgs e)
        {
            Cipherer.CipherType type = comboAlgorithm.SelectedIndex == 0 ? Cipherer.CipherType.Column : Cipherer.CipherType.Visner;
            string key = RemoveNonAlphabetic(textKey.Text);
            if (string.IsNullOrEmpty(key)) 
            {
                MessageBox.Show(
                    "Пожалуйста, используйте только русские буквы в КЛЮЧЕ.",          
                    "Недопустимые символы",                                   
                    MessageBoxButtons.OK,                                     
                    MessageBoxIcon.Warning                                     
                );
                return; 
            }
            string content = textIn.Text;
           

            string res = Cipherer.PerformCiphering(content, key, type);
            textOut.Text = res;


            if (cbSave.Checked)
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                    saveFileDialog.FilterIndex = 1;
                    saveFileDialog.RestoreDirectory = true;
                    saveFileDialog.Title = "Сохранить результат дешифрования";
                    saveFileDialog.DefaultExt = "txt";
                    saveFileDialog.AddExtension = true;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            // Get the file path
                            string filePath = saveFileDialog.FileName;

                            // Write the result to the file
                            System.IO.File.WriteAllText(filePath, res, System.Text.Encoding.UTF8);

                            MessageBox.Show(
                                $"Результат успешно сохранен в файл:\n{filePath}",
                                "Сохранение завершено",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(
                                $"Ошибка при сохранении файла:\n{ex.Message}",
                                "Ошибка сохранения",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        }
                    }
                }
            }

        }




        private void btnDecipher_Click(object sender, EventArgs e)
        {
            Cipherer.CipherType type = comboAlgorithm.SelectedIndex == 0 ? Cipherer.CipherType.Column : Cipherer.CipherType.Visner;
            string key = RemoveNonAlphabetic(textKey.Text);
            if (string.IsNullOrEmpty(key))
            {
                MessageBox.Show(
                    "Пожалуйста, используйте только русские буквы в КЛЮЧЕ.",
                    "Недопустимые символы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }
            string content = textIn.Text;
            

            string res = Cipherer.PerformDeciphering(content, key, type);
            textOut.Text = res;

            if (cbSave.Checked)
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                    saveFileDialog.FilterIndex = 1;
                    saveFileDialog.RestoreDirectory = true;
                    saveFileDialog.Title = "Сохранить результат дешифрования";
                    saveFileDialog.DefaultExt = "txt";
                    saveFileDialog.AddExtension = true;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            // Get the file path
                            string filePath = saveFileDialog.FileName;

                            // Write the result to the file
                            System.IO.File.WriteAllText(filePath, res, System.Text.Encoding.UTF8);

                            MessageBox.Show(
                                $"Результат успешно сохранен в файл:\n{filePath}",
                                "Сохранение завершено",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(
                                $"Ошибка при сохранении файла:\n{ex.Message}",
                                "Ошибка сохранения",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        }
                    }
                }
            }
        
        }


    }
}
