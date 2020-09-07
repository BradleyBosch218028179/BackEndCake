using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Project_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IGame_Service" in both code and config file together.
    [ServiceContract]
    public interface IGame_Service
    {
        //Handle the users login
        [OperationContract]
        int login(String email, String password);

        //Define the Register function which adds the user to the database
        [OperationContract]
         int Register(string firstname, string surname, string email, string password, string usertype, string username, string cellphone, string address);

        //Allow the user to edit their data
        [OperationContract]
        int UpdateUser(User user);

        //Get a list of all the game
        [OperationContract]
        List<Game> getAllGames();

        //Get a list of all the game on special
        //[OperationContract]
        //List<Game> GameOnSpecial();

        //Add a new game to the database - This will be called when login is 1
        [OperationContract]
        int AddGame(string title, string type, string descrption, short special, int rating, int quantity, int num_Comments, decimal price, decimal discountedPrice, string game_image, string game_trailer, int user_ID, int status);

        //Edit the game details
        [OperationContract]
        int EditGame(Game game);

        //Delete the game in the database
        [OperationContract]
        int RemoveGame(string Game_Title);

        //Helper method to get the price of the game
        //[OperationContract]
        //decimal isOnSpecial(int ID);

        //Get the users ID
        [OperationContract]
        User getUser(int id);


        //Get the games recently added according to todays date
        [OperationContract]
        List<Game> getRecentGame();

        //Status 1 means it is active
        [OperationContract]
        Game GetOneGame(int ID);
    }
}
