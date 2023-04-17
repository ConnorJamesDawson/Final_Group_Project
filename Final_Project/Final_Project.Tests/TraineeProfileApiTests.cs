using Final_Project.ApiServices;
using Final_Project.Controllers.ApiControllers;
using Final_Project.Models;
using Final_Project.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Final_Project.Tests
{
    internal class TraineeProfileApiTests
    {
        [Category("Happy Path")]
        [Category("GetTraineeProfile")]
        [Test]
        public async Task GetTraineeProfile_WhenThereAreProfiles_ReturnsListOfProfiles()
        {
            var mockService = Mock.Of<ISpartaApiService<TraineeProfile>>();
            List<TraineeProfile> profiles = new()
            {
                new TraineeProfile
                {
                    Id = 1,
                    Title = "First profile",
                    AboutMe = "I am first profile"
                }
            };

            Mock
            .Get(mockService)
            .Setup(sc => sc.GetAllAsync().Result)
            .Returns(profiles);


            var sut = new TraineeProfilesControllerApi(mockService);
            var result = await sut.GetTraineeProfile();
            Assert.That(result.Value, Is.InstanceOf<List<TraineeProfileDTO>>());
        }

        [Category("Sad Path")]
        [Category("GetTraineeProfile")]
        [Test]
        public async Task GetTraineeProfile_WhenRepositoryEmpty_ReturnsNull()
        {
            var mockService = Mock.Of<ISpartaApiService<TraineeProfile>>();

            Mock
            .Get(mockService)
            .Setup(sc => sc.GetAllAsync().Result)
            .Returns(() => null);

            var sut = new TraineeProfilesControllerApi(mockService);
            var result = await sut.GetTraineeProfile();
            Assert.That(result.Value, Is.Null);
        }

        [Category("Happy Path")]
        [Category("GetTraineeProfile")]
        [Test]
        public async Task GetTraineeProfile_WhenGivenValidID_ReturnsProfile()
        {
            var mockService = Mock.Of<ISpartaApiService<TraineeProfile>>();

            Mock
            .Get(mockService)
            .Setup(sc => sc.GetAsync(1).Result)
            .Returns(new TraineeProfile());



            var sut = new TraineeProfilesControllerApi(mockService);
            var result = await sut.GetTraineeProfile(1);
            Assert.That(result.Value, Is.InstanceOf<TraineeProfileDTO>());
        }

        [Category("Sad Path")]
        [Category("GetTraineeProfile")]
        [Test]
        public async Task GetTraineeProfile_WhenInvalidId_ReturnsNotFound()
        {
            var mockService = Mock.Of<ISpartaApiService<TraineeProfile>>();

            Mock
            .Get(mockService)
            .Setup(sc => sc.GetAsync(1).Result)
            .Returns(() => null);

            var sut = new TraineeProfilesControllerApi(mockService);
            var result = await sut.GetTraineeProfile(1);
            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }

        [Category("Happy Path")]
        [Category("DeleteTraineeProfile")]
        [Test]
        public async Task DeleteTraineeProfile_RemovesProfile()
        {
            var mockService = new Mock<ISpartaApiService<TraineeProfile>>();

            mockService.Setup(sc => sc.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync(true);

            var sut = new TraineeProfilesControllerApi(mockService.Object);
            var result = await sut.DeleteTraineeProfile(1);

            mockService.Verify(s => s.DeleteAsync(1), Times.Once);
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Category("Sad Path")]
        [Category("DeleteTraineeProfile")]
        [Test]
        public async Task DeleteTraineeProfile_WhenGivenInvalidID_ReturnsNotFound()
        {
            var mockService = new Mock<ISpartaApiService<TraineeProfile>>();

            mockService.Setup(sc => sc.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync(false);

            var sut = new TraineeProfilesControllerApi(mockService.Object);
            var result = await sut.DeleteTraineeProfile(1);

            mockService.Verify(s => s.DeleteAsync(1), Times.Once);
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Category("Happy Path")]
        [Category("PutTraineeProfile")]
        [Test]
        public async Task PutTraineeProfile_WhenGivenDetails_ReturnsCorrectResult()
        {
            var mockService = Mock.Of<ISpartaApiService<TraineeProfile>>();

            TraineeProfile traineeProfileOrigin = new()
            {
                Id = 1,
                Title = "testing",
                AboutMe = "testing 1"
            };
            TraineeProfile traineeProfileUpdate = new()
            {
                Id = 1,
                Title = "testing again",
                AboutMe = "testing 2"
            };
            Mock
            .Get(mockService)
            .Setup(sc => sc.UpdateAsync(1, traineeProfileUpdate).Result)
            .Returns(true);

            var sut = new TraineeProfilesControllerApi(mockService);
            var result = await sut.PutTraineeProfile(1, traineeProfileUpdate);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Category("Sad Path")]
        [Category("PutTraineeProfile")]
        [Test]
        public async Task PutTraineeProfile_WhenGivenInvalidId_ReturnsNotFound()
        {
            var mockService = Mock.Of<ISpartaApiService<TraineeProfile>>();
            TraineeProfile traineeProfile = new()
            {
                Id = 1,
                Title = "testing",
                AboutMe = "testing 1"
            };

            Mock.Get(mockService)
            .Setup(sc => sc.UpdateAsync(0, traineeProfile).Result)
            .Returns(false);

            var sut = new TraineeProfilesControllerApi(mockService);
            var result = await sut.PutTraineeProfile(1, traineeProfile);

            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Category("Sad Path")]
        [Category("PutTraineeProfile")]
        [Test]
        public async Task PutTraineeProfile_WhenGivenMismatchingIds_ReturnsBadRequest()
        {
            var mockService = Mock.Of<ISpartaApiService<TraineeProfile>>();
            TraineeProfile traineeProfile = new()
            {
                Id = 1,
                Title = "testing",
                AboutMe = "testing 1"
            };

            var sut = new TraineeProfilesControllerApi(mockService);
            var result = await sut.PutTraineeProfile(2, traineeProfile);

            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Category("Happy Path")]
        [Category("PostTraineeProfile")]
        [Test]
        public async Task PostTraineeProfile_WhenGivenDetails_AddTraineeProfile()
        {
            var mockService = Mock.Of<ISpartaApiService<TraineeProfile>>();

            TraineeProfile traineeProfile = new()
            {
                Id = 1,
                Title = "testing",
                AboutMe = "testing 1"
            };

            Mock
            .Get(mockService)
            .Setup(sc => sc.CreateAsync(It.IsAny<TraineeProfile>()).Result)
            .Returns(true);

            var sut = new TraineeProfilesControllerApi(mockService);
            var result = await sut.PostTraineeProfile(traineeProfile);

            Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());
        }



        [Category("Sad Path")]
        [Category("PostTraineeProfile")]
        [Test]
        public async Task PostTraineeProfile_WhenGivenNullProfile_ReturnBadRequest()
        {
            var mockService = Mock.Of<ISpartaApiService<TraineeProfile>>();

            Mock
            .Get(mockService)
            .Setup(sc => sc.CreateAsync(It.IsAny<TraineeProfile>()).Result)
            .Returns(false);

            var sut = new TraineeProfilesControllerApi(mockService);
            var result = await sut.PostTraineeProfile(It.IsAny<TraineeProfile>());

            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }
    }
}
