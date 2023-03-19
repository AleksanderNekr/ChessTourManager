﻿using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Insert;
using NUnit.Framework;

namespace ChessTourManager.UnitTests;

[TestFixture]
public class InsertQueriesTests
{
    // For each method of IInsertQueries interface, we need to test the following cases:
    // 1.	Incorrect data.
    // 1.1.	Incorrect data (nulls in required fields).
    // 1.2.	The object to be added already exists (user with the same login).
    // 2.	Correct data.

    // 1.1. Incorrect data (nulls in required fields).
    [Test]
    public void TryAddUser_WhenUserIsNull_ReturnsUserIsNull()
    {
        // Arrange.
        var context = new ChessTourContext();
        var queries = IInsertQueries.CreateInstance(context);

        // Act.
        InsertResult result = queries.TryAddUser(email: null!,
                                                 password: null!,
                                                 lastName: null!,
                                                 firstName: null!);

        // Assert.
        Assert.AreEqual(InsertResult.Fail, result);
    }

    // 1.2. The object to be added already exists (user with the same login).
    [Test]
    public void TryAddUser_WhenLoginIsUsed()
    {
        // Arrange.
        var context = new ChessTourContext();
        var queries = IInsertQueries.CreateInstance(context);

        // Act.
        InsertResult result = queries.TryAddUser(email: "petre@live.com",
                                                 password: "password",
                                                 lastName: "Petrov",
                                                 firstName: "Petr");

        // Assert.
        Assert.AreEqual(InsertResult.Fail, result);
    }

    // 2. Correct data.
    [Test]
    public void TryAddUser_Correct()
    {
        // Arrange.
        var context = new ChessTourContext();
        var queries = IInsertQueries.CreateInstance(context);

        // Act.
        InsertResult result = queries.TryAddUser(email: "petrov_p@yandex.ru",
                                                 password: "password",
                                                 lastName: "Petrov",
                                                 firstName: "Petr");

        // Assert.
        Assert.AreEqual(InsertResult.Success, result);
    }

    // Additional: test correct email format.
    [Test]
    public void TryAddUser_WhenEmailIsIncorrect_ReturnsEmailIsIncorrect()
    {
        // Arrange.
        var context = new ChessTourContext();
        var queries = IInsertQueries.CreateInstance(context);

        // Act.
        InsertResult result = queries.TryAddUser(email: "petrov_p@ya",
                                                 password: "password",
                                                 lastName: "Petrov",
                                                 firstName: "Petr");

        // Assert.
        Assert.AreEqual(InsertResult.Fail, result);
    }
}