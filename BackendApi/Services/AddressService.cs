using Househole_shop.DTOs;
using Househole_shop.Models;
using Househole_shop.Repositories;

namespace Househole_shop.Services{
    public interface IAddressService{
        Task<IEnumerable<Address?>> GetAddressByUserIdAsync (string user_id);
        Task Create (string user_id, string name, string phone, string address);
        Task Update (string user_id, string name, string phone, string address);
    }
    public class AddressService (AddressRepository AddressRepository): IAddressService
    {
        private readonly AddressRepository _AddressRepository = AddressRepository;
        public async Task Create(string user_id, string name, string phone, string address)
        {
            await _AddressRepository.Create(user_id,name,phone, address);
        }

        public async Task<IEnumerable<Address?>> GetAddressByUserIdAsync(string user_id)
        {
            var address = await _AddressRepository.GetAddressByUserIdAsync(user_id) as List<Address?> ?? new List<Address?>();

            if (!address.Any())
            {
                address.Add(new Address
                {
                    user_id = user_id,
                    name = "",
                    phone = "",
                    address = ""
                });
            }

            return address;
        }



        public async Task Update(string user_id, string name, string phone, string address)
        {
            await _AddressRepository.Update(user_id, name, phone, address);
        }
    }
}