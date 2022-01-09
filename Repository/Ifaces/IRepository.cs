using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Repository.Ifaces
{
    public interface IRepository<T, TResult> 
        where T : class
        where TResult : IActionResult
    {
        Task<TResult> Create(T item);

        Task<TResult> Delete(T item);

        Task<TResult> Update(T item);
    }
}