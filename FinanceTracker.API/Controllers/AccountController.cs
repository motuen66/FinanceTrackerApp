using AutoMapper;
using FinanceTracker.API.Handlers;
using FinanceTracker.API.Models.API;
using FinanceTracker.API.Services;
using FinanceTracker.Domain;
using FinanceTracker.Services.DTOs.UserDtos;
using FinanceTracker.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public AccountController(JwtService jwtService, IUserService userService, IMapper mapper)
        {
            _jwtService = jwtService;
            _userService = userService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _jwtService.Authenticate(loginRequest);
            if (response == null)
            {
                return Unauthorized("Invalid email or password.");
            }
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserMutationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _userService.GetUserByEmailAsync(dto.Email.Trim().ToLowerInvariant());
            if (existingUser != null)
            {
                return Conflict("A user with this email already exists.");
            }

            var newUser = _mapper.Map<User>(dto);
            newUser.Email = newUser.Email.Trim().ToLowerInvariant();
            newUser.PasswordHash = PasswordHashHandler.HashPassword(dto.Password);
            newUser.Name = dto.Name;
            newUser.CreatedAt = DateTime.UtcNow;
            newUser.UpdatedAt = DateTime.UtcNow;

            await _userService.AddAsync(newUser);
            
            var loginReq = new LoginRequestModel
            {
                Email = dto.Email,
                Password = dto.Password
            };
            var response = await _jwtService.Authenticate(loginReq);
            if(response == null)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}