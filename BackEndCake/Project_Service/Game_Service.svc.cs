using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Project_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Game_Service" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Game_Service.svc or Game_Service.svc.cs at the Solution Explorer and start debugging.
    public class Game_Service : IGame_Service
    {
        //Connection to the dbml, which connects to the database
        DataClasses1DataContext db = new DataClasses1DataContext();


        //Handle the login according to type of user
        //Returns users ID if successful
        //Error is 0
        //Admin is 1

        public int login(String email, String password)
        {
            //check if the user's information is in the database
            var user = (from u in db.Users
                        where u.User_Email.Equals(email) && u.User_Password.Equals(Secrecy.HashPassword(password))
                        select u).FirstOrDefault();


            //If the user exists and is a customer
            if (user != null && user.USer_Type.Equals("Customer"))
            {
                //Return the users ID
                return user.User_ID;
            }
            else if (user == null)
            {
                //Display an error
                return 0;
            }
            else if (user.USer_Type.Equals("Admin"))
            {
                //Assign the ID to an admin ID who can add, edit and delete products
                return 1;
            }
            else
            {
                //Display an error
                return -1;
            }
        }

        //Add a new user to the database and hash their password
        public int Register(string firstname, string surname, string email, string password, string usertype, string username, string cellphone, string address)
        {
            //Check if the user exists
            var checkuser = (from u in db.Users
                             where u.User_Email.Equals(email)
                             select u).FirstOrDefault();

            if (checkuser == null)
            {
                var newUser = new User
                {
                    //Assign the new users data to the database
                    User_FirstName = firstname,
                    User_Surname = surname,
                    User_Email = email,
                    //Hash their password
                    User_Password = Secrecy.HashPassword(password),
                    User_Name = username,
                    User_Address = address,
                    User_Cellphone = cellphone,
                    USer_Type = usertype
                };
                //Submit to the database
                db.Users.InsertOnSubmit(newUser);

                try
                {
                    //Submit the changes
                    db.SubmitChanges();
                    return 1;
                }
                catch (Exception ex)
                {
                    //Show an error if something goes wrong
                    ex.GetBaseException();
                    return -1;
                }
            }
            else
            {
                //Return 0 if it is successful
                return 0;
            }
        }

        //Allow the user to edit their data
        public int UpdateUser(User user)
        {
            //Check if the user exists 
            var checkuserID = (from u in db.Users
                               where u.User_ID.Equals(user.User_ID)
                               select u).FirstOrDefault();

            //If the user exists
            if (checkuserID != null)
            {
                //Assign this users old details to the new details

                checkuserID.User_FirstName = user.User_FirstName;
                checkuserID.User_Surname = user.User_Surname;
                checkuserID.User_Email = user.User_Email;
                checkuserID.User_Name = user.User_Name;
                checkuserID.User_Password = user.User_Password;

                try
                {
                    //Submit the users new information to the database
                    db.SubmitChanges();
                    return 1;
                }
                catch (IndexOutOfRangeException ex)
                {
                    //Show an error if something goes wrong
                    ex.GetBaseException();
                    return 0;
                }
            }
            else
            {
                //Show an error if something goes wrong
                return -1;
            }
        }

        //Get a list of all the game
        public List<Game> getAllGames()
        {
            //Create a return list
            var listGames = new List<Game>();

            //Select all the games from the database
            dynamic game = (from g in db.Games
                            select g);

            //Go through each product in the database
            foreach (Game gm in game)
            {
                //If the product is active 
                if (gm.Game_Status == 1)
                {
                    var newGame = new Game
                    {
                        //Create a new game object with all the data from the database
                        Game_Title = gm.Game_Title,
                        Game_Type = gm.Game_Type,
                        Game_Descrp = gm.Game_Descrp,
                        Game_Rating = gm.Game_Rating,
                        Game_Quantity = gm.Game_Quantity,
                        Game_Num_Comments = gm.Game_Num_Comments,
                        Game_price = gm.Game_price,
                        Game_Image = gm.Game_Image,
                        Game_Trailer = gm.Game_Trailer,
                        Game_Status = gm.Game_Status,
                        Discount_Percentage = gm.Discount_Percentage,
                        Game_Id = gm.Game_Id,
                        User_ID = gm.User_ID
                    };

                    //newGame.User = null;
                    //Add the new game object to the array
                    listGames.Add(newGame);
                }

            }
            //Return the game list
            return listGames;
        }

        //Helper method to get the price of the game
        //On special is 1
        //Not on special is 0
        /*public decimal isOnSpecial(int ID)
        {

            //Select all game prices that are on special

            var SpecialPrice = (from sp in db.Game_Pricings
                                select sp);

            foreach (Game_Pricing pp in SpecialPrice)
            {
                if (pp.Game_Special == 1)
                {
                    //If the games on special return the special price
                    return pp.Game_Discounted_Price;
                }
                else if (pp.Game_Special == 0)
                { //If not on special, give the normal price
                    return pp.Game_Price;
                }
            }

            return 0;
        }*/

        //Get a list of all the games on special - ID is the game ID
        //Status 1 means it is active
        /* public List<Game> GameOnSpecial()
         {
             //Create a return list
             var GameOnSpecial = new List<Game>();

             //Select all games on special
             dynamic game = (from p in db.Games
                                select p);

             //Go through each game in the database
             foreach (Game g in game)
             {
                 //If the 2 prices are not equal then add this product to the specials list and if the products active
                 if ((g.Game_Pricings.Equals(isOnSpecial(g.Game_Id)) == false) && (g.Game_Status == 1))
                 {
                     var specialGame = new Game
                     {
                         //Create a new game object with all the data from the database
                         Game_Type = g.Game_Type,
                         Game_Title = g.Game_Title,
                         User_ID = g.User_ID,
                         Game_Price = isOnSpecial(g.Game_Id),

                     };
                     //Add the new game object to the array
                     GameOnSpecial.Add(specialGame);
                 }
             }
             //Return the product
             return GameOnSpecial;
         }*/


        //Get a list of all the products on special - ID is the products ID
        //Status 1 means it is active

        public Game GetOneGame(int ID)
        {
            //Create a return list
            var One_Game = new Game();

            //Select all games
            dynamic product = (from p in db.Games
                               where p.Game_Id.Equals(ID)
                               select p);

            //Go through each game in the database
            foreach (Game gm in product)
            {
                //If the product is active 
                if (gm.Game_Status == 1)
                {
                    One_Game = new Game
                    {
                        //Create a new game object with all the data from the database
                        Game_Title = gm.Game_Title,
                        Game_Type = gm.Game_Type,
                        Game_Descrp = gm.Game_Descrp,
                        Game_Rating = gm.Game_Rating,
                        Game_Quantity = gm.Game_Quantity,
                        Game_Num_Comments = gm.Game_Num_Comments,
                        Game_price = gm.Game_price,
                        Game_Image = gm.Game_Image,
                        Game_Trailer = gm.Game_Trailer,
                        Game_Status = gm.Game_Status,
                        Discount_Percentage = gm.Discount_Percentage,
                        Game_Id = gm.Game_Id,
                        User_ID = gm.User_ID
                    };
                    break;
                }
            }

            //Return the product
            return One_Game;
        }

        //Add a new product to the database - This will be called when login is 1
        public int AddGame(string title, string type, string descrption, short special, int rating, int quantity, int num_Comments, decimal price, decimal discountedPrice, string game_image, string game_trailer, int user_ID, int status)
        {

            //1  good
            //-1 bad
            //0 exsists

            //Check if game exsists
            //Select all games
            dynamic product = (from p in db.Games
                               where p.Game_Title.Equals(title)
                               select p);

            foreach (Game gm in product)
            {
                //Game exsists
                return 0;
            }




            //Get todays current date
            DateTime today = DateTime.Today;

            //Create the new game

            var newGame = new Game
            {
                //Create a new game object with all the data from the database
                Game_Title = title,
                Game_Type = type,
                Game_Descrp = descrption,
                Game_Rating = rating,
                Game_Quantity = quantity,
                Game_Num_Comments = num_Comments,
                Game_price = price,
                Game_Image = game_image,
                Game_Date_Added = today,
                Game_Trailer = game_trailer,
                User_ID = user_ID,
                Game_Status = 1,
                Discount_Percentage = discountedPrice
            };



            //Submit to the database
            db.Games.InsertOnSubmit(newGame);



            try
            {
                //Submit the changes
                db.SubmitChanges();
                return 1;
            }
            catch (Exception ex)
            {
                //Show an error if something goes wrong
                ex.GetBaseException();
                return -1;
            }
        }

        //Edit the product details
        public int EditGame(Game game)
        {
            //Check if product exists 
            var checkProduct = (from u in db.Games
                                where u.Game_Id.Equals(game.Game_Id)
                                select u).FirstOrDefault();

            //If the product exists and the product active
            if (checkProduct != null && checkProduct.Game_Status == 1)
            {
                //Assign this game old details to the new details

                checkProduct.Game_Title = game.Game_Title;
                checkProduct.Game_Type = game.Game_Type;
                checkProduct.Game_Descrp = game.Game_Descrp;
                checkProduct.Game_Rating = game.Game_Rating;
                checkProduct.Game_Quantity = game.Game_Quantity;
                checkProduct.Game_Num_Comments = game.Game_Num_Comments;
                checkProduct.Game_price = game.Game_price;
                checkProduct.Game_Image = game.Game_Image;
                checkProduct.Game_Trailer = game.Game_Trailer;
                checkProduct.Game_Date_Added = game.Game_Date_Added;
                checkProduct.User_ID = game.User_ID;


                try
                {
                    //Submit the game new information to the database
                    db.SubmitChanges();
                    return 1;
                }
                catch (IndexOutOfRangeException ex)
                {
                    //Show an error if something goes wrong
                    ex.GetBaseException();
                    return -1;
                }
            }
            else
            {
                //Show an error if something goes wrong
                return 0;
            }
        }

        //Delete the game in the database
        //Product status =  1 is active
        public int RemoveGame(string Game_Title)
        {
            //Check if product exists 
            var GameExists = (from u in db.Games
                              where u.Game_Id.Equals(Game_Title)
                              select u).FirstOrDefault();

            //If the product exists and the game active
            if (GameExists != null && GameExists.Game_Status == 1)
            {
                //Set the game to not be active
                GameExists.Game_Status = 0;

                try
                {
                    //Submit the game new information to the database
                    db.SubmitChanges();
                    return 1;
                }
                catch (IndexOutOfRangeException ex)
                {
                    //Show an error if something goes wrong
                    ex.GetBaseException();
                    return -1;
                }
            }
            else
            {
                //Show an error if something goes wrong
                return 0;
            }

        }


        //Get the game recently added according to todays date
        public List<Game> getRecentGame()
        {
            //Define a list of recent games
            var recentProduct = new List<Game>();

            //Get todays current date
            DateTime today = DateTime.Today;

            //Select all the game from the database
            dynamic games = (from p in db.Games
                             select p);

            //Go through each game in the database
            foreach (Game gm in games)
            {
                //If the game is active 
                if (gm.Game_Status == 1 && gm.Game_Date_Added == today)
                {
                    var Game_Recent = new Game
                    {
                        //Create a new product object with all the data from the database thats most recent
                        Game_Title = gm.Game_Title,
                        Game_Type = gm.Game_Type,
                        Game_Descrp = gm.Game_Descrp,
                        Game_Rating = gm.Game_Rating,
                        Game_Quantity = gm.Game_Quantity,
                        Game_Num_Comments = gm.Game_Num_Comments,
                        //Game_Price = gm.Game_Price,
                    };
                    //Add the new game object to the array
                    recentProduct.Add(Game_Recent);
                }

            }
            //Return the game list
            return recentProduct;
        }

        //Get the users ID
        public User getUser(int id)
        {
            try
            {
                //Check if the user exists
                User user = (from u in db.Users
                             where u.User_ID.Equals(id)
                             select u).FirstOrDefault();

                user.Games = null;
                user.Invoices= null;


                if (user == null)
                {
                    //If they don not then return nothing
                    return null;
                }
                else
                {
                    //If they do return the users ID
                    return user;
                }
            }
            catch
            {
                return null;
            }
           
        }
    }
}
