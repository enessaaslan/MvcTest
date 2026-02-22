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
            var query = "select * from Products p inner join PhoneCategories c on p.PhoneId=c.PhoneId";
            using (var connection = _context.CreateConnection())
            {

                var products = await connection.QueryAsync<Product, PhoneCategory, Product>(query, (product, category) =>
                {
                    product.PhoneCategory = category;
                    return product;
                }, splitOn: "PhoneId");
                return View(products.ToList());
            }
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            var query = "insert into Products (ProductName,Price,ProductDescription,PhoneId) values(@Name,@Price,@Description,@PhoneId)";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, product);
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var query = "select * from Products where ProductId=@Id";
            using (var connection = _context.CreateConnection())
            {
                var product = await connection.QuerySingleOrDefaultAsync<Product>(query, new { Id = id });
                if (product == null)
                    return NotFound();
                return View(product);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            var query = "update Products set ProductName=@Name,Price=@Price,ProductDescription=@Description,CategoryId=@CategoryId where ProductId=@Id";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, product);
                return RedirectToAction("Index");
            }
        }
    }
}