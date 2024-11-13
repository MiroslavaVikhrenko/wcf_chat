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
using ChatClient.ServiceChat;

namespace ChatClient
{
    //In order IServiceChatCallback can pass messages - in the clien we need to implement this interface
    public partial class MainWindow : Window, IServiceChatCallback
    {
        //implemetation for the client side (graphical interface)
        //bool varuable for understanding if the user is currently connected to the service/server or not; default value is false
        bool IsConnected = false;

        //create an object of host type in client so that we can interact with its methods
        //as we already connected it in references (connected services) => we can already use it here
        ServiceChatClient client;
        int ID;

        //Initialization will take place when calling event Window_Loaded() for our main window
        //When main window is loaded = when the client is loaded => we will create and provide memory for service chat client object
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ////when window is being created, a service chat client object is created and we will be able to interact with this object
            //client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this)); //removed from here and moved to ConnectUser() method
        }
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
                client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this));
                //pass name of the user from text box UserName - Text property
                //this method returns ID
                ID = client.Connect(tbUserName.Text);
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
                client.Disconnect(ID);
                //assign client to null after disconnecting
                client = null;
                tbUserName.IsEnabled = true; //when the user is disconnected, we can change user name
                bConnDicon.Content = "Connect";
                IsConnected = false;
            }
        }
        //Event for tbMessage text box
        private void tbMessage_KeyDown(object sender, KeyEventArgs e)
        {
            //check what key is pressed
            if (e.Key == Key.Enter)
            {
                //check for null => if client is not null it means that we already connected 
                if (client != null)
                {
                    //if 'enter' key was pressed => it means that user typed their message and we need to send it
                    //call SendMsg() method and passing 2 arguments: text from text box message and the id
                    client.SendMsg(tbMessage.Text, ID);
                    //clear tbMessage after message is sent to server
                    tbMessage.Text = string.Empty;
                }
            }

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

        //Implement IServiceChatCallback interface
        public void MsgCallback(string msg)
        {
            //add new item (=message) to the listbox where we store messages
            //each time server will send us a message => this message will appear in the list box
            lbChat.Items.Add(msg);
        }

        //Event for closing window (press x)
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisconnectUser();
        }
    }
}
