using Househole_shop.DTOs;
using Househole_shop.Models;
using Househole_shop.Repositories;
namespace Househole_shop.Services{
    public interface IFeedBackService{
        Task<IEnumerable<FeedBack>> getAll();
        Task addFeedBack(string user_id, string username,  FeedBackDTO feedBackDTO);
        Task<IEnumerable<FeedBack>> getByUser(string user_id);
    }
    public class FeedBackService(IFeedBackRepository feedBackRepository):IFeedBackService{
        private readonly IFeedBackRepository _feedBackRepository = feedBackRepository;

        public async Task addFeedBack(string user_id, string username,  FeedBackDTO feedBackDTO)
        {
            await _feedBackRepository.addFeedBack(user_id,username,feedBackDTO);
        }

        public async Task<IEnumerable<FeedBack>> getAll()
        {
            return await _feedBackRepository.getAll();
        }

        public async Task<IEnumerable<FeedBack>> getByUser(string user_id)
        {
            return await _feedBackRepository.getByUser(user_id);
        }
    }
}