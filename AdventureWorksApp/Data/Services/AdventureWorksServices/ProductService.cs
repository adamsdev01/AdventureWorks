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
    public class ProductService
    {
        private readonly AdventureWorksContext _context;

        public ProductService(AdventureWorksContext context)
        {
            _context = context;
        }

        #region Person Data
        /// <summary>
        /// Read lots of data in the grid
        /// </summary>
        /// <param name="queryAttrib"></param>
        /// <returns></returns>
        public async Task<DataSourceResult> ReadProductsByQuerying(DataSourceRequest queryAttrib)
        {
            queryAttrib.Filters.Add(new FilterDescriptor
            {
                Member = "ActiveFlag",
                Value = "Y"
            });

            var productsList = await _context.Product
                //.Include(p => p.BillOfMaterialsComponent)
                //.Include(p => p.BillOfMaterialsProductAssembly)
                //.Include(p => p.ProductCostHistory)
                //.Include(p => p.ProductInventory)
                //.Include(p => p.ProductListPriceHistory)
                //.Include(p => p.ProductProductPhoto)
                //.Include(p => p.ProductReview)
                //.Include(p => p.ProductVendor)
                //.Include(p => p.PurchaseOrderDetail)
                //.Include(p => p.ShoppingCartItem)
                //.Include(p => p.SpecialOfferProduct)
                //.Include(p => p.TransactionHistory)
                //.Include(p => p.WorkOrder)
                .OrderByDescending(p => p.ModifiedDate)
                .ToDataSourceResultAsync(queryAttrib);

            return productsList;
        }

        /// <summary>
        /// Get Product by Id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public Product GetProductById(int productId)
        {
            var product = _context.Product
                 .Include(p => p.BillOfMaterialsComponent)
                .Include(p => p.BillOfMaterialsProductAssembly)
                .Include(p => p.ProductCostHistory)
                .Include(p => p.ProductInventory)
                .Include(p => p.ProductListPriceHistory)
                .Include(p => p.ProductProductPhoto)
                .Include(p => p.ProductReview)
                .Include(p => p.ProductVendor)
                .Include(p => p.PurchaseOrderDetail)
                .Include(p => p.ShoppingCartItem)
                .Include(p => p.SpecialOfferProduct)
                .Include(p => p.TransactionHistory)
                .Include(p => p.WorkOrder)
                .Where(p => p.ProductId == productId && p.ActiveFlag == "Y")
                .FirstOrDefault();

            return product;
        }

        /// <summary>
        /// Create a new Product
        /// </summary>
        /// <param name="Product"></param>
        /// <returns></returns>
        public async Task<Product> CreateProduct(Product product)
        {
            product.ActiveFlag = "Y";
            product.ModifiedDate = DateTime.Now;

            await _context.AddAsync(product);
            await _context.SaveChangesAsync();

            return product;
        }

        /// <summary>
        /// Update an Product obj
        /// </summary>
        /// <param name="salesOrderHeader"></param>
        /// <returns></returns>
        public async Task<Product> UpdateProduct(Product product)
        {
            product.ActiveFlag = "N";
            product.ModifiedDate = DateTime.Now;

            await _context.AddAsync(product);
            await _context.SaveChangesAsync();

            return product;
        }

        /// <summary>
        /// Set Active Flag to N and remove from Grid
        /// </summary>
        /// <param name="salesOrderHeader"></param>
        /// <returns></returns>
        public Product DeleteProduct(Product product)
        {
            product.ModifiedDate = DateTime.Now;
            product.ActiveFlag = "N";

            _context.SaveChanges();

            return product;
        }

        /// <summary>
        /// Get List of Products
        /// </summary>
        /// <returns></returns>
        public List<Product> GetProducts()
        {
            var products = _context.Product
                .ToList();

            return products;
        }
    }
    #endregion
}

