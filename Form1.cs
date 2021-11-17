using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZIKS5
{
    public partial class Lab5Form : Form
    {
        static string inputFilePath = "";
        static string outputFilePath = "";
        static string file;
        static Chilkat.Crypt2 AES = new Chilkat.Crypt2();
        public Lab5Form()
        {
            InitializeComponent();
            AES.CryptAlgorithm = "aes";
            AES.CipherMode = "ctr";
            AES.KeyLength = 128;
            AES.EncodingMode = "base64";
            AES.HashAlgorithm = "md5";
        }
        private void openFileButton_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "txt files (*.txt)|*.txt";
            openFileDialog.FileName = "";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                inputFilePath = openFileDialog.FileName;
                openFileBox.Text = inputFilePath;
                outputFilePath = inputFilePath.Substring(0, inputFilePath.Length - 4);
                file = File.ReadAllText(inputFilePath);
            }
        }
        private void crypto(int flag)
        {
            try
            {
                if (passwordBox.Text.Length >= 1 && passwordBox.Text.Length <= 16)
                {
                    bool asciiFlag = true;
                    foreach (char c in passwordBox.Text)
                        if (c < 32 || c >= 127)
                            asciiFlag = false;
                    if (asciiFlag)
                    {
                        bool isHex = true;
                        foreach (char c in vectorBox.Text)
                        {
                            if (!((c >= '0' && c <= '9') || (c >= 'A' && c <= 'F')))
                            {
                                isHex = false;
                            }
                        }
                        if (isHex)
                        {
                            AES.SetEncodedIV(vectorBox.Text, "ascii");
                            AES.SetEncodedKey(passwordBox.Text.PadLeft(16, '0'), "ascii");
                            if (flag == 1)
                                File.WriteAllText(outputFilePath + "_encrypted.txt", AES.EncryptStringENC(file));
                            else if (flag == 2)
                                File.WriteAllText(outputFilePath + "_decrypted.txt", AES.DecryptStringENC(file));
                        }
                    }
                    else
                        MessageBox.Show("Error! Password must be ASCII encoded!");
                }
            }
            catch { }
        }
        private void encryptButton_Click(object sender, EventArgs e)
        {
            crypto(1);
        }
        private void decryptButton_Click(object sender, EventArgs e)
        {
            crypto(2);
        }
    }
}
