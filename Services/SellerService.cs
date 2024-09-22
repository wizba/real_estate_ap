using RealEstateAPI.Models;
using RealEstateAPI.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstateAPI.Services
{
    public class SellerService : ISellerService
    {
        private readonly ISellerRepository _sellerRepository;

        public SellerService(ISellerRepository sellerRepository)
        {
            _sellerRepository = sellerRepository;
        }

        public async Task<IEnumerable<Seller>> GetAllSellersAsync()
        {
            return await _sellerRepository.GetAllAsync();
        }

        public async Task<Seller> GetSellerByIdAsync(long id)
        {
            return await _sellerRepository.GetByIdAsync(id);
        }

        public async Task AddSellerAsync(Seller seller)
        {
            await _sellerRepository.AddAsync(seller);
        }

        public async Task UpdateSellerAsync(Seller seller)
        {
            await _sellerRepository.UpdateAsync(seller);
        }

        public async Task DeleteSellerAsync(long id)
        {
            await _sellerRepository.DeleteAsync(id);
        }
    }
}
