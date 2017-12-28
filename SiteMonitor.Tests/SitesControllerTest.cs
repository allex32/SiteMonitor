using KellermanSoftware.CompareNetObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using SiteMonitor.Controllers;
using SiteMonitor.Data.DbContexts;
using SiteMonitor.Models;
using SiteMonitor.Models.Repositories;
using SiteMonitor.Models.SiteViewModels;
using SiteMonitor.Services.SiteAvailability;
using System;

namespace SiteMonitor.Tests
{
    [TestClass]
    public class SitesControllerTest
    {

        private static DbContextOptions<AppDataDbContext> ContextOptions
        {

            get
            {
                // Create a fresh service provider, and therefore a fresh 
                // InMemory database instance.
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                // Create a new options instance telling the context to use an
                // InMemory database and the new service provider.
                var builder = new DbContextOptionsBuilder<AppDataDbContext>();
                builder.UseInMemoryDatabase()
                       .UseInternalServiceProvider(serviceProvider);

                return builder.Options;
            }
        }
        private static SiteController GenerateController(AppDataDbContext dbContext)
        {
            var _siteRepository = new EFSiteRepository(dbContext);
            var _mockJobScheduler = new Mock<ISiteAvailabilityJobScheduler>();


            var _siteService = new SiteAvailabilityService(_siteRepository, _mockJobScheduler.Object);

            return new SiteController(_siteService);
        }


        private Fixture _fixture = new Fixture();
        private CompareLogic compareLogic = new CompareLogic();

        [TestMethod]
        public void Should_ChangeAvailabilityCheckTimeSpanProperty_When_EditMethodInvoked()
        {
            var site = _fixture.Build<EditSiteViewModel>()
                .With(s => s.SiteId, 1)
                .With(s => s.CheckOnlineTimespan, TimeSpan.MinValue)
                .Create();

            var editSideViewModel = _fixture.Build<EditSiteViewModel>()
                .With(s => s.SiteId, 1)
                .With(s => s.CheckOnlineTimespan, TimeSpan.MaxValue)
                .Create();

            using (var context = new AppDataDbContext(ContextOptions))
            {
                context.Sites.Add(site);
                context.SaveChanges();


                var controller = GenerateController(context);
              

                controller.Edit(editSideViewModel.SiteId, editSideViewModel).Wait();

                var modifiedSite = context.Sites.FirstOrDefaultAsync(s => s.SiteId == 1).Result;

                Assert.AreEqual(TimeSpan.MaxValue,modifiedSite.CheckOnlineTimespan);
            }

        }
      }
}
