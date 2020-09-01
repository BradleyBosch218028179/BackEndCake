using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Service1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        int Register(string username, string password, string email, string address, string cell);

        [OperationContract]
        int Login(string username, string password);

        [OperationContract]
        int UpdateUserInfo(int User_ID, String uUsername, String uPassword, String uEmail, String nAddress, String cell);

        [OperationContract]
        int addGame(string title, string type, string description, int rating, decimal Price, int quantity, string trailer, 
            string image, int Special, int Status, int comments, int User_ID);

        [OperationContract]
        int editGame(int Game_ID, string title, string type, string description, int rating, decimal Price, int quantity, 
            string trailer,string image, int Special, int Status, int comments, int User_ID);

        [OperationContract]
        int removeGame(int Game_ID);
    }
}
