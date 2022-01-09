using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;

namespace Repository.Ifaces
{
    public interface IProductRepository : IRepository<Product, IActionResult>
    {
        Task<Product> GetProductByName(string name);

        Task<IActionResult> FindProduct(Product item);

        Task<IEnumerable<Product>> GetAllAsync();
    }
}