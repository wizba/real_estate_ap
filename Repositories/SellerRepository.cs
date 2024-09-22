using RealEstateAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstateAPI.Repositories
{
    public class SellerRepository : ISellerRepository
    {
        private readonly RealEstateContext _context;

        public SellerRepository(RealEstateContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Seller>> GetAllAsync()
        {
            return await _context.Sellers.ToListAsync();
        }

        public async Task<Seller> GetByIdAsync(long id)
        {
            return await _context.Sellers.FindAsync(id);
        }

        public async Task AddAsync(Seller seller)
        {
            _context.Sellers.Add(seller);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Seller seller)
        {
            _context.Entry(seller).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var seller = await _context.Sellers.FindAsync(id);
            if (seller != null)
            {
                _context.Sellers.Remove(seller);
                await _context.SaveChangesAsync();
            }
        }
    }
}
