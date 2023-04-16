using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.Controllers;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Tests
{
    public class DestinationServiceTests
    {
        private ApplicationDbContext _context;
        private DestinationService _destinationService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new ApplicationDbContext(options);

            _destinationService = new DestinationService(_context);
        }

        [Test]
        public async Task CreateDestination_ValidInput_CreatesDestination()
        {
            var viewModel = new CreateDestinationViewModel
            {
                StartingDestination = "New York",
                FinalDestination = "Los Angeles",
                Duration = TimeSpan.FromHours(6),
                Departure = DateTime.Now,
                TimeOfArrival = DateTime.Now.AddHours(6),
                SelectedBusId = Guid.NewGuid(),
                RepeatingDayOfWeek = null,
                TotalPrice = 100
            };

            await _destinationService.CreateDestination(viewModel);

            Assert.AreEqual(1, _context.Destinations.CountAsync().Result);
            var destination = await _context.Destinations.FirstAsync();
            Assert.AreEqual(viewModel.StartingDestination, destination.StartingDestination);
            Assert.AreEqual(viewModel.FinalDestination, destination.FinalDestination);
            Assert.AreEqual(viewModel.Duration, destination.Duration);
            Assert.AreEqual(viewModel.Departure, destination.Departure);
            Assert.AreEqual(viewModel.TimeOfArrival, destination.TimeOfArrival);
            Assert.AreEqual(viewModel.SelectedBusId, destination.BusId);
            Assert.AreEqual(viewModel.RepeatingDayOfWeek, destination.RepeatingDayOfWeek);
            Assert.AreEqual(viewModel.TotalPrice, destination.Price);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }
    }
}