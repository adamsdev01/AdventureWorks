using AdventureWorksApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;

namespace AdventureWorksApp.Data.Services.AdventureWorksServices
{
    public class SalesService
    {
        private readonly AdventureWorksContext _context;

        public SalesService(AdventureWorksContext context)
        {
            _context = context;
        }

        #region SalesOrderHeader Data
        /// <summary>
        /// Read lots of data in the grid
        /// </summary>
        /// <param name="queryAttrib"></param>
        /// <returns></returns>
        public async Task<DataSourceResult> ReadSalesOrderHeaderByQuerying(DataSourceRequest queryAttrib)
        {
            queryAttrib.Filters.Add(new FilterDescriptor
            {
                Member = "ActiveFlag",
                Value = "Y"
            });

            var salesOrderHeaderList = await _context.SalesOrderHeader
                .OrderByDescending(s => s.ModifiedDate)
                .ToDataSourceResultAsync(queryAttrib);

            return salesOrderHeaderList;
        }

        /// <summary>
        /// Get SalesOrderHeader by Id
        /// </summary>
        /// <param name="slesOrderId"></param>
        /// <returns></returns>
        public SalesOrderHeader GetSalesOrderHeaderById(int salesOrderId)
        {
            var salesOrderHeader = _context.SalesOrderHeader
                .Include(s => s.SalesOrderDetail)
                .Include(s => s.SalesOrderHeaderSalesReason)
                .Where(s => s.SalesOrderId == salesOrderId && s.ActiveFlag == "Y")
                .FirstOrDefault();

            return salesOrderHeader;
        }

        /// <summary>
        /// Create a new SalesOrderHeader
        /// </summary>
        /// <param name="salesOrderHeader"></param>
        /// <returns></returns>
        public async Task<SalesOrderHeader> CreateSalesOrderHeader(SalesOrderHeader salesOrderHeader)
        {
            salesOrderHeader.ActiveFlag = "Y";
            salesOrderHeader.ModifiedDate = DateTime.Now;

            await _context.AddAsync(salesOrderHeader);
            await _context.SaveChangesAsync();

            return salesOrderHeader;
        }

        /// <summary>
        /// Update a new SalesOrderHeader
        /// </summary>
        /// <param name="salesOrderHeader"></param>
        /// <returns></returns>
        public async Task<SalesOrderHeader> UpdateSalesOrderHeader(SalesOrderHeader salesOrderHeader)
        {
            salesOrderHeader.ActiveFlag = "Y";
            salesOrderHeader.ModifiedDate = DateTime.Now;

            await _context.AddAsync(salesOrderHeader);
            await _context.SaveChangesAsync();

            return salesOrderHeader;
        }

        /// <summary>
        /// Set Active Flag to N and remove from Grid
        /// </summary>
        /// <param name="salesOrderHeader"></param>
        /// <returns></returns>
        public SalesOrderHeader DeleteSalesOrderHeader(SalesOrderHeader salesOrderHeader)
        {
            salesOrderHeader.ModifiedDate = DateTime.Now;
            salesOrderHeader.ActiveFlag = "N";

            _context.SaveChanges();

            return salesOrderHeader;
        }

        /// <summary>
        /// Get List of Sales People
        /// </summary>
        /// <returns></returns>
        public List<VSalesPerson> GetSalesPeople()
        {
            var salesPeople = _context.VSalesPerson
                .ToList();

            return salesPeople;
        }


        #endregion
    }
}
