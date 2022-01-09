using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Repository.Ifaces;
using Repository.Models;

namespace Repository
{
    public class PostgresRepository : IProductRepository
    {
        private readonly IDbConnection _connection;

        public PostgresRepository(IDbConnection dbConnection)
        {
            _connection = dbConnection;
        }

        public async Task<IActionResult> Create(Product item)
        {
            try
            {
                var res = await FindAsync(item);
                if (res == null)
                {
                    await CreateAsync(item);
                }
                else
                {
                    await UpdateAsync(item);
                }

                return new OkResult();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while adding to DataBase");
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> Delete(Product item)
        {
            try
            {
                var res = await FindAsync(item);
                if (res != null)
                    await _connection.ExecuteAsync("DELETE FROM product WHERE Id = @Id", new {Id = res.Id});
                return new OkResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> Update(Product item)
        {
            try
            {
                return new OkResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new BadRequestResult();
            }
        }

        public async Task<Product> GetProductByName(string name)
        {
            try
            {
                return await _connection.QueryFirstOrDefaultAsync<Product>(
                    @"SELECT * FROM product WHERE Name = @Name",
                    new {Name = name});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<IActionResult> FindProduct(Product item)
        {
            try
            {
                var res = await FindAsync(item);
                return res != null ? new OkResult() : new NotFoundResult();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while adding to DataBase");
                return new BadRequestResult();
            }

            return new OkResult();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                return await _connection.QueryAsync<Product>("SELECT * FROM product");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        private async Task CreateAsync(Product item)
        {
            await _connection.ExecuteAsync(
                @"INSERT INTO product (Name, Price, StoreName) VALUES(@Name, @Price, @StoreName)",
                new {Name = item.Name, Price = item.Price, StoreName = item.StoreName});
        }

        private async Task<Product> FindAsync(Product item)
        {
            return await _connection.QueryFirstOrDefaultAsync<Product>(
                @"SELECT * FROM product WHERE Name = @Name AND Price = @Price AND StoreName = @StoreName",
                new {Name = item.Name, Price = item.Price, StoreName = item.StoreName});
        }

        private async Task UpdateAsync(Product item)
        {
            await _connection.ExecuteAsync(
                "UPDATE product SET Name = @Name, Price = @Price, StoreName = @StoreName WHERE Name = @Name AND StoreName = @StoreName",
                new {Name = item.Name, Price = item.Price, StoreName = item.StoreName});
        }
    }
}