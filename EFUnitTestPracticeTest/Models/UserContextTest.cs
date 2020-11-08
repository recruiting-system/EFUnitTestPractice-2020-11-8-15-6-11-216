using System;
using System.Collections.Generic;
using EFUnitTestPractice.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EFUnitTestPracticeTest.Models
{
    public class UserContextTest
    {
        public static DbContextOptions<UserContext> CreateDbContextOptions(string databaseName)
        {
            var serviceProvider = new ServiceCollection().
                AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<UserContext>();
            builder.UseInMemoryDatabase(databaseName)
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        [Fact]
        public async void Should_return_user_when_the_user_exist()
        {
            // given
            var options = CreateDbContextOptions("database");
            var context = new UserContext(options);
            context.Users.Add(new User() { Name = "ef core" });
            context.SaveChanges();

            // when
            List<User> users = await context.Users.ToListAsync<User>();

            // then
            Assert.Single(users);
            Assert.Equal("ef core", users[0].Name);
        }
    }
}
