using CourierManagementSystem.Api.Exceptions;
using CourierManagementSystem.Api.Models.Entities;
using CourierManagementSystem.Api.Models.DTOs.Requests;
using CourierManagementSystem.Api.Repositories;
using CourierManagementSystem.Api.Services;
using Moq;
using Xunit;

namespace CourierManagementSystem.Tests.Services;

public class UserContextServiceTest
{
    private readonly UserContextService _userContextService;

    public UserContextServiceTest()
    {
        _userContextService = new UserContextService();
    }

    // TBD
}
