using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Dtos;
using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;
    private readonly IMapper _mapper;

    public AuthController(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider, IMapper mapper)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpPost("Reg")]
    public async Task<IActionResult> Reg(UserForRegistration userForRegistration)
    {
        if (!await _userRepository.FindEmail(userForRegistration.Email))
        {
            if (userForRegistration.Password == userForRegistration.PasswordConfirm)
            {
                await _userRepository.Add(userForRegistration);
                return Ok("Complete");
            }

            throw new Exception("Password");
        }

        throw new Exception("Email");
    }


    [AllowAnonymous]
    [HttpPost("Log")]
    public async Task<IActionResult> Log(UserForLogin userForLogin)
    {
        User user = await _userRepository.GetByEmail(userForLogin.Email);
        if (_passwordHasher.Verify(userForLogin.Password, user.PasswordHash))
        {
            return Ok(new Dictionary<string, string>
            {
                {"token",_jwtProvider.CreateToken(user.UserId)}
            });
        }
        throw new Exception("Log failed");
    }

    [HttpGet("RefreshToken")]
    public async Task<IActionResult> Refresh()
    {
        User? user = await _userRepository.GetById(Int32.Parse(this.User.FindFirst("userId")?.Value));
        return Ok(new Dictionary<string, string>
        {
            {"token",_jwtProvider.CreateToken(user.UserId)}
        });
    }
}