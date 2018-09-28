using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using HairSalon.Models;

namespace HairSalon.Tests
{
    [TestClass]
    public class SpecialtyTests : IDisposable
    {
        public void Dispose()
        {
            Specialty.DeleteAll();
        }
        public SpecialtyTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=meria_thomas_test;";
        }
    

        [TestMethod]
        public void GetAll_DBStartsEmpty_0()
        {
        //Arrange
        List<Specialty> allSpecialty = Specialty.GetAll();

        //Act
        int count = allSpecialty.Count;

        //Assert
        Assert.AreEqual(0,count);
        }

        [TestMethod]
        public void Save_SaveSpecialtyToList_True()
        {
        Specialty newSpecialty = new Specialty("Meria Potter");
        newSpecialty.Save();

        List<Specialty> result = Specialty.GetAll();

        Assert.AreEqual(newSpecialty, result[0]);
        }

        [TestMethod]
        public void Find_FoundSpecialtyIsSameAsSavedSpecialty_True()
        {
        //Arrange
        Specialty newSpecialty = new Specialty("Meria Potter");
        newSpecialty.Save();
        int searchId = newSpecialty.Id;

        //Act
        Specialty foundSpecialty = Specialty.Find(searchId);

        //Assert
        Assert.AreEqual(newSpecialty, foundSpecialty);
        }

        [TestMethod]
        public void Edit_EditedSpecialtyHasNewName_True()
        {
        //Arrange
        Specialty newSpecialty = new Specialty("Meria Potter");
        newSpecialty.Save();

        //Act
        string updateName = "Ryan Potter";
        newSpecialty.Edit(updateName);

        //Assert
        Assert.AreEqual(newSpecialty.Name, updateName);
        }
        
        [TestMethod]
        public void Delete_DeleteRightSpecialty_0()
        {
        Specialty newSpecialty = new Specialty("Meria Potter");
        newSpecialty.Save();

        Specialty.DeleteSpecialty(newSpecialty.Id);
        List<Specialty> allSpecialty = Specialty.GetAll();

        Assert.AreEqual(allSpecialty.Count, 0);
        }
    }
}