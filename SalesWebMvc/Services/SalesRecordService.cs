using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMvcContext _context;

        public SalesRecordService(SalesWebMvcContext context)
        {
            _context = context;
        }

        
        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var salesQuery = from sale in _context.SalesRecord select sale;

            if (minDate.HasValue)
            {
                salesQuery = salesQuery.Where(sr => minDate <= sr.Date);
            }

            if (maxDate.HasValue)
            {
                salesQuery = salesQuery.Where(sr => sr.Date <= maxDate);
            }

            return await salesQuery
                .Include(sr => sr.Seller)
                .Include(sr => sr.Seller.Department)
                .OrderByDescending(sr => sr.Date)
                .ToListAsync();
        }

        public async Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            var salesQuery = from sale in _context.SalesRecord select sale;

            if (minDate.HasValue)
            {
                salesQuery = salesQuery.Where(sr => minDate <= sr.Date);
            }

            if (maxDate.HasValue)
            {
                salesQuery = salesQuery.Where(sr => sr.Date <= maxDate);
            }

            return await salesQuery
                .Include(sr => sr.Seller)
                .Include(sr => sr.Seller.Department)
                .OrderByDescending(sr => sr.Date)
                .GroupBy(sr => sr.Seller.Department)
                .ToListAsync();
        }

    }
}
