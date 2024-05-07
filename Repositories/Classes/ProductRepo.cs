using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using OnlineShoppingApp.Context;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using OnlineShoppingApp.Extentions;
using System;


namespace OnlineShoppingApp.Repositories.Classes
{
    public class ProductRepo : IProductRepo
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ShoppingContext Context { get; }
        public ProductRepo(ShoppingContext _Context, IWebHostEnvironment hostingEnvironment)
        {
            Context = _Context;
            _hostingEnvironment = hostingEnvironment;
        }
        public List<Product> GetAll()
        {

            return Context.Products.Include(p => p.Category).Include(p => p.Brand).Include(p => p.Images).Include(p => p.Comments).Include(p => p.Rates).ToList();

        }
        public Product GetById(int id)
        {
            return Context.Products.Include(p => p.Category).Include(p => p.Brand).Include(p => p.Images).Include(p => p.Rates).Include(p => p.Comments).FirstOrDefault(p => p.Id == id);
        }
        public void insertImage(List<IFormFile> ImageUrl,int productID)
        {
            bool isFirstImage = true; // Flag to track the first image

            foreach (var file in ImageUrl)
            {
                if (file != null && file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);

                    // Construct the image path with the file name and extension
                    var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", fileName);

                    // Save the file to the server
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    // Determine IsMain based on isFirstImage flag
                    var isMain = isFirstImage ? 1 : 0;
                    isFirstImage = false; // Set isFirstImage to false after processing the first image

                    // Save the image path to the database
                    var image = new Images { Source = "/images/" + fileName, IsMain = isMain, ProductId =  productID };
                    Context.Images.Add(image);
                }
            }
        }
        public void Insert(Product product, int userId, List<IFormFile> ImageUrl)
        {
            if (product != null)
            {
                // Add the product to the context
                Context.Products.Add(product);

                // Save changes to generate product ID
                Context.SaveChanges();

                // Create a new instance of the junction entity (ProductSeller)
                var productSeller = new ProductSeller
                {
                    ProductId = product.Id,
                    SellerId = userId,
                };

                Context.ProductSellers.Add(productSeller);

                // Ensure there are image URLs
                if (ImageUrl != null && ImageUrl.Count > 0)
                {
                    insertImage(ImageUrl,product.Id);

                    // Save changes to insert image paths with product ID
                    Context.SaveChanges();
                }
                Context.SaveChanges();
            }
        }
        public void DeleteImage(Images image, string path)
        {     // Remove the image from the database
            
            Context.Images.Remove(image);
            Context.SaveChanges();
            //// Delete the image file from the server
            var imagePath = path.TrimStart('/');
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }
        public void Edit(int id, Product newProduct, int userId, List<IFormFile> ImageUrl)
        {
            // Retrieve the existing product by its id including related images
            Product oldProd = GetById(id);

            // If the product exists
            if (oldProd != null)
            {
                // Update only the properties of the existing product with non-null and changed values
                if (!string.IsNullOrEmpty(newProduct.Name))
                    oldProd.Name = newProduct.Name;

                if (!string.IsNullOrEmpty(newProduct.Description))
                    oldProd.Description = newProduct.Description;

                if (newProduct.Price != 0) // Or any default value you set for Price
                    oldProd.Price = newProduct.Price;

                if (newProduct.categoryId != 0) // Or any default value you set for categoryId
                    oldProd.categoryId = newProduct.categoryId;

                if (newProduct.brandId != 0) // Or any default value you set for brandId
                    oldProd.brandId = newProduct.brandId;

                if (ImageUrl != null && ImageUrl.Any())
                {
                    var imagesCopy = oldProd.Images.ToList();

                    foreach (var image in imagesCopy)
                    {
                        oldProd.Images.Remove(image);
                        DeleteImage(image, image.Source); // Implement this method to delete images
                    }

                    insertImage(ImageUrl, oldProd.Id);
                }

                Context.SaveChanges();
            }
        }
        public void Delete(Product product, int userId)
        {
            //Product oldProd = GetById(id);
            var prodSeller = new ProductSeller
            {
                ProductId = product.Id,
                SellerId = userId,
            };
            Context.ProductSellers.Remove(prodSeller);
            var imagesCopy = product.Images.ToList();
            foreach (var image in imagesCopy)
            {
                // Delete the image from the repository
                DeleteImage(image, image.Source);
            }
            Context.Products.Remove(product);
            Context.SaveChanges();
        }

        public List<Product> GetByName(string Name)
        {
            return Context.Products.Include(p => p.Category).Include(p => p.Brand).Include(p => p.Rates).Include(p => p.Comments)
                .Include(p => p.Images).Where(p => p.Name.StartsWith(Name) ||
                p.Category.Name.StartsWith(Name) || p.Brand.Name.StartsWith(Name) ||
                  p.Description.Contains(Name)).ToList();

        }
        public List<Product> GetProductsStartingWith(string search)
        {
            return GetByName(search);
        }
        public List<Product> GetProductsPerSeller(int sellerId)
        {
            var prodIDS = Context.ProductSellers.Where(ps => ps.SellerId == sellerId).Select(ps => ps.ProductId).ToList();

            var products = Context.Products.Include(p => p.Category).Include(p => p.Brand).Include(p => p.Images).Include(p => p.Rates).Include(p => p.Comments).Where(p => prodIDS.Contains(p.Id)).ToList();

            return products;
        }
        public List<Product> GetBestSellingProducts()
        {

            var bestSellersQuery = Context.OrderItems.GroupBy(o => o.ProductId)
                .Select(g => new { ProductId = g.Key, TotalQuantitySold = g.Sum(o => o.Quantity) })
                .Where(p => p.TotalQuantitySold >= 5);


            var bestSellerProducts = Context.Products.Include(p => p.Category).Include(p => p.Brand)
                                    .Include(p => p.Images).Include(p => p.Rates).Include(p => p.Comments)
                                    .Where(p => bestSellersQuery.Select(q => q.ProductId).Contains(p.Id)).ToList();

            return bestSellerProducts;

        }

        public AppUser GetProductSellerId(int prodID)
        {
            var sellerId = Context.ProductSellers.Include(s=>s.Product).FirstOrDefault(ps => ps.ProductId == prodID).SellerId;

            return Context.Users.FirstOrDefault(s => s.Id == sellerId);
        }

        public void UpdateProductQuantity(int prodID, Product product)
        {
            var prod = Context.Products.Find(prodID);
            if(prod != null)
            {
                prod.Quantity = product.Quantity;
                Context.SaveChanges();
            }
        }
    }
}
