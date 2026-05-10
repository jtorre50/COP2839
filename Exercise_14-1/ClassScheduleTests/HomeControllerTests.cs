using Moq;
using Microsoft.AspNetCore.Mvc;
using ClassSchedule.Models;
using ClassSchedule.Controllers;

namespace ClassScheduleTests
{
    public class HomeControllerTests
    {
        [Fact]
        public void IndexActionMethod_ReturnsAViewResult()
        {
            // Arrange
            var mockClasses = new Mock<IRepository<Class>>();
            var mockDays = new Mock<IRepository<Day>>();
            var controller = new HomeController(mockClasses.Object, mockDays.Object);

            // Act
            var result = controller.Index(0);

            // Assert
            Assert.IsType<ViewResult>(result);
        }
    }
}
