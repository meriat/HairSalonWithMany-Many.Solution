using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using HairSalon.Controllers;
using HairSalon.Models;

namespace HairSalon.Tests
{
    [TestClass]
    public class StylistControllerTest
    {
        [TestMethod]
        public void Index_ReturnsCorrectView_True()
        {
            //Arrange
            HomeController controller = new HomeController();

            //Act
            ActionResult indexView = controller.Index();

            //Assert
            Assert.IsInstanceOfType(indexView, typeof(ViewResult));
        }
 
        [TestMethod]
        public void Index_TestIndexViewData_ViewResult()
        {
            //Arrange
            StylistController controller = new StylistController();
            List <Stylist> allStylists = Stylist.GetAll();

            //Act
            ViewResult result = controller.Index() as ViewResult;
            List <Stylist> actualAllStylists = result.ViewData.Model as List <Stylist>;

            //Assert
            CollectionAssert.AreEqual(allStylists, actualAllStylists);
        }
    }
}
