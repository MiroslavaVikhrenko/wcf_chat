using System;
using System.Collections.Generic;
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

        public int Connect()
        {
            throw new NotImplementedException();
        }

        public void Disconnect(int id)
        {
            throw new NotImplementedException();
        }

        public void SendMsg(string msg)
        {
            throw new NotImplementedException();
        }
    }
}
