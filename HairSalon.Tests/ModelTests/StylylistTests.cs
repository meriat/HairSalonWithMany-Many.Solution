using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using HairSalon.Models;

namespace HairSalon.Tests
{
  [TestClass]
  public class StylistTests : IDisposable
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
    public StylistTests()
    {
    DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=meria_thomas_test;";
    }
    [TestMethod]
    public void GetAll_DBStartsEmpty_0()
    {
        //Arrange
        //Act
        int result = Stylist.GetAll().Count;

        //Assert
        Assert.AreEqual(0,result);
    }
    [TestMethod]
    public void GetAll_DBCompareObjects_Equal()
    {
        //Arrange
        List<Stylist> stylists = new List<Stylist>{};
        Stylist newStylist = new Stylist("Skye");
        newStylist.Save();

        //Act
        stylists = Stylist.GetAll();

        //Assert
        Assert.AreEqual(newStylist,stylists[0]);
    }
    [TestMethod]
    public void Find_FindtylistInDatabase_Stylist()
    {
        //Arrange
        Stylist newStylist = new Stylist("Skye");
        newStylist.Save();

        //Act
        Stylist foundStylist = Stylist.Find(newStylist.Id);

        //Assert
        Assert.AreEqual(newStylist, foundStylist);
    }
    }
}