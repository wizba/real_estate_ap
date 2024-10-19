using RealEstateAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace RealEstateAPI.Repositories
{
    public class PropertyRepository : IRepository<Property>
    {
        private readonly RealEstateContext _context;

        public PropertyRepository(RealEstateContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Property>> GetAllAsync()
        {
            return await _context.Properties.Include(p => p.City)
                                            .Include(p => p.Community)
                                            .ToListAsync();
        }

        public async Task<Property> GetByIdAsync(long id)
        {
            return await _context.Properties.Include(p => p.City)
                                            .Include(p => p.Community)
                                            .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Property property)
        {
            _context.Properties.Add(property);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Property property)
        {
            _context.Entry(property).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property != null)
            {
                _context.Properties.Remove(property);
                await _context.SaveChangesAsync();
            }
        }
    }
}
