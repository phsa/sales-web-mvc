using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Services.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace SalesWebMvc.Services
{
    public class SellerService
    {

        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }
        public void Insert(Seller seller)
        {
            _context.Add(seller);
            _context.SaveChanges();
        }

        public void Remove(int id)
        {
            Seller toRemove = FindById(id);
            _context.Seller.Remove(toRemove);
            _context.SaveChanges();
        }

        public void Update(Seller sellerToUpdate)
        {
            if (!_context.Seller.Any(s => s.Id == sellerToUpdate.Id))
            {
                throw new NotFoundException("Seller with id = " + sellerToUpdate.Id + "was not found.");
            }

            try
            {
                _context.Seller.Update(sellerToUpdate);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }

        public Seller FindById(int id)
        {
            return _context.Seller.Include(s => s.Department).FirstOrDefault(s => s.Id == id);
        }

        public List<Seller> FindAll()
        {
            return _context.Seller.ToList();
        }

    }
}
