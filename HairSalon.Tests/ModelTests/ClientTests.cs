using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using HairSalon.Models;

namespace HairSalon.Tests
{
    [TestClass]
    public class ClientTests : IDisposable
    {
        public void Dispose()
        {
            Stylist.DeleteAll();

            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"ALTER TABLE stylists AUTO_INCREMENT = 1;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public ClientTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=meria_thomas_test;";
        }
        [TestMethod]
        public void GetAll_DBStartsEmpty_0()
        {
            //Arrange
            //Act
            int result = Client.GetClients().Count;

            //Assert
            Assert.AreEqual(0, result);
        }

        public void Save_SaveClient()
        {
            //Arrange
            Stylist stylist = new Stylist("Skye");
            stylist.Save();
            Client newClient = new Client("Meria",stylist.Id);
        
            //Act
            newClient.Save();

            //Assert
            Assert.AreEqual(newClient, Client.GetClients()[0]);
        }
    }
}