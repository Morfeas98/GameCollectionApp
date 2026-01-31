using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCollection.Application.DTOs;

namespace GameCollection.Application.Services
{
    public interface IUserService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<UserProfileDto> GetUserProfileAsync(int userId);
        Task<UserStatsDto> GetUserStatsAsync(int userId);
        Task<UserProfileDto> UpdateUserProfileAsync(int userId, UserProfileUpdateDto updateDto);
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
        Task<List<ActivityDto>> GetRecentActivitiesAsync(int userId, int limit = 5);
        Task<List<CollectionGameDto>> GetRecentNotesAsync(int userId, int limit = 5);

    }

    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int CollectionCount { get; set; }

    }
}
