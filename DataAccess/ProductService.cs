using LilacInsight.Model;
using LilacInsight.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LilacInsight.DataAccess
{
    public class ProductService:IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productService)
        {
            _productRepository = productService;
        }
        public void AddProduct(Testmaster product)
        {
            _productRepository.Add(product);
        }
        public void RemoveProduct(Testmaster productId)
        {
            _productRepository.Remove(productId);
        }
        public Testmaster GetProduct(string productId)
        {
            var product = new Testmaster { patientid = productId };
            return product;
        }
        public IEnumerable<Testmaster> GetAllProducts()
        {
            var products = _productRepository.GetAll();
            return products;
        }
        public IEnumerable<Testmaster> GetAllProductsByCategory(int categoryId)
        {
            var products = _productRepository.GetAllProductsByCategoryQuery(categoryId);
            return products;
        }
        public IEnumerable<Testmaster> GetAllCategory()
        {
            var categories = _productRepository.GetAllCategoriesQuery();
            return categories;
        }
    }
}