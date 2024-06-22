using Househole_shop.DTOs;
using Househole_shop.Models;
using Househole_shop.Repositories;

namespace Househole_shop.Services{
    public interface IProfileService{
        Task<Profile?> GetProfileByUserIdAsync (string userId);
        Task Create (ProfileDTO profileDTO);
        Task Update (ProfileDTO profileDTO);
    }
    public class ProfileService (ProfileRepository profileRepository): IProfileService
    {
        private readonly ProfileRepository _profileRepository = profileRepository;
        public async Task Create(ProfileDTO profileDTO)
        {
            await _profileRepository.Create(profileDTO);
        }

        public async Task<Profile?> GetProfileByUserIdAsync(string userId)
        {
            var profile = await _profileRepository.GetProfileByUserIdAsync(userId);
            if (profile == null)
            {
                throw new InvalidOperationException("Profile not found");
            }
            return profile;
        }

        public async Task Update(ProfileDTO profileDTO)
        {
            await _profileRepository.Update(profileDTO);
        }
    }
}