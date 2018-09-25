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
            Client.DeleteAll();
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
            int result = Client.GetAll().Count;

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
            Assert.AreEqual(newClient, Client.GetAll()[0]);
        }
        [TestMethod]
        public void Find_FindClient_Client()
        {
            Stylist newStylist = new Stylist("Meria");
            newStylist.Save();
            Client testClient = new Client("Skye", newStylist.Id);
            testClient.Save();

            Client foundClient = Client.Find(testClient.Id);

            Assert.AreEqual(testClient,foundClient);
        }
        [TestMethod]
        public void Edit_EditClientName()
        {
        //Arrange
        Stylist stylist = new Stylist("Kiran");
        stylist.Save();
        Client client = new Client("Laduree", stylist.Id);
        client.Save();

        //Act
        client.Edit("Pierre Herme");
        Client expectedClient = new Client("Pierre Herme", stylist.Id, client.Id);

        //Assert
        Assert.AreEqual(expectedClient, client);
        }
        [TestMethod]
        public void DeleteClient_DeleteAClient()
        {
        //Arrange
        Stylist stylist = new Stylist("French");
        stylist.Save();
        Client client = new Client("Laduree", stylist.Id);
        client.Save();

        //Act
        Client.DeleteClient(client.Id);
        int actualCount = Client.GetAll().Count;

        //Assert
        Assert.AreEqual(0, actualCount);
        }
    }
}