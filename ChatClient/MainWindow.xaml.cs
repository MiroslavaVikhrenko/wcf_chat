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

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //implemetation for the client side (graphical interface)
        //bool varuable for understanding if the user is currently connected to the service/server or not; default value is false
        bool IsConnected = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        //Method to connect the user
        void ConnectUser()
        {
            //check variable IsConnected - if the user is not yet connected - then change value for this variable to true
            if (!IsConnected)
            {
                //when the user is connected we need to block functionality to change user name => we set property IsEnabled to false
                tbUserName.IsEnabled = false;
                //change the button text from Connect to Disconnect so that we can re-use the button for both connect and disconnect
                bConnDicon.Content = "Disconnect";
                //connect the user
                IsConnected = true;
            }
        }

        //Method to disconnect the user (similar logic as above)
        void DisconnectUser()
        {
            if (IsConnected)
            {
                tbUserName.IsEnabled = true; //when the user is disconnected, we can change user name
                bConnDicon.Content = "Connect";
                IsConnected = false;
            }
        }

        private void tbMessage_KeyDown(object sender, KeyEventArgs e)
        {

        }

        //If the button is clicked...
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (IsConnected)
            {
                DisconnectUser();
            }
            else
            {
                ConnectUser();
            }
        }
    }
}
