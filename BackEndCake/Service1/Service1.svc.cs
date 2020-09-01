using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace Service1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        //register function
        public int Register(string username, string password, string email, string address, string cell)
        {
            var newUser = new User
            {
                Username = username,
                Password = password,
                Password2 = password,
                Email = email,
                Address = address,
                Cellphone = cell
            };

            var user = (from u in db.Users
                        where u.Email.Equals(newUser.Email)
                        select u).FirstOrDefault();

            if (user != null)
            {
                //email already in use
                return -1;

            }
            else
            {
                db.Users.InsertOnSubmit(newUser);

                try
                {
                    db.SubmitChanges();
                    return 1;
                }
                catch (Exception ex)
                {
                    ex.GetBaseException();
                    //return unknown error
                    return -2;
                }
            }
        }

        //login function
        public int Login(string username, string password)
        {
            var user = (from u in db.Users
                        where u.Username.Equals(username) && u.Password.Equals(password)
                        select u).FirstOrDefault();

            if (user != null)
            {
                return user.ID;
            }
            else
            {
                //return user doesn't exist
                return -1;
            }
        }

        //updateUser
		public int UpdateUserInfo(int User_ID, string uUsername, string uPassword, string uEmail, string nAddress, string cell)
		{
            //checking if email exists
            if (!checkEmailExists(User_ID, uEmail))
            {
                User uUser = FindUser(User_ID);

                if (uUser != null)
                {
                    uUser.Email = uEmail;
                    uUser.Password = uPassword;
                    uUser.Password2 = uPassword;
                    uUser.Cellphone = cell;
                    uUser.Username = uUsername;
                    uUser.Address = nAddress;

                    try
                    {
                        db.SubmitChanges();
                        return 1;
                    }
                    catch (Exception ex)
                    {
                        //return unknown error
                        return -2;
                    }
                }
                else
                {
                    return -3;
                }
            }
            else
            {
                //return email already exists
                return -1;
            }
		}


        //Add Game
        public int addGame(string title, string type, string description, int rating, decimal price, int quantity, string trailer,
            string image, int special, int status, int comments, int user_ID)
        {
            Game nGame = new Game
            {
                Title = title,
                Description = description,
                Rating = rating,
                Price = price,
                Quantity = quantity,
                Trailer = trailer,
                Image = image,
                Special = special,
                Status = status,
                Comments = comments,
                User_ID = user_ID
            };

            db.Games.InsertOnSubmit(nGame);

            try
            {
                db.SubmitChanges();
                return 1;
            }
            catch (Exception ex)
            {
                ex.GetBaseException();
                //unknown error
                return -1;
            }
        }

        public int editGame(int Game_ID, string title, string type, string description, int rating, decimal price, 
            int quantity, string trailer, string image, int special, int status, int comments, int user_ID)
        {
            Game eG = FindGame(Game_ID);

            if (eG != null)
            {
                eG.Title = title;
                eG.Description = description;
                eG.Rating = rating;
                eG.Price = price;
                eG.Quantity = quantity;
                eG.Trailer = trailer;
                eG.Image = image;
                eG.Special = special;
                eG.Status = status;
                eG.Comments = comments;
                eG.User_ID = user_ID;

                try
                {
                    db.SubmitChanges();
                    return 1;
                }
                catch (Exception ex)
                {
                    ex.GetBaseException();
                    //unknown error
                    return -1;
                }
            }
            else
            {
                return -2;
            }
        }

        public int removeGame(int Game_ID)
        {
            Game rG = FindGame(Game_ID);

            if (rG != null)
            {
                db.Games.DeleteOnSubmit(rG);

                try
                {
                    db.SubmitChanges();
                    return 1;
                }
                catch (Exception ex)
                {
                    return -1;
                }

            }
            else
            {
                    return -2;
            }
        }

        private bool checkEmailExists(int User_ID, String Email)
        {
            var checkData = (from u in db.Users
                             where u.Email.Equals(Email) && u.ID != User_ID
                             select u).FirstOrDefault();

            if (checkData != null)
            {
                //email does exist
                return true;
            }
            else
            {
                //email not in use
                return false;
            }
        }

        //finding a user
        private User FindUser(int ID)
        {
            User u = (from ur in db.Users
                      where ur.ID.Equals(ID)
                      select ur).FirstOrDefault();

            if (u != null)
            {
                return u;
            }
            else
            {
                return null;
            }
        }

        private Game FindGame(int Game_ID)
        {
            Game g = (from ur in db.Games
                      where ur.ID.Equals(Game_ID)
                      select ur).FirstOrDefault();

            if (g != null)
            {
                return g;
            }
            else
            {
                return null;
            }
        }

		
	}
}
