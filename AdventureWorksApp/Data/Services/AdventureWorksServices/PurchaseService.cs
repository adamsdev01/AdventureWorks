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
    public class PurchaseService
    {
        private readonly AdventureWorksContext _context;

        public PurchaseService(AdventureWorksContext context)
        {
            _context = context;
        }

        #region Purchase Order Header Data
        /// <summary>
        /// Read lots of data in the grid
        /// </summary>
        /// <param name="queryAttrib"></param>
        /// <returns></returns>
        public async Task<DataSourceResult> ReadPurchaseOrderHeadersByQuerying(DataSourceRequest queryAttrib)
        {
            queryAttrib.Filters.Add(new FilterDescriptor
            {
                Member = "ActiveFlag",
                Value = "Y"
            });

            var purchaseOrderHeadersList = await _context.PurchaseOrderHeader
                .Include(p => p.PurchaseOrderDetail)
                .OrderByDescending(p => p.ModifiedDate)
                .ToDataSourceResultAsync(queryAttrib);

            return purchaseOrderHeadersList;
        }

        /// <summary>
        /// Get PurchaseOrderHeader by Id
        /// </summary>
        /// <param name="purchaseOrderId"></param>
        /// <returns></returns>
        public PurchaseOrderHeader GetPurchaseOrderHeaderById(int purchaseOrderId)
        {
            var purchaseOrderHeaders = _context.PurchaseOrderHeader
                .Include(p => p.PurchaseOrderDetail)
                .Where(e => e.PurchaseOrderId == purchaseOrderId && e.ActiveFlag == "Y")
                .FirstOrDefault();

            return purchaseOrderHeaders;
        }

        /// <summary>
        /// Create a new PurchaseOrderHeader
        /// </summary>
        /// <param name="PurchaseOrderHeader"></param>
        /// <returns></returns>
        public async Task<PurchaseOrderHeader> CreatePurchaseOrderHeader(PurchaseOrderHeader purchaseOrderHeader)
        {
            purchaseOrderHeader.ActiveFlag = "Y";
            purchaseOrderHeader.ModifiedDate = DateTime.Now;

            await _context.AddAsync(purchaseOrderHeader);
            await _context.SaveChangesAsync();

            return purchaseOrderHeader;
        }

        /// <summary>
        /// Update an PurchaseOrderHeader obj
        /// </summary>
        /// <param name="salesOrderHeader"></param>
        /// <returns></returns>
        public async Task<PurchaseOrderHeader> UpdatePurchaseOrderHeader(PurchaseOrderHeader purchaseOrderHeader)
        {
            purchaseOrderHeader.ActiveFlag = "Y";
            purchaseOrderHeader.ModifiedDate = DateTime.Now;

            await _context.AddAsync(purchaseOrderHeader);
            await _context.SaveChangesAsync();

            return purchaseOrderHeader;
        }

        /// <summary>
        /// Set CurrentFlag Flag to 0 and remove from Grid
        /// </summary>
        /// <param name="salesOrderHeader"></param>
        /// <returns></returns>
        public PurchaseOrderHeader DeletePurchaseOrderHeader(PurchaseOrderHeader purchaseOrderHeader)
        {
            purchaseOrderHeader.ActiveFlag = "N";
            purchaseOrderHeader.ModifiedDate = DateTime.Now;

            _context.SaveChanges();

            return purchaseOrderHeader;
        }

        /// <summary>
        /// Get List of PurchaseOrderHeaders
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrderHeader> GetPurchaseOrderHeaders()
        {
            var purchaseOrderHeaders = _context.PurchaseOrderHeader
                .ToList();

            return purchaseOrderHeaders;
        }

        #endregion
    }
}
