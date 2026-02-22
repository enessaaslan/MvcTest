using Dapper;
using Microsoft.AspNetCore.Mvc;
using MvcApplication.Data;
using MvcApplication.Models;

namespace MvcApplication.Controllers
{
    public class ProductController : Controller
    {
        private readonly DapperContext _context;
        public ProductController(DapperContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var query = "select * from Products p inner join PhoneCategory c on p.CategoryId=c.CategoryId";
            using (var connection = _context.CreateConnection())
            {
                var products = await connection.QueryAsync<Product, PhoneCategory, Product>(query, (product, category) =>
                {
                    product.PhoneCategory = category;
                    return product;
                }, splitOn: "CategoryId");
                return View(products.ToList());
            }
        }
    }
}
