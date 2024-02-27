using LR_12_WEB_NET.Services.AuthService;
using LR6_WEB_NET.Models.Dto;
using LR6_WEB_NET.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LR_12_WEB_NET.Controllers;

[ApiController]
[Route("[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(ILogger<AuthController> logger, IAuthService authService)
    {
        _logger = logger;
        _authService = authService;
    }


    /// <summary>
    ///     Register user.
    /// </summary>
    /// <returns></returns>
    [HttpPost("register")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<AuthResponseDto> Register([FromBody] UserRegisterDto userRegisterDto)
    {
        Log.Debug("Register user {@UserRegisterDto}", userRegisterDto);
        var result = await _authService.Register(userRegisterDto);
        Response.StatusCode = StatusCodes.Status200OK;
        return new AuthResponseDto
        {
            StatusCode = StatusCodes.Status200OK,
            Description = "Success",
            Token = result.Token
        };
    }

    /// <summary>
    ///     Login user.
    /// </summary>
    /// <returns></returns>
    [HttpPost("login")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<AuthResponseDto> Login([FromBody] UserLoginDto userLoginDto)
    {
        Log.Debug("Login user {@UserLoginDto}", userLoginDto);
        var result = await _authService.Login(userLoginDto);
        Response.StatusCode = StatusCodes.Status200OK;
        return new AuthResponseDto
        {
            StatusCode = StatusCodes.Status200OK,
            Description = "Success",
            Token = result.Token
        };
    }
}