using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameCollection.Application.DTOs;
using GameCollection.Domain.Entities;
using GameCollection.Domain.Repositories;

namespace GameCollection.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, ITokenService tokenService, IMapper mapper)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            // Check if Username or Email already exists
            if (await _userRepository.UsernameExistsAsync(registerDto.Username))
                throw new ArgumentException("Username already exists");

            if (await _userRepository.EmailExistsAsync(registerDto.Email))
                throw new ArgumentException("Email already exists");

            // Create new User
            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Role = "User", // Default Role
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            // Generate Token
            var token = _tokenService.CreateToken(createdUser);

            return new AuthResponseDto()
            {
                UserId = createdUser.Id,
                Username = createdUser.Username,
                Email = createdUser.Email,
                Token = token,
                TokenExpiration = DateTime.UtcNow.AddDays(7)
            };
        }



        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            // Find User by Username or Email
            var user = await _userRepository.GetByUsernameAsync(loginDto.UsernameOrEmail) ??
                       await _userRepository.GetByEmailAsync(loginDto.UsernameOrEmail);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid username/email or password");

            // Generate Token
            var token = _tokenService.CreateToken(user);

            return new AuthResponseDto
            {
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                Token = token,
                TokenExpiration = DateTime.UtcNow.AddDays(7)
            };
        }


        public async Task<UserProfileDto> GetUserProfileAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            return _mapper.Map<UserProfileDto>(user);
        }
    }
}
