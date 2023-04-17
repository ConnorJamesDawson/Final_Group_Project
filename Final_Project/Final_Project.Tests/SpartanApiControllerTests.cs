using Final_Project.ApiServices;
using Final_Project.Controllers.ApiControllers;
using Final_Project.Models;
using Final_Project.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Final_Project.Tests;

internal class SpartanApiControllerTests
{
    private ISpartanApiService<Spartan> _spartanService;
    private ISpartaApiService<TraineeProfile> _profileService;
    private ISpartaApiService<PersonalTracker> _trackerService;
    private SpartanController _sut;

    [SetUp]
    public void SetUp()
    {
        _spartanService = Mock.Of<ISpartanApiService<Spartan>>();
        _profileService = Mock.Of<ISpartaApiService<TraineeProfile>>();
        _trackerService = Mock.Of<ISpartaApiService<PersonalTracker>>();
        _sut = new SpartanController(_spartanService, _trackerService, _profileService);
    }

    [Test]
    [Category("Happy Path")]
    public async Task GetSpartans_ShouldReturnListOfSpartans()
    {
        List<Spartan> spartans = new List<Spartan>
        {
              new Spartan()
        };
        
        Mock.Get(_spartanService)
            .Setup(s => s.GetAllAsync().Result)
            .Returns(spartans);

        var result = await _sut.GetSpartans();

        Assert.That(result.Value, Is.InstanceOf<List<SpartanDTO>>());
    }

    [Test]
    [Category("Sad Path")]
    public async Task GetSpartans_GivenNoSpartans_ShouldReturnNotFound()
    {
        Mock.Get(_spartanService)
            .Setup(s => s.GetAllAsync().Result)
            .Returns((List<Spartan>)null);

        var result = await _sut.GetSpartans();

        Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    [Category("Happy Path")]
    public async Task GetSpartan_ShouldReturnSpartan()
    {
        Spartan spartan = new Spartan();
        
        Mock.Get(_spartanService)
            .Setup(s => s.GetAsync(It.IsAny<string>()).Result)
            .Returns(spartan);

        var result = await _sut.GetSpartan(It.IsAny<string>());

        Assert.That(result.Value, Is.InstanceOf<SpartanDTO>());
    }

    [Test]
    [Category("Sad Path")]
    public async Task GetSpartan_GivenInvalidId_ShouldReturnNotFound()
    {
        Mock.Get(_spartanService)
            .Setup(s => s.GetAsync(It.IsAny<string>()).Result)
            .Returns((Spartan)null);

        var result = await _sut.GetSpartan(It.IsAny<string>());

        Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    [Category("Happy Path")]
    public async Task PutSpartan_ReturnsActionResultWhenUpdatedSuccessfully()
    {
        Spartan spartan = new Spartan();
        SpartanDTO spartanDto = new SpartanDTO();

        Mock.Get(_spartanService)
            .Setup(s => s.GetAsync(It.IsAny<string>()).Result)
            .Returns(spartan);

        Mock.Get(_spartanService)
            .Setup(s => s.UpdateAsync(It.IsAny<string>(), spartan).Result)
            .Returns(true);

        var result = await _sut.PutSpartan(It.IsAny<string>(), spartanDto);

        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    [Category("Sad Path")]
    public async Task PutSpartan_ReturnsNotFoundResultWithNotMatchingIds()
    {
        SpartanDTO spartanDto = new SpartanDTO();
        string id = It.IsAny<string>();

        Mock.Get(_spartanService)
            .Setup(s => s.GetAsync(id).Result)
            .Returns((Spartan)null);

        var result = await _sut.PutSpartan(id, spartanDto);

        Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    [Category("Sad Path")]
    public async Task PutSpartan_ReturnsBadRequestWhenUpdateUnsuccessful()
    {
        Spartan spartan = new Spartan();
        SpartanDTO spartanDto = new SpartanDTO();

        Mock.Get(_spartanService)
            .Setup(s => s.GetAsync(It.IsAny<string>()).Result)
            .Returns(spartan);

        Mock.Get(_spartanService)
            .Setup(s => s.UpdateAsync(It.IsAny<string>(), spartan).Result)
            .Returns(false);

        var result = await _sut.PutSpartan(It.IsAny<string>(), spartanDto);

        Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    [Category("Happy Path")]
    public async Task PostSpartan_ReturnsCreatedAtActionNewSpartan()
    {
        Spartan spartan = new Spartan
        {
            PasswordHash = ""
        };
        SpartanDTO spartanDto = new SpartanDTO
        {
            PasswordHash = ""
        };

        Mock.Get(_spartanService)
            .Setup(s => s.CreateAsync(spartan).Result)
            .Returns(true);

        var actionResult = await _sut.PostSpartan(spartanDto);
        Assert.That(actionResult.Result, Is.InstanceOf<CreatedAtActionResult>());
        var result = actionResult.Result as CreatedAtActionResult;
        Assert.That(result.Value, Is.InstanceOf<SpartanDTO>());
    }

    [Test]
    [Category("Happy Path")]
    public async Task DeleteSpartan_ReturnsNoContentWhenSuccessful()
    {
        string id = It.IsAny<string>();

        Mock.Get(_spartanService)
            .Setup(s => s.GetAsync(id).Result)
            .Returns(new Spartan());

        Mock.Get(_spartanService)
            .Setup(s => s.DeleteAsync(id).Result)
            .Returns(true);

        var result = await _sut.DeleteSpartan(id);

        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    [Category("Sad Path")]
    public async Task DeleteSpartan_ReturnsNotFoundWhenInvalidId()
    {
        string id = It.IsAny<string>();

        Mock.Get(_spartanService)
            .Setup(s => s.GetAsync(id).Result)
            .Returns((Spartan)null);

        var result = await _sut.DeleteSpartan(id);

        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    [Category("Sad Path")]
    public async Task DeleteSpartan_ReturnsNotFoundWhenSuccessful()
    {
        string id = It.IsAny<string>();

        Mock.Get(_spartanService)
            .Setup(s => s.GetAsync(id).Result)
            .Returns(new Spartan());

        Mock.Get(_spartanService)
            .Setup(s => s.DeleteAsync(id).Result)
            .Returns(false);

        var result = await _sut.DeleteSpartan(id);

        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }
}
