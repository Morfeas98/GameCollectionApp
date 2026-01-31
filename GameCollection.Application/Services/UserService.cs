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
        private readonly ICollectionRepository _collectionRepository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, ICollectionRepository collectionRepository, ITokenService tokenService, IMapper mapper)
        {
            _userRepository = userRepository;
            _collectionRepository = collectionRepository;
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
                TokenExpiration = DateTime.UtcNow.AddDays(7),
                Role = user.Role
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
                TokenExpiration = DateTime.UtcNow.AddDays(7),
                Role = user.Role
            };
        }


        public async Task<UserProfileDto> GetUserProfileAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            return new UserProfileDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                Role = user.Role
            };
        }

        public async Task<UserStatsDto> GetUserStatsAsync(int userId)
        {
            return new UserStatsDto
            {
                TotalCollections = await _collectionRepository.GetUserCollectionCountAsync(userId),
                TotalGamesInCollections = await _collectionRepository.GetUserTotalGamesInCollectionsAsync(userId),
                CompletedGames = await _collectionRepository.GetUserCompletedGamesCountAsync(userId),
                CurrentlyPlaying = await _collectionRepository.GetUserCurrentlyPlayingCountAsync(userId),
                AverageRating = await _collectionRepository.GetUserAverageRatingAsync(userId),
                LastActivity = await _collectionRepository.GetUserLastActivityAsync(userId),
                TotalReviews = 0, //TEMP
                WishlistCount = 0 //TEMP
            };
        }

        public async Task<UserProfileDto> UpdateUserProfileAsync(int userId, UserProfileUpdateDto updateDto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            if (user.Username != updateDto.Username)
            {
                if (await _userRepository.UsernameExistsAsync(updateDto.Username))
                    throw new ArgumentException($"Username '{updateDto.Username}' is already taken");

                user.Username = updateDto.Username;
            }

            if (user.Email != updateDto.Email)
            {
                if (await _userRepository.EmailExistsAsync(updateDto.Email))
                    throw new ArgumentException($"Email '{updateDto.Email}' is already in use");

                user.Email = updateDto.Email;
            }

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return new UserProfileDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                CollectionCount = user.Collections?.Count ?? 0
            };
        }

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, user.PasswordHash))
                throw new UnauthorizedAccessException("Current password is incorrect");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<List<ActivityDto>> GetRecentActivitiesAsync(int userId, int limit = 5)
        {
            var activities = new List<ActivityDto>();

            try
            {
                var recentCollections = (await _collectionRepository.GetUserCollectionsAsync(userId))
                    .Take(2);

                activities.AddRange(recentCollections.Select(c => new ActivityDto
                {
                    Type = "CollectionCreated",
                    CollectionName = c.Name,
                    Description = $"Collection: {c.Name} Created",
                    Timestamp = c.CreatedAt,
                    IconClass = "bi-folder-plus",
                    BadgeColor = "primary"
                }));

                var recentGames = await _collectionRepository.GetRecentCollectionGamesByUserAsync(userId, 3);

                activities.AddRange(recentGames.Select(cg => new ActivityDto
                {
                    Type = "GameAdded",
                    GameTitle = cg.Game?.Title ?? "Unknown Game",
                    CollectionName = cg.Collection?.Name ?? "Unknown Collection",
                    Description = $"Game: {cg.Game?.Title ?? "Unknown"} added to {cg.Collection?.Name ?? "Unknown"}",
                    Timestamp = cg.CreatedAt,
                    IconClass = "bi-joystick",
                    BadgeColor = "success"
                }));

                var recentNotes = await _collectionRepository.GetUserNotesAndRatingsAsync(userId, 2);

                activities.AddRange(recentNotes.Select(cg =>
                {
                    var activityType = cg.PersonalNotes != null ? "NoteAdded" : "RatingAdded";
                    var description = cg.PersonalNotes != null
                        ? $"Note added for {cg.Game?.Title ?? "Unknown"}"
                        : $"Rated {cg.Game?.Title ?? "Unknown"}";

                    return new ActivityDto
                    {
                        Type = activityType,
                        GameTitle = cg.Game?.Title ?? "Unknown Game",
                        CollectionName = cg.Collection?.Name ?? "Unknown Collection",
                        Description = description,
                        Timestamp = cg.UpdatedAt ?? cg.CreatedAt,
                        IconClass = cg.PersonalNotes != null ? "bi-pencil" : "bi-star",
                        BadgeColor = "info",
                        Rating = cg.PersonalRating,
                        NotePreview = cg.PersonalNotes?.Length > 30
                            ? cg.PersonalNotes.Substring(0, 30) + "..."
                            : cg.PersonalNotes
                    };
                }));

                return activities
                    .OrderByDescending(a => a.Timestamp)
                    .Take(limit)
                    .ToList();
            }
            catch (Exception ex)
            {
                return new List<ActivityDto>();
            }
        }

        public async Task<List<CollectionGameDto>> GetRecentNotesAsync(int userId, int limit = 5)
        {
            try
            {
                var notes = await _collectionRepository.GetUserNotesAndRatingsAsync(userId, limit);
                return _mapper.Map<List<CollectionGameDto>>(notes);
            }
            catch (Exception ex)
            {
                return new List<CollectionGameDto>();
            }
        }
    }
}
