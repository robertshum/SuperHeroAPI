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
    public class SuperHeroControllerTests
    {
        private Mock<ISuperHeroService> _mockSuperHeroService;
        private SuperHeroController _superHeroController;

        [SetUp]
        public void Setup()
        {
            //mock data context and service
            _mockSuperHeroService = new Mock<ISuperHeroService>();
            _superHeroController = new SuperHeroController(_mockSuperHeroService.Object);
        }

        [Test]
        public async Task GetSuperHeroes_ReturnsOkResult_WhenGetSuperHeroesSuccess()
        {
            // Add expected superheroes here if needed
            var expectedSuperHeroes = new List<SuperHero>();

            _mockSuperHeroService.Setup(service => service.GetAllSuperHeroesAsync()).ReturnsAsync(expectedSuperHeroes);

            //do
            var result = await _superHeroController.Get();
            Assert.That(result, Is.InstanceOf<ActionResult<List<SuperHero>>>());
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetSpecificSuperHero_ReturnsOkResult_WhenGetSpecificSuperHeroSuccess()
        {
            // Add expected superheroes here if needed
            var superHero = new SuperHero
            {
                Id = 1,
                Description = "description",
                FirstName = "name",
                LastName = "name",
                Place = "place",
                //...etc.
            };

            var id = 1;

            _mockSuperHeroService.Setup(service => service.Get(id)).ReturnsAsync(superHero);

            //do
            var result = await _superHeroController.Get(id);
            Assert.That(result, Is.InstanceOf<ActionResult<SuperHero>>());
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetSpecificSuperHero_ReturnsBadRequestResult_WhenGetSpecificSuperHeroFail()
        {
            //non-existent id
            var id = 9999;

            _mockSuperHeroService.Setup(service => service.Get(id)).ThrowsAsync(new SuperHeroNotFoundException());

            //do
            var result = await _superHeroController.Get(id);
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddSuperHero_ReturnsOkResult_WhenSuperHeroAddSuccess()
        {
            var emptyList = new List<int>();

            //info from request
            var superHeroModelView = new SuperHeroModelView
            {
                Name = "name",
                Description = "description",
                FirstName = "name",
                LastName = "name",
                Place = "place",
                PowerIds = emptyList
                //...etc.
            };

            // Add expected superheroes here if needed
            var expectedSuperHeroes = new List<SuperHero>();

            _mockSuperHeroService.Setup(service => service.AddHero(superHeroModelView)).ReturnsAsync(expectedSuperHeroes);

            //do
            var result = await _superHeroController.AddHero(superHeroModelView);

            //assert
            Assert.That(result, Is.InstanceOf<ActionResult<List<SuperHero>>>());
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task EditSuperHero_ReturnsOkResult_WhenEditSuperHeroSuccess()
        {
            //info from request
            var editSuperHeroModelView = new EditSuperHeroModelView
            {
                Id = 1,
                Description = "new description",
                FirstName = "new name",
                LastName = "new name",
                Place = "new place",
                PowerIds = new List<int> { 2 }
                //...etc.
            };

            // Add expected superheroes here if needed
            var expectedSuperHeroes = new List<SuperHero>();

            _mockSuperHeroService.Setup(service => service.UpdateHero(editSuperHeroModelView)).ReturnsAsync(expectedSuperHeroes);

            //do
            var result = await _superHeroController.UpdateHero(editSuperHeroModelView);

            //assert
            Assert.That(result, Is.InstanceOf<ActionResult<List<SuperHero>>>());
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task EditSuperHero_ReturnsBadRequest_WhenEditSuperHeroFailed()
        {
            //info from request
            var editSuperHeroModelView = new EditSuperHeroModelView { Id = 9999, Name = "New Name" };

            // Add expected superheroes here if needed
            var expectedSuperHero = new List<SuperHero>();

            _mockSuperHeroService.Setup(service => service.UpdateHero(editSuperHeroModelView)).ThrowsAsync(new SuperHeroNotFoundException());

            //do
            var result = await _superHeroController.UpdateHero(editSuperHeroModelView);

            //assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task DeleteSuperHero_ReturnsOkResult_WhenDeleteSuperHeroSuccess()
        {
            var id = 1;

            // Add expected superheroes here if needed
            var expectedSuperHero = new List<SuperHero>();

            _mockSuperHeroService.Setup(service => service.Delete(id)).ReturnsAsync(expectedSuperHero);

            //do
            var result = await _superHeroController.Delete(id);

            //assert
            Assert.That(result, Is.InstanceOf<ActionResult<List<SuperHero>>>());
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task DeleteSuperHero_ReturnsBadRequest_WhenDeleteSuperHeroFailed()
        {
            var id = 9999;

            _mockSuperHeroService.Setup(service => service.Delete(id)).ThrowsAsync(new SuperHeroNotFoundException());

            //do
            var result = await _superHeroController.Delete(id);

            //assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }
    }
}
