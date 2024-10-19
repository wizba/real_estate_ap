using Microsoft.EntityFrameworkCore;
using RealEstateAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateAPI.Repositories
{
    public class PersonRepository : IRepository<Person>
    {
        private readonly RealEstateContext _context;

        public PersonRepository(RealEstateContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await _context.People.ToListAsync();
        }

        // Get all clients
        public async Task<IEnumerable<Person>> GetAllClientsAsync()
        {
            return await _context.People.Where(p => p.Role == "Client").ToListAsync();
        }

        // Get all sellers
        public async Task<IEnumerable<Person>> GetAllSellersAsync()
        {
            return await _context.People.Where(p => p.Role == "Seller").ToListAsync();
        }

        public async Task<Person> GetByIdAsync(long id)
        {
            return await _context.People.FindAsync(id);
        }

        public async Task AddAsync(Person person)
        {
            _context.People.Add(person);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Person person)
        {
            _context.Entry(person).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var person = await _context.People.FindAsync(id);
            if (person != null)
            {
                _context.People.Remove(person);
                await _context.SaveChangesAsync();
            }
        }
    }
}
