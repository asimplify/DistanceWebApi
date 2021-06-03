using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using DistanceWebApi.Helpers;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using DistanceWebApi.Services;
using DistanceWebApi.Models;
using DistanceWebApi.ViewModels.Users;
using System.Text.Json;
using DistanceWebApi.ViewModels;

namespace DistanceWebApi.Controllers
{
    [Authorize]
    [Route("api")]
    [ApiController]
    
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UsersController(IUserService userService, IMapper mapper,IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDtoViewModel)
        {
            var user = _userService.Authenticate(loginDtoViewModel.Username, loginDtoViewModel.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
            return Ok(new
            {
                Id = user.Id,
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString,
                Expire = token.ValidTo
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]RegisterDto dtoModel)
        {
            // map Viewmodel to Model+
            var user = _mapper.Map<User>(dtoModel);

            try
            {
                // create user
                _userService.Create(user, dtoModel.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            var userReadDto = _mapper.Map<UserReadDto>(user);
            return Ok(userReadDto);
        }

        [HttpPost("getdistance")]
        public IActionResult CaculateDistance([FromBody]DistanceDto editDistanceDto)
        {
            LevenshteinAlgo obj = new LevenshteinAlgo();
            var distance = obj.ComputeDistance(editDistanceDto.First, editDistanceDto.Second);
            return Ok(new
            {
                calculatedDistance = distance
            });
        }

       

       
    }
}
