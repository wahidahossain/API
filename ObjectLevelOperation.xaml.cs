using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using Amazon.S3.Transfer;
using System.Collections.ObjectModel;

namespace WahidaHossainComp306Lab01
{
    /// <summary>
    /// Interaction logic for ObjectLevelOperation.xaml
    /// </summary>
    public partial class ObjectLevelOperation : Window
    {
        S3connection S3connectionobj = new S3connection();

        string pathOfFile;
        string nameOfBucket;
        string nameOfKey;

        public ObjectLevelOperation()
        {
            InitializeComponent();
            PopulateComboBox();
        }

        private void objlevelback(object sender, RoutedEventArgs e)
        {
            MainWindow win2 = new MainWindow();
            win2.Show();
            this.Close();
        }

        

        //bucket elements upload------------------------------------
        
        public void PopulateComboBox()
        {
            S3connectionobj.GetBucketList();
            comboBoxBucket.ItemsSource = S3connectionobj.listOfBucketNames;
        }
        public void UpdateDataGrid()
        {
            S3connectionobj.GetObjectList(comboBoxBucket.SelectedItem.ToString());
            dataGrid.ItemsSource = S3connectionobj.bucketObjectCollection;
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDataGrid();
        }

        private void btnBrows_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "Any";
            fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "All files (*.*)|*.*|All files (*.*)|*.*";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == true)
            {
                textBoxFilePath.Text = fdlg.FileName;
            }
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxBucket.SelectedItem != null)
            {
                try
                {
                    nameOfKey = "KeyName";
                    pathOfFile = textBoxFilePath.Text;
                    nameOfBucket = comboBoxBucket.SelectedItem.ToString();

                    S3connectionobj.UploadFileAsync(pathOfFile, nameOfBucket, nameOfKey);

                    S3connectionobj.GetObjectList(comboBoxBucket.SelectedItem.ToString());
                    dataGrid.ItemsSource = S3connectionobj.bucketObjectCollection;
                }
                catch (Exception exception)
                {
                    MessageBox.Show("File Upload Not Possible: \n" + exception.Message);
                }
                finally
                {
                    textBoxFilePath.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Please choose a bucket name from the list!");
            }
        }

        private void comboBoxBucket_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //string comboboxSelectedItem = comboBoxBucket.SelectedItem.ToString();
            //MessageBox.Show(comboboxSelectedItem);

            UpdateDataGrid();
        }
    }
}
