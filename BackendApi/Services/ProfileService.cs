using Househole_shop.DTOs;
using Househole_shop.Models;
using Househole_shop.Repositories;

namespace Househole_shop.Services{
    public interface IProfileService{
        Task<Profile?> GetProfileByUserIdAsync (string user_id);
        Task Create (string user_id, string fullname, string phone, string address);
        Task Update (string user_id, string fullname, string phone, string address);
    }
    public class ProfileService (ProfileRepository profileRepository): IProfileService
    {
        private readonly ProfileRepository _profileRepository = profileRepository;
        public async Task Create(string user_id, string fullname, string phone, string address)
        {
            await _profileRepository.Create(user_id,fullname,phone, address);
        }

        public async Task<Profile?> GetProfileByUserIdAsync(string user_id)
        {
            var profile = await _profileRepository.GetProfileByUserIdAsync(user_id);
            if (profile == null)
            {
                profile = new Profile
                {
                    user_id = user_id,
                    fullname = "",
                    phone = "",
                    address = ""
                };
            }
            return profile;
        }

        public async Task Update(string user_id, string fullname, string phone, string address)
        {
            await _profileRepository.Update(user_id, fullname, phone, address);
        }
    }
}