using AutoMapper;
using Final_Project.ApiServices;
using Final_Project.Data.Repositories;
using Final_Project.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Tests;

internal class ServiceShould
{
    private ISpartaApiRepository<TraineeProfile> _repository;
    private ISpartaApiService<TraineeProfile> _sut;

    [SetUp]
    public void SetUp()
    {
        _repository = Mock.Of<ISpartaApiRepository<TraineeProfile>>();
        _sut = new SpartaApiService<TraineeProfile>(_repository);
    }

    [Test]
    [Category("Happy Path")]
    public async Task CreateAsync_ReturnsTrueWhenEntityAdded()
    {
        var profile = new TraineeProfile
        {
            Id = It.IsAny<int>(),
            SpartanId = It.IsAny<string>(),
            Title = It.IsAny<string>(),
        };

        Mock.Get(_repository)
            .Setup(r => r.Add(profile));
        Mock.Get(_repository)
            .Setup(r => r.IsNull)
            .Returns(false);
        var result = _sut.CreateAsync(profile).Result;

        Assert.That(result, Is.True);
    }
    [Test]
    [Category("Happy Path")]
    public async Task CreateAsync_RepositoryMethodsCalledOnce()
    {
        var profile = new TraineeProfile
        {
            Id = It.IsAny<int>(),
            SpartanId = It.IsAny<string>(),
            Title = It.IsAny<string>(),
        };
        Mock.Get(_repository)
           .Setup(r => r.IsNull)
           .Returns(false);
        await _sut.CreateAsync(profile);
        Mock.Get(_repository)
           .Verify(r => r.Add(profile), Times.Once);
        Mock.Get(_repository)
            .Verify(r => r.SaveAsync(), Times.Once);
    }

    [Test]
    [Category("Sad Path")]
    public async Task CreateAsync_ReturnsFalseWhenRepositoryEmpty()
    {
        var profile = new TraineeProfile
        {
            Id = It.IsAny<int>(),
            SpartanId = It.IsAny<string>(),
            Title = It.IsAny<string>(),
        };

        Mock.Get(_repository)
            .Setup(r => r.IsNull)
            .Returns(true);

        var result = _sut.CreateAsync(profile).Result;
        Assert.That(result, Is.False);
    }
    [Test]
    [Category("Sad Path")]
    public async Task CreateAsync_ReturnsFalseWhenEntityNull()
    {
        var profile = (TraineeProfile)null;

        Mock.Get(_repository)
            .Setup(r => r.IsNull)
            .Returns(false);

        var result = _sut.CreateAsync(profile).Result;
        Assert.That(result, Is.False);
    }

    [Test]
    [Category("Happy Path")]
    public async Task DeleteAsync_ReturnsTrueWhenSuccessful()
    {
        var profile = new TraineeProfile
        {
            Id = It.IsAny<int>(),
            SpartanId = It.IsAny<string>(),
            Title = It.IsAny<string>(),
        };

        Mock.Get(_repository)
            .Setup(r => r.IsNull)
            .Returns(false);

        Mock.Get(_repository)
            .Setup(r => r.FindAsync(It.IsAny<int>()).Result)
            .Returns(profile);

        var result = _sut.DeleteAsync(It.IsAny<int>()).Result;
        Assert.That(result, Is.True);
    }
    [Test]
    [Category("Happy Path")]
    public async Task DeleteAsync_RepositoryMethodsCalledOnce()
    {
        var profile = new TraineeProfile
        {
            Id = It.IsAny<int>(),
            SpartanId = It.IsAny<string>(),
            Title = It.IsAny<string>(),
        };

        Mock.Get(_repository)
            .Setup(r => r.IsNull)
            .Returns(false);

        Mock.Get(_repository)
            .Setup(r => r.FindAsync(It.IsAny<int>()).Result)
            .Returns(profile);

        var result = _sut.DeleteAsync(It.IsAny<int>()).Result;
        Mock.Get(_repository)
            .Verify(r => r.FindAsync(It.IsAny<int>()), Times.Once());
        Mock.Get(_repository)
            .Verify(r => r.SaveAsync(), Times.Once());
    }

    [Test]
    [Category("Sad Path")]
    public async Task DeleteAsync_ReturnsFalseWhenRepositoryEmpty()
    {
        Mock.Get(_repository)
            .Setup(r => r.IsNull)
            .Returns(true);

        var result = _sut.DeleteAsync(It.IsAny<int>()).Result;
        Assert.That(result, Is.False);
    }
    [Test]
    [Category("Sad Path")]
    public async Task DeleteAsync_ReturnsFalseWhenEntityNull()
    {
        var profile = (TraineeProfile)null;
        Mock.Get(_repository)
            .Setup(r => r.IsNull)
            .Returns(true);

        Mock.Get(_repository)
           .Setup(r => r.FindAsync(It.IsAny<int>()).Result)
           .Returns(profile);

        var result = _sut.DeleteAsync(It.IsAny<int>()).Result;
        Assert.That(result, Is.False);
    }

    [Test]
    [Category("Happy Path")]
    public async Task GetAllAsync_ReturnsListOfEntities()
    {
        Mock.Get(_repository)
            .Setup(r => r.IsNull)
            .Returns(false);

        var result = _sut.GetAllAsync().Result;

        Assert.That(result, Is.InstanceOf<List<TraineeProfile>>());
    }
    [Test]
    [Category("Sad Path")]
    public async Task GetAllAsync_ReturnsNullWhenRepositoryNull()
    {
        Mock.Get(_repository)
            .Setup(r => r.IsNull)
            .Returns(true);

        var result = _sut.GetAllAsync().Result;

        Assert.That(result, Is.Null);
    }

    [Test]
    [Category("Happy Path")]
    public async Task GetAsync_ReturnsEntity()
    {
        var profile = new TraineeProfile
        {
            Id = It.IsAny<int>(),
            SpartanId = It.IsAny<string>(),
            Title = It.IsAny<string>(),
        };
        Mock.Get(_repository)
            .Setup(r => r.IsNull)
            .Returns(false);

        Mock.Get(_repository)
            .Setup(r => r.FindAsync(It.IsAny<int>()).Result)
            .Returns(profile);
        var result = _sut.GetAsync(It.IsAny<int>()).Result;
        Assert.That(result, Is.EqualTo(profile));
    }
    [Test]
    [Category("Sad Path")]
    public async Task GetAsync_ReturnsNullWhenRepositoryNull()
    {
        Mock.Get(_repository)
            .Setup(r => r.IsNull)
            .Returns(true);

        var result = _sut.GetAsync(It.IsAny<int>()).Result;
        Assert.That(result, Is.Null);
    }

    [Test]
    [Category("Sad Path")]
    public async Task GetAsync_ReturnsNullWhenEntityNull()
    {
        var profile = (TraineeProfile)null;
        Mock.Get(_repository)
            .Setup(r => r.IsNull)
            .Returns(false);
        Mock.Get(_repository)
            .Setup(r => r.FindAsync(It.IsAny<int>()).Result)
            .Returns(profile);

        var result = _sut.GetAsync(It.IsAny<int>()).Result;
        Assert.That(result, Is.Null);
    }

    [Test]
    [Category("Happy Path")]
    public async Task UpdateAsync_ReturnsTrueWhenSuccessful()
    {
        var profile = new TraineeProfile
        {
            Id = It.IsAny<int>(),
            SpartanId = It.IsAny<string>(),
            Title = It.IsAny<string>(),
        };

        Mock.Get(_repository)
            .Setup(r => r.SaveAsync());

        var result = _sut.UpdateAsync(It.IsAny<int>(), profile).Result;
        Assert.That(result, Is.True);
    }
    [Test]
    [Category("Happy Path")]
    public async Task UpdateAsync_RepositoryMethodsCalledOnce()
    {
        var profile = new TraineeProfile
        {
            Id = It.IsAny<int>(),
            SpartanId = It.IsAny<string>(),
            Title = It.IsAny<string>(),
        };

        Mock.Get(_repository)
            .Setup(r => r.SaveAsync());

        await _sut.UpdateAsync(It.IsAny<int>(), profile);
        Mock.Get(_repository)
             .Verify(r => r.Update(profile), Times.Once);
        Mock.Get(_repository)
            .Verify(r => r.SaveAsync(), Times.Once);
    }

    [Test]
    [Category("Sad Path")]
    public async Task UpdateAsync_ReturnsfalseWhenDbUpdateConcurrencyException()
    {
        var profile = new TraineeProfile
        {
            Id = It.IsAny<int>(),
            SpartanId = It.IsAny<string>(),
            Title = It.IsAny<string>(),
        };
        Mock.Get(_repository)
            .Setup(r => r.SaveAsync())
            .Throws<DbUpdateConcurrencyException>();

        var result = _sut.UpdateAsync(It.IsAny<int>(), profile).Result;

        Assert.That(result, Is.False);
    }
}
