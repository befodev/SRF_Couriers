using CourierManagementSystem.Api.Models.DTOs.Requests;
using CourierManagementSystem.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourierManagementSystem.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserContextService _userContextService;

    public AuthController(IAuthService authService, IUserContextService userContextService)
    {
        _authService = authService;
        _userContextService = userContextService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        return Ok(response);
    }

    [HttpGet("debug")]
    [AllowAnonymous]
    public IActionResult Debug()
    {
        var response = _userContextService.GetUserDebugInfo(User);
        return Ok(response);
    }
}
