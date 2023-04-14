using Final_Project.Controllers.ApiControllers;
using Final_Project.Data;
using Final_Project.Data.ApiRepositories;
using Final_Project.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Tests
{
    internal class SpartaApiRepositoryTests
    {

        private SpartaDbContext _context;

        private SpartaApiRepository<TraineeProfile> _sut;


        [OneTimeSetUp]

        public void OneTimeSetup()

        {
            var options = new DbContextOptionsBuilder<SpartaDbContext>()
                .UseInMemoryDatabase("SpartaDB").Options;

            _context = new SpartaDbContext(options);

            _sut = new SpartaApiRepository<TraineeProfile>(_context);

        }


        [SetUp]
        public async Task SetUp()
        {
            if (_context.TraineeProfile != null)
            {
                _context.TraineeProfile.RemoveRange(_context.TraineeProfile);
            }



            _context.TraineeProfile!.AddRange(
            new List<TraineeProfile>
            {
                 new TraineeProfile
                 {
                    Id = 1,
                    Title = "About Bob",
                    WorkExperience = "At Jolly Joes Jolly Chippy",
                    SpartanId = "1"
                 },
               new TraineeProfile
                 {
                    Id = 2,
                    Title = "About Todd",
                    WorkExperience = "At NASA",
                    SpartanId = "1"
                 }
            });
            await _context.SaveChangesAsync();
        }


        [Category("Happy Path")]
        [Category("FindAsync")]
        [Test]
        public void FindAsync_GivenValidID_ReturnsCorrectTraineeProfile()
        {
            var result = _sut.FindAsync(1).Result;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<TraineeProfile>());
            Assert.That(result.Title, Is.EqualTo("About Bob"));
        }

        [Category("Happy Path")]
        [Category("GetAllAsync")]
        [Test]
        public void GetAllAsync_ReturnsListWithCorrectCount()
        {
            var result = _sut.GetAllAsync().Result;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<List<TraineeProfile>>());
            Assert.That(result.Count, Is.EqualTo(2));
        }


        [Category("Happy Path")]
        [Category("Remove")]
        [Test]
        public void Remove_GivenValidID_DeleteTraineeProfile()
        {
            var result = _sut.FindAsync(2).Result;
            _sut.Remove(result);
            _sut.SaveAsync().Wait();
            var hasResult = _sut.GetAllAsync().Result.Contains(result);
            Assert.That(hasResult, Is.False);

        }


        [Category("Happy Path")]
        [Category("Add")]
        [Test]
        public void Add_CreatesANewTraineeProfile()
        {
            TraineeProfile t = new TraineeProfile
            {
                Id = 3,
                Title = "About James",
                WorkExperience = "At McDonalds",
                SpartanId = "1"
            };

            _sut.Add(t);

            _context.SaveChanges();
            var result = _sut.FindAsync(3).Result;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<TraineeProfile>());
            Assert.That(result.Title, Is.EqualTo("About James"));
        }


        [Category("Happy Path")]
        [Category("AddRange")]
        [Test]
        public void AddRange_CreatesANewTraineeProfiles()
        {
            var list = new List<TraineeProfile>
            {
                 new TraineeProfile
                 {
                    Id = 3,
                    Title = "About Byron",
                    WorkExperience = "At WakeyWines",
                    SpartanId = "1"
                 },
               new TraineeProfile
                 {
                    Id = 4,
                    Title = "About Dott",
                    WorkExperience = "At Tesla",
                    SpartanId = "1"
                 }
            };

            _sut.AddRange(list);

            _context.SaveChanges();
            var result = _sut.FindAsync(3).Result;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<TraineeProfile>());
            Assert.That(result.Title, Is.EqualTo("About Byron"));
            var resultList = _sut.GetAllAsync().Result;

            Assert.That(resultList.Count, Is.EqualTo(4));

        }



        [Category("Happy Path")]
        [Category("Update")]
        [Test]
        public async Task Update_ReturnsListWithCorrectValue()
        {
           

            TraineeProfile t = _sut.FindAsync(2).Result;
            t.Title = "About Jones";

            _sut.Update(t);

            await _sut.SaveAsync();
            var result = _sut.FindAsync(2).Result;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<TraineeProfile>());
            Assert.That(result.Title, Is.EqualTo("About Jones"));
        }

    }
}
