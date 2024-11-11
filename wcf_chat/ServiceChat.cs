using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace wcf_chat
{
    //implementation of interface - description of how it will be done
    //here all methods of our interface should be implemented

    //by default when a client addresses our service, server (host) for each of such clients creates their own service object
    //moreover for each connection a new service object is created
    //that's why when several clients connect to services - each clients has their own service
    //but it is a chat => meaning service should be the same because our service needs to know about all clients
    //and send messages to all connected clients
    //
    //to achieve this we have 2 options:

    //#1 (the ugly one) - to create a simple static list of our clients who connect to service
    //as a list field will be static and will be shared among all services

    //#2 WCF has a functionality to make such service shared for all client 
    //to achieve it we need to specify an attribute [ServiceBehavior] with parameter 'InstanceContextMode'
    //This parameter will define how our service will work
    //If we choose 'PerCall', then with each addressing to service, its own service object with each client's addressing
    //and once its work is done, it will be destroyed
    //If we choose 'PerSession', then when a client connected a new session is created for them and when they disconnect the session is destroyed
    //but in this case anyway for each client we will have their own session so this option doesn't fit either
    //We choose 'Single' option = all clients who will be connectingh to our host to our service will be working with our service

    //Before we start implementing our service we need to create another class which would be like a specific container for 3 fields: 
    //---id of the current user,
    //---their name and
    //---operation context where the info about the client's connection to our service will be stored =>
    //so that we could for the user who has been connected in the past to adddress from our service side

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceChat : IServiceChat
    {
        //Create a list of ServerUser objects 
        List<ServerUser> users = new List<ServerUser>();
        //Add a variable for ID generation, assign by defaul value '1'
        int nextId = 1;

        //Method to create a new user
        public int Connect(string name)
        {
            ServerUser user = new ServerUser()
            {
                Id = nextId,
                Name = name,
                //for operationContext we take the data coming with connection of the user from class OperationContext from Current property
                operationContext = OperationContext.Current
            };
            //after we assigned the ID we need to increment nextId value by 1 because for the next user id should have a different value
            nextId++; //this ensures each user will have a different id

            //we need to add a message to all our users that a new user connected to the chat
            //0 => so that we won't send this message to the conneting user themselves
            SendMsg(user.Name+" connected to the chat.", 0);

            //after a user has been created we need to add them to the users list so that our service knows what users we have
            users.Add(user);
            //the method returns a generated ID for the user
            return user.Id;
        }

        //Method to disconnect a user, takes a user ID as entry parameter
        //after connecting the user will know their id - the id is returned by Connect() method
        //so when they disconnect they send their id to our service saying that I am leaving the chat, take me off the list, don't send me new messages
        public void Disconnect(int id)
        {
            //In order to remove the user from the list, we first need to find them
            //we could use loops with search and checks however C# offers LINQ functionality, it's more convenient way 
            var user = users.FirstOrDefault(i => i.Id == id);
            //if there is no such user => the user will be null => so we need to check this variable for null
            if (user != null)
            {
                users.Remove(user);
                //after we found and deleted this user, we need to send out a message to all the rest users that this user left the chat
                SendMsg(user.Name + " left the chat.", 0);
            }
        }

        public void SendMsg(string msg, int id)
        {
            //here we need to go through all users 
            foreach (var item in users)
            {
                //create a message which will be a reply from server to all users
                //in message we need a date when the message was sent by server
                //DateTime class has property Now - we will use that
                //we do not need all the details so we call method ToShortTimeString()
                string answer = DateTime.Now.ToShortTimeString();

                //then we need to add the name of the user who sent this message
                //to find the name of this user we use an id from entry parameter
                var user = users.FirstOrDefault(i => i.Id == id);
                if (user != null)
                {
                    //if we found such a user, then to variable answer we need to add the name of this user
                    answer += ": " + user.Name + " ";
                }

                //then we need to add a text message itself which we received as an entry parameter in the method
                answer += msg;

                //if we did not find a user in our users list, it means that it was either connect or disconnect
                //becuase if these methods as an entry id parameter we sent 0, and we cannot have users with 0 id (generation of id starts from 1)
                //in this case name field will be sent in advance with the messages and we won't need to add it here

                //after a message content was formed, we need to send out this message to the user with which we work in foreach loop
                //to send this message we have a callback method
                //we need to address to operation context field of our user that we are currently going through
                //and call the method GetCallBackChannel() and pass the callback interface in <> and
                //call the method from this interface which will be implemented on the client's side and
                //the client will receive this message that we formed

                item.operationContext.GetCallbackChannel<IServerChatCallBack>().MsgCallback(answer);
            }
        }
    }
}
