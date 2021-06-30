using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cifrado_App
{
    public partial class Form1 : Form
    {
        byte[] key;
        byte[] IV;
        string encryptData;
        byte[] encrypted;
        byte[] encryptedFile;
        byte[] keyFile;
        byte[] IVFile;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            string data = txtInputData.Text;
            EncryptAesManaged(data);
            txtInputData.Text = "";
        }
          void EncryptAesManaged(string raw)
        {
            try
            {
                // Create Aes that generates a new key and initialization vector (IV).    
                // Same key must be used in encryption and decryption    
                using (AesManaged aes = new AesManaged())
                {
                    key = aes.Key;
                    IV = aes.IV;
                    // Encrypt string    
                    encrypted = Encrypt(raw, key, IV);
                    // Print encrypted string    
                    //Console.WriteLine($ "Encrypted data: {System.Text.Encoding.UTF8.GetString(encrypted)}");
                    encryptData = Encoding.UTF8.GetString(encrypted);
                    Console.WriteLine(encryptData);
                    txtEncryptData.Text = encryptData;
                    // Decrypt the bytes to a string.    
                    //string decrypted = Decrypt(encrypted, aes.Key, aes.IV);
                    // Print decrypted string. It should be same as raw data    
                    //Console.WriteLine($ "Decrypted data: {decrypted}");
                }
            }
            catch (Exception exp)
            {
                //Console.WriteLine(exp.Message);
            }
            //Console.ReadKey();
        }
        static byte[] Encrypt(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            // Create a new AesManaged.    
            using (AesManaged aes = new AesManaged())
            {
                // Create encryptor    
                ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV);
                // Create MemoryStream    
                using (MemoryStream ms = new MemoryStream())
                {
                    // Create crypto stream using the CryptoStream class. This class is the key to encryption    
                    // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream    
                    // to encrypt    
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        // Create StreamWriter and write data to a stream    
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(plainText);
                        encrypted = ms.ToArray();
                    }
                }
            }
            // Return encrypted data    
            return encrypted;
        }
        static string Decrypt(byte[] cipherText, byte[] Key, byte[] IV)
        {
            string plaintext = null;
            // Create AesManaged    
            using (AesManaged aes = new AesManaged())
            {
                // Create a decryptor    
                ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);
                // Create the streams used for decryption.    
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    // Create crypto stream    
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        // Read crypto stream    
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }

        private void btnDesEncrypt_Click(object sender, EventArgs e)
        {
            string dataEncrypt = txtEncryptData.Text;
            using (AesManaged aes = new AesManaged())
            {
                txtInputData.Text= Decrypt(encrypted, key, IV);

            }
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\Users\Jessenia-PC",
                Title = "Browse Text Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "txt files (*.txt)|*.txt",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = openFileDialog1.FileName;
            }
        }

        private void btnEncryptFile_Click(object sender, EventArgs e)
        {
            string FilePath = txtFilePath.Text;
            EncryptAesManagedFile(FilePath);
        }

        void EncryptAesManagedFile(string path)
        {
            try
            {
                // Create Aes that generates a new key and initialization vector (IV).    
                // Same key must be used in encryption and decryption    
                using (AesManaged aes = new AesManaged())
                {
                    keyFile = aes.Key;
                    IVFile = aes.IV;
                    byte[] plainContent = File.ReadAllBytes(path);
                    // Encrypt string    
                    encryptedFile = EncryptFile(plainContent, keyFile, IVFile);
                   
                }
            }
            catch (Exception exp)
            {
                //Console.WriteLine(exp.Message);
            }
            //Console.ReadKey();
        }

        static byte[] EncryptFile(byte[] plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            // Create a new AesManaged.    
            using (AesManaged aes = new AesManaged())
            {
                // Create encryptor    
                ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV);
                // Create MemoryStream    
                using (MemoryStream ms = new MemoryStream())
                {
                    // Create crypto stream using the CryptoStream class. This class is the key to encryption    
                    // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream    
                    // to encrypt    
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        // Create StreamWriter and write data to a stream    
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(plainText);
                        encrypted = ms.ToArray();
                        //Escribir lo que esta encriptado
                        string filePath = @"C:\Users\Jessenia-PC\Cifrado\textoCifrado.txt";
                        File.WriteAllBytes(filePath, encrypted);
                    }
                }
            }
            // Return encrypted data    
            return encrypted;
        }

        private void btnDesEncryptFile_Click(object sender, EventArgs e)
        {
            
            string FilePath = txtFilePath.Text;
            byte[] plainContent = File.ReadAllBytes(FilePath);
            string texto = Encoding.UTF8.GetString(plainContent);
            
            
            using (AesManaged aes = new AesManaged())
            {
                string texto1 = DecryptFile(encryptedFile, keyFile, IVFile);
                file(texto1, FilePath);
               // DecryptFile(FilePath, keyFile, IVFile);

            }
        }
        public void file(string texto, string path) {
            // Write file using StreamWriter  
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(texto);
            }
        }
        public static async Task ExampleAsync( string texto)
        {
            string filePath = @"C:\Users\Jessenia-PC\Cifrado\textoDescifrado.txt";
            string text =
                "A class is the most powerful data type in C#. Like a structure, " +
                "a class defines the data and behavior of the data type. ";

            await File.WriteAllTextAsync(filePath, texto);
        }
        static string DecryptF(byte[] cipherText, byte[] Key, byte[] IV)
        {
            string plaintext = null;
            // Create AesManaged    
            using (AesManaged aes = new AesManaged())
            {
                // Create a decryptor    
                ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);
                // Create the streams used for decryption.    
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    // Create crypto stream    
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        // Read crypto stream    
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }

        static string DecryptFile(byte[] cipherText, byte[] Key, byte[] IV)
        {
            string plaintext = null;
            // Create AesManaged    
            using (AesManaged aes = new AesManaged())
            {
                // Create a decryptor    
                ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);
                // Create the streams used for decryption.    
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    // Create crypto stream    
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        // Read crypto stream    
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }


    }
}
