using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace wcf_chat
{
    //server part logic is here

    //description of what our service can do
    //here there will be finctions which a client can use while working with our server
    //so here we describe core functionalities for our service

    //we need to inform service that there is callback interface which it can call on the client's side
    //for this reason on attribute [ServiceContract] we need to specify a parameter 'CallbackContract' and pass IServerChatCallBack for typeof

    [ServiceContract(CallbackContract = typeof(IServerChatCallBack))]
    public interface IServiceChat
    {
        //note on WCF: when we call SendMsg() method we call it from our service, client sends to us the message
        //then they wait for reply from serive that the message is received 
        //even if the method does not return any value our client will anyway be blocked until server will reply and the client would receive the reply
        //However, in some cases we do not need to wait for such reply - we just need to send a message to server and that's it
        //and server needs to send out this message to the rest of the clients

        //Method to connect to our service - takes a connecting user name as parameter
        //Here we need to wait for server's reply becaue result of this method will be receiving the id assigned to our user by our server
        [OperationContract] //methods with this attribute will be 'visible' from the client's side     
        int Connect(string name);

        //Method to disconnect from our service => will be called when the client leaves the chat OR press the button to disconnect from the chat
        //needed to inform the service that this client no longer exists - no need to send messages to this client
        //takes as a parameter id of the disconnecting client
        [OperationContract]      
        void Disconnect(int id);

        //Method to send message - it takes string parameter (message)
        //if we do not need to wait for server reply, then we need to add a parameter 'IsOneWAy' to attribuite [OperationContract]
        //this parameter should take 'true' value 
        //by default all these attributes have this parameter but it's value is false 
        [OperationContract(IsOneWay = true)]
        void SendMsg(string msg);

        //Method SendMsg() receives a message and server needs somehow send out the message to all clients 
        //In order to envoke any action on the client's side from our server's end, we need a callback 
        //meaning some function that is called back
        //On server part we describe interfice, but realization of this callback will be in the client's part
        //allowing the client to receive a message from server
        //that's why below we create another interface 
    }

    public interface IServerChatCallBack
    {
        //Callback method to send out messages 
        [OperationContract]
        void MsgCallback(string msg);

    }
}
