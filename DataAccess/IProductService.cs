using LilacInsight.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LilacInsight.DataAccess
{
    public interface IProductService
    {
        void AddProduct(Testmaster product);
        void RemoveProduct(Testmaster productId);
        Testmaster GetProduct(string productId);
        IEnumerable<Testmaster> GetAllProducts();
        IEnumerable<Testmaster> GetAllProductsByCategory(int categoryId);
        IEnumerable<Testmaster> GetAllCategory();
    }
}
