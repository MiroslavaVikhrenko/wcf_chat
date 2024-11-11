using System.ServiceModel;

namespace wcf_chat
{
    public class ServerUser
    {
        //Add property:
        public int Id {  get; set; }
        public string Name { get; set; }
        //for operation context we need to connect System.ServiceModel
        public OperationContext operationContext { get; set; }

    }
}
