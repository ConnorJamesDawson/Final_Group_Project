﻿using Final_Project.ApiServices;
using Final_Project.Controllers.ApiControllers;
using Final_Project.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Final_Project.Tests;

internal class TrackerApiControllerShould
{
    private ISpartaApiService<PersonalTracker> _service;
    private PersonalTrackersApiController _sut;

    [SetUp]
    public void SetUp()
    {
        _service = Mock.Of<ISpartaApiService<PersonalTracker>>();
        _sut = new PersonalTrackersApiController(_service);
    }

    [Test]
    [Category("Happy Path")]
    public async Task GetPersonalTrackers_ShouldReturnListOfTrackers()
    {
        List<PersonalTracker> trackers =  new List<PersonalTracker>
        {
              It.IsAny<PersonalTracker>()
        };
        Mock.Get(_service)
            .Setup(s => s.GetAllAsync().Result)
            .Returns(trackers);

        var result = await _sut.GetPersonal_Tracker();

        Assert.That(result.Value, Is.InstanceOf<List<PersonalTracker>>());
    }

    [Test]
    [Category("Sad Path")]
    public async Task GetPersonalTrackers_ReturnsNotFoundWhenDbSetNull()
    {
        Mock.Get(_service)
            .Setup(s => s.GetAllAsync().Result)
            .Returns((List<PersonalTracker>)null);

        var result = await _sut.GetPersonal_Tracker();

        Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    [Category("Happy Path")]
    public async Task GetPersonalTracker_ReturnsPersonalTracker()
    {
        var tracker = new PersonalTracker{
            Id = It.IsAny<int>(),
            SpartanId = It.IsAny<string>(),
            Title = It.IsAny<string>(),
        };
        Mock.Get(_service)
            .Setup(s => s.GetAsync(It.IsAny<int>()).Result)
            .Returns(tracker);

        var result = await _sut.GetPersonalTracker(It.IsAny<int>());

        Assert.That(result.Value, Is.InstanceOf<PersonalTracker>());
    }

    [Test]
    [Category("Sad Path")]
    public async Task GetPersonalTracker_NotFoundWhenIncorrectId()
    {
        Mock.Get(_service)
            .Setup(s => s.GetAsync(It.IsAny<int>()).Result)
            .Returns((PersonalTracker)null);

        var result = await _sut.GetPersonalTracker(It.IsAny<int>());

        Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    [Category("Happy Path")]
    public async Task PutPersonalTracker_ReturnsNoContentWhenUpdatedSuccessfully()
    {
        var tracker = new PersonalTracker
        {
            Id = It.IsAny<int>(),
            SpartanId = It.IsAny<string>(),
            Title = It.IsAny<string>(),
        };

        Mock.Get(_service)
            .Setup(s => s.UpdateAsync(It.IsAny<int>(), tracker).Result)
            .Returns(true);

        var result = await _sut.PutPersonalTracker(It.IsAny<int>(), tracker);

        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    [Category("Sad Path")]
    public async Task PutPersonalTracker_ReturnsBadRequestWithNotMatchingIds()
    {
        var tracker = new PersonalTracker
        {
            Id = 1,
            SpartanId = It.IsAny<string>(),
            Title = It.IsAny<string>(),
        };

        var result = await _sut.PutPersonalTracker(0, tracker);

        Assert.That(result, Is.InstanceOf<BadRequestResult>());
    }
    [Test]
    [Category("Sad Path")]
    public async Task PutPersonalTracker_ReturnsNotFoundWhenUpdateUnsuccessful()
    {
        var tracker = new PersonalTracker
        {
            Id = It.IsAny<int>(),
            SpartanId = It.IsAny<string>(),
            Title = It.IsAny<string>(),
        };

        Mock.Get(_service)
            .Setup(s => s.UpdateAsync(It.IsAny<int>(), tracker).Result)
            .Returns(false);

        var result = await _sut.PutPersonalTracker(It.IsAny<int>(), tracker);

        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    [Category("Happy Path")]
    public async Task PostPersonalTracker_ReturnsCreatedAtAndNewPersonalTracker()
    {
        var tracker = new PersonalTracker
        {
            SpartanId = It.IsAny<string>(),
            Title = It.IsAny<string>()
        };

        Mock.Get(_service)
            .Setup(s => s.CreateAsync(tracker).Result)
            .Returns(true);

        var result = await _sut.PostPersonalTracker(tracker);

        Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());
    }

   
}
