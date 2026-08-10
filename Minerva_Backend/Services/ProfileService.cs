using Microsoft.EntityFrameworkCore;
using Minerva_Backend.Data;
using Minerva_Backend.DTO.Profile;
using Minerva_Backend.GenericResponse;
using Minerva_Backend.IServices;
using Minerva_Backend.Models;

namespace Minerva_Backend.Services
{
    public class ProfileService(AppDbContext _context) : IProfileService
    {
        public async Task<ResponseResult<ProfileResponseDTO>> GetProfile(string userId)
        {
            var user = await _context.Users
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return new ResponseResult<ProfileResponseDTO>
                {
                    Data = null,
                    Message = "User not found.",
                    Status = false,
                };
            }

            if (user.Profile == null)
            {
                var newProfile = new UserProfile
                {
                    UserId = user.Id
                };
                _context.UserProfiles.Add(newProfile);
                await _context.SaveChangesAsync();
                user.Profile = newProfile;
            }

            return new ResponseResult<ProfileResponseDTO>
            {
                Data = MapToDto(user, user.Profile),
                Message = "Profile fetched successfully.",
                Status = true,
            };
        }

        public async Task<ResponseResult<ProfileResponseDTO>> UpdateProfile(string userId, UpdateProfileDto dto)
        {
            var user = await _context.Users
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return new ResponseResult<ProfileResponseDTO>
                {
                    Data = null,
                    Message = "User not found.",
                    Status = false,
                };
            }

            if (user.Profile == null)
            {
                user.Profile = new UserProfile { UserId = user.Id };
                _context.UserProfiles.Add(user.Profile);
            }

            user.Profile.University = dto.University;
            user.Profile.Degree = dto.Degree;
            user.Profile.Semester = dto.Semester;
            user.Profile.Interests = dto.Interests;
            user.Profile.Skills = dto.Skills;
            user.Profile.GitHubUrl = dto.GitHubUrl;
            user.Profile.LinkedInUrl = dto.LinkedInUrl;

            await _context.SaveChangesAsync();

            return new ResponseResult<ProfileResponseDTO>
            {
                Data = MapToDto(user, user.Profile),
                Message = "Profile updated successfully.",
                Status = true,
            };
        }

        public async Task<ResponseResult<string>> UpdateJourney(string userId, UpdateJourneyDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.JourneyType))
            {
                return new ResponseResult<string>
                {
                    Data = null,
                    Message = "Journey type is required.",
                    Status = false,
                };
            }

            var user = await _context.Users
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return new ResponseResult<string>
                {
                    Data = null,
                    Message = "User not found.",
                    Status = false,
                };
            }

            if (user.Profile == null)
            {
                user.Profile = new UserProfile { UserId = user.Id };
                _context.UserProfiles.Add(user.Profile);
            }

            user.Profile.JourneyType = dto.JourneyType;
            await _context.SaveChangesAsync();

            return new ResponseResult<string>
            {
                Data = dto.JourneyType,
                Message = "Journey updated successfully.",
                Status = true,
            };
        }

        private static ProfileResponseDTO MapToDto(AppUser user, UserProfile? profile)
        {
            return new ProfileResponseDTO
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email ?? string.Empty,
                University = profile?.University,
                Degree = profile?.Degree,
                Semester = profile?.Semester,
                Interests = profile?.Interests,
                Skills = profile?.Skills,
                GitHubUrl = profile?.GitHubUrl,
                LinkedInUrl = profile?.LinkedInUrl,
                JourneyType = profile?.JourneyType
            };
        }
    }
}