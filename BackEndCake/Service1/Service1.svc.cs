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
        public int Register(string username, string password, string email, string address, string cell)
        {
            var newUser = new User
            {
                Username = username,
                Password = password,
                Email = email,
                Address = address,
                Cellphone = cell
            };

            var user = (from u in db.Users
                        where u.Email.Equals(newUser.Email)
                        select u).FirstOrDefault();

            if (user != null)
            {
                return 0;

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
                    return -1;
                }
            }
        }

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
                return 0;
            }
        }
    }
}
