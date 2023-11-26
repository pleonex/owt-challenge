namespace Contactor.Tests.Models.Business;

using System.Collections.Generic;
using Contactor.Models.Domain;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

[SetUpFixture]
public class DatabaseFixture
{
    private const string ConnectionString = @"Data Source=tests.db";

    private static readonly object lockObj = new();
    private static bool databaseInitialized;

    [OneTimeSetUp]
    public void SetupDb()
    {
        lock (lockObj) {
            if (!databaseInitialized) {
                using (var context = CreateContext()) {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                }

                databaseInitialized = true;
            }
        }
    }

    public static ContactsDbContext CreateContext()
        => new ContactsDbContext(
            new DbContextOptionsBuilder<ContactsDbContext>()
                .UseSqlite(ConnectionString)
                .Options);
}
