using RealEstateAPI.Models;

namespace RealEstateAPI.Services
{
    public interface ISellerService
    {
        Task<IEnumerable<Seller>> GetAllSellersAsync();
        Task<Seller> GetSellerByIdAsync(long id);
        Task AddSellerAsync(Seller seller);
        Task UpdateSellerAsync(Seller seller);
        Task DeleteSellerAsync(long id);
    }
}
