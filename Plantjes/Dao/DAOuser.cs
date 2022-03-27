﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plantjes.Models.Db;
using System.Security.Cryptography;

namespace Plantjes.Dao
{
    internal class DaoUser : DaoBase
    {
        //written by Warre
        /// <summary>
        /// <see cref="GetList{Gebruiker}(bool)"/>.
        /// </summary>
        /// <param name="email">The email to be parsed.</param>
        /// <returns>Returns the user with said email.</returns>
        public static Gebruiker GetUser(string email)
        {
            return GetList<Gebruiker>().FirstOrDefault(g => g.Emailadres == email);
        }

        //written by Renzo
        /// <summary>
        /// Adds a user to the db.
        /// </summary>
        /// <param name="vivesNr">The vives nr of the user.</param>
        /// <param name="firstName">The firstname of the user.</param>
        /// <param name="lastName">The lastname of the user.</param>
        /// <param name="emailadres">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        public static Gebruiker AddUser(string vivesNr, string firstName, string lastName, string emailadres, string password)
        {
            var passwordBytes = Encoding.ASCII.GetBytes(password);
            var md5Hasher = new MD5CryptoServiceProvider();
            var passwordHashed = md5Hasher.ComputeHash(passwordBytes);

            //written by Warre
            int role = 2;
            if (emailadres.ToLower().Contains("@vives.be"))
                role = 0;
            if (emailadres.ToLower().Contains("@student.vives.be"))
                role = 1;

            var gebruiker = new Gebruiker()
            {
                Vivesnr = vivesNr,
                Voornaam = firstName,
                Achternaam = lastName,
                Emailadres = emailadres,
                RolId = role,
                HashPaswoord = passwordHashed
            };
            context.Gebruikers.Add(gebruiker);
            context.SaveChanges();
            return gebruiker;
        }
    }
}
