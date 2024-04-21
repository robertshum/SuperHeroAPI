using NUnit.Framework;
using SuperHeroAPI.Controllers;
using SuperHeroAPI.ModelViews;
using SuperHeroAPI.Services;
using Moq;
using Microsoft.AspNetCore.Mvc;
using SuperHeroAPI.Exceptions;

namespace SuperHeroAPI.Tests
{
    [TestFixture]
    public class PowerControllerTests
    {
        private Mock<IPowerService> _mockPowerService;
        private PowerController _powerController;

        [SetUp]
        public void Setup()
        {
            //mock data context and service
            _mockPowerService = new Mock<IPowerService>();
            _powerController = new PowerController(_mockPowerService.Object);
        }

        [Test]
        public async Task GetPowers_ReturnsOkResult_WhenGetPowersSuccess()
        {
            // Add expected powers here if needed
            var expectedPowers = new List<Power>();

            _mockPowerService.Setup(service => service.GetAllPowersAsync()).ReturnsAsync(expectedPowers);

            //do
            var result = await _powerController.Get();
            Assert.That(result, Is.InstanceOf<ActionResult<List<Power>>>());
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetSpecificPower_ReturnsOkResult_WhenGetSpecificPowerSuccess()
        {
            // Add expected power here if needed
            var power = new Power
            {
                Tag = "Laser Eyes",
                Description = "Shoot lasers from your eye sockets."
            };

            var id = 1;

            _mockPowerService.Setup(service => service.GetPower(id)).ReturnsAsync(power);

            //do
            var result = await _powerController.Get(id);
            Assert.That(result, Is.InstanceOf<ActionResult<Power>>());
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetSpecificPower_ReturnsBadRequestResult_WhenGetSpecificPowerFail()
        {
            //non-existent id
            var id = 9999;

            _mockPowerService.Setup(service => service.GetPower(id)).ThrowsAsync(new PowerNotFoundException());

            //do
            var result = await _powerController.Get(id);
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddPower_ReturnsOkResult_WhenPowerAddSuccess()
        {
            //info from request
            var powerModelView = new PowerModelView { Tag = "Super Strength", Description = "Massive Biceps." };

            // Add expected powers here if needed
            var expectedPowers = new List<Power>();

            _mockPowerService.Setup(service => service.AddPower(powerModelView)).ReturnsAsync(expectedPowers);

            //do
            var result = await _powerController.AddPower(powerModelView);

            //assert
            Assert.That(result, Is.InstanceOf<ActionResult<List<Power>>>());
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task EditPower_ReturnsOkResult_WhenEditPowerSuccess()
        {
            //info from request
            var editPowerModeView = new EditPowerModelView { Id = 1, Tag = "Super Duper Strength", Description = "Massively gigantic Biceps." };

            // Add expected powers here if needed
            var expectedPowers = new List<Power>();

            _mockPowerService.Setup(service => service.Update(editPowerModeView)).ReturnsAsync(expectedPowers);

            //do
            var result = await _powerController.Update(editPowerModeView);

            //assert
            Assert.That(result, Is.InstanceOf<ActionResult<List<Power>>>());
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task EditPower_ReturnsBadRequest_WhenEditPowerFailed()
        {
            //info from request
            var editPowerModeView = new EditPowerModelView { Id = 9999, Tag = "Super Duper Strength", Description = "Massively gigantic Biceps." };

            // Add expected powers here if needed
            var expectedPowers = new List<Power>();

            _mockPowerService.Setup(service => service.Update(editPowerModeView)).ThrowsAsync(new PowerNotFoundException());

            //do
            var result = await _powerController.Update(editPowerModeView);

            //assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task DeletePower_ReturnsOkResult_WhenDeletePowerSuccess()
        {
            var id = 1;

            // Add expected powers here if needed
            var expectedPowers = new List<Power>();

            _mockPowerService.Setup(service => service.Delete(id)).ReturnsAsync(expectedPowers);

            //do
            var result = await _powerController.Delete(id);

            //assert
            Assert.That(result, Is.InstanceOf<ActionResult<List<Power>>>());
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task DeletePower_ReturnsBadRequest_WhenDeletePowerFailed()
        {
            var id = 9999;

            _mockPowerService.Setup(service => service.Delete(id)).ThrowsAsync(new PowerNotFoundException());

            //do
            var result = await _powerController.Delete(id);

            //assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }
    }
}
