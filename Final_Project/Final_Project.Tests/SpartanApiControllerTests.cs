using AutoMapper;
using Final_Project.ApiServices;
using Final_Project.Controllers.ApiControllers;
using Final_Project.Models;
using Final_Project.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using System.Security.Principal;

namespace Final_Project.Tests;

internal class SpartanApiControllerTests
{
    private ISpartanApiService<Spartan> _spartanService;

    [SetUp]
    public void SetUp()
    {
        _spartanService = Mock.Of<ISpartanApiService<Spartan>>();
    }

    private SpartanController GetBasicSut(ISpartanApiService<Spartan> mockService)
    {
        var profileService = Mock.Of<ISpartaApiService<TraineeProfile>>();
        var trackerService = Mock.Of<ISpartaApiService<PersonalTracker>>();
        var userRoleService = Mock.Of<ISpartanApiService<IdentityUserRole<string>>>();
        var roleService = Mock.Of<ISpartanApiService<IdentityRole>>();
        var mockUserManager = GetMockUserManager();
        Mock.Get(mockUserManager)
            .Setup(m => m.GetRolesAsync(It.IsAny<Spartan>()).Result)
            .Returns(new List<string>());
        Mock.Get(mockUserManager)
            .Setup(m => m.RemoveFromRolesAsync(It.IsAny<Spartan>(), It.IsAny<List<string>>()).Result)
            .Returns(new IdentityResult());
        Mock.Get(mockUserManager)
            .Setup(m => m.AddToRolesAsync(It.IsAny<Spartan>(), It.IsAny<List<string>>()).Result)
            .Returns(new IdentityResult());
        Mock.Get(roleService)
            .Setup(s => s.GetAllAsync().Result)
            .Returns(new List<IdentityRole> { new IdentityRole
            {
                Name = "Trainee",
                NormalizedName = "TRAINEE"
            }});

        var sut = new SpartanController(mockService, trackerService, profileService, userRoleService, roleService, mockUserManager);
        sut.ControllerContext = new ControllerContext
        {
            HttpContext = GetMockHttpContext(GetMockClaimsPrincipal())
        };
        return sut;
    }

    private UserManager<Spartan> GetMockUserManager()
    {
        return new Mock<UserManager<Spartan>>(
        Mock.Of<IUserStore<Spartan>>(),
        null, null, null, null, null, null, null, null
        ).Object;
    }

    private HttpContext GetMockHttpContext(ClaimsPrincipal mockPrincipal)
    {
        var mockHttpContext = Mock.Of<HttpContext>();
        Mock
        .Get(mockHttpContext)
        .Setup(m => m.User)
        .Returns(mockPrincipal);
        return mockHttpContext;
    }

    private ClaimsPrincipal GetMockClaimsPrincipal(string role = "Trainee", bool isInRole = false)
    {
        var identity = new GenericIdentity("FakeUserName", "");

        var mockPrincipal = Mock.Of<ClaimsPrincipal>();
        Mock
        .Get(mockPrincipal)
        .Setup(x => x.Identity)
        .Returns(identity);
        Mock
        .Get(mockPrincipal)
        .Setup(x => x.IsInRole(role))
        .Returns(isInRole);

        return mockPrincipal;
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

        var result = await GetBasicSut(_spartanService).GetSpartans();

        Assert.That(result.Value, Is.InstanceOf<List<SpartanDTO>>());
    }

    [Test]
    [Category("Sad Path")]
    public async Task GetSpartans_GivenNoSpartans_ShouldReturnNotFound()
    {
        Mock.Get(_spartanService)
            .Setup(s => s.GetAllAsync().Result)
            .Returns((List<Spartan>)null);

        var result = await GetBasicSut(_spartanService).GetSpartans();

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

        var result = await GetBasicSut(_spartanService).GetSpartan(It.IsAny<string>());

        Assert.That(result.Value, Is.InstanceOf<SpartanDTO>());
    }

    [Test]
    [Category("Sad Path")]
    public async Task GetSpartan_GivenInvalidId_ShouldReturnNotFound()
    {
        Mock.Get(_spartanService)
            .Setup(s => s.GetAsync(It.IsAny<string>()).Result)
            .Returns((Spartan)null);

        var result = await GetBasicSut(_spartanService).GetSpartan(It.IsAny<string>());

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

        var result = await GetBasicSut(_spartanService).PutSpartan(It.IsAny<string>(), spartanDto);

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

        var result = await GetBasicSut(_spartanService).PutSpartan(id, spartanDto);

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

        var result = await GetBasicSut(_spartanService).PutSpartan(It.IsAny<string>(), spartanDto);

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
            PasswordHash = "",
            Role = new List<string> { "Trainee" }
        };

        Mock.Get(_spartanService)
            .Setup(s => s.CreateAsync(spartan).Result)
            .Returns(true);

        var actionResult = await GetBasicSut(_spartanService).PostSpartan(spartanDto);
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

        var result = await GetBasicSut(_spartanService).DeleteSpartan(id);

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

        var result = await GetBasicSut(_spartanService).DeleteSpartan(id);

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

        var result = await GetBasicSut(_spartanService).DeleteSpartan(id);

        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }
}
