using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace WahidaHossainComp306Lab01
{
    
    public partial class CreateBucket : Window
    {
        S3connection S3connectionobj = new S3connection();
        public CreateBucket()
        {
            InitializeComponent();
            S3connectionobj.GetBucketList();
            dataGrid.ItemsSource = S3connectionobj.bucketCollection;
        }

        private void createback(object sender, RoutedEventArgs e)
        {
            MainWindow win2 = new MainWindow();
            win2.Show();
            this.Close();
        }

        

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // S3connection q = new S3connection();
           // q.getList();
        }        

        //creating bucket - btn click event---------------------------------------------------

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string newBucketName = textBoxBucketName.Text;
                S3connectionobj.CreateBucket(newBucketName);
                S3connectionobj.GetBucketList();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Unable to create the bucket because: \n" + exception.Message);
            }
        }
    }
}
