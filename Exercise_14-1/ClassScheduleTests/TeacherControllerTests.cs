using Moq;
using Microsoft.AspNetCore.Mvc;
using ClassSchedule.Models;
using ClassSchedule.Controllers;

namespace ClassScheduleTests
{
    public class TeacherControllerTests
    {
        [Fact]
        public void IndexActionMethod_ReturnsAViewResult()
        {
            // Arrange
            var mockRepo = new Mock<IRepository<Teacher>>();
            var controller = new TeacherController(mockRepo.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void IndexActionMethod_ModelIsAListOfTeacherObjects()
        {
            // Arrange
            var mockRepo = new Mock<IRepository<Teacher>>();
            mockRepo.Setup(r => r.List(It.IsAny<QueryOptions<Teacher>>()))
                    .Returns(new List<Teacher>());
            var controller = new TeacherController(mockRepo.Object);

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsType<List<Teacher>>(result?.Model);
        }
    }
}
