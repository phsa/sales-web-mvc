using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Services.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Services
{
    public class DepartmentService
    {

        private readonly SalesWebMvcContext _context;

        public DepartmentService(SalesWebMvcContext context)
        {
            _context = context;
        }
        
        
        public async Task InsertAsync(Department department)
        {
            _context.Add(department);
            await _context.SaveChangesAsync();
        }
        
        public async Task RemoveAsync(int id)
        {
            try
            {
                Department toRemove = await FindByIdAsync(id);
                _context.Department.Remove(toRemove);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new IntegrityException("Department has registered sellers and cannot be deleted.");
            }
        }

        public async Task UpdateAsync(Department departmentToUpdate)
        {
            if (!_context.Department.Any(d => d.Id == departmentToUpdate.Id))
            {
                throw new NotFoundException("Department with id = " + departmentToUpdate.Id + " was not found.");
            }

            try
            {
                _context.Department.Update(departmentToUpdate);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }

        public async Task<Department> FindByIdAsync(int id)
        {
            return await _context.Department.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Department>> FindAllAsync()
        {
            return await _context.Department.OrderBy(d => d.Name).ToListAsync();
        }
    }
}
