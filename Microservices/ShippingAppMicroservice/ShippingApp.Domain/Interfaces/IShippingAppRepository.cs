using Models = ShippingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShippingApp.Domain.Interfaces
{
    public interface IShippingAppRepository
    {
        List<Models.Product> GetAllProducts();
        Task<Models.Product> GetProductsbyID(Guid id);
        Task<int> DeleteProductByID(Guid Id);
        Task<int> UpdateProduct(Models.Product productModel);
        Task<int> CreateNewProduct(Models.Product productModel);

        //List<ProductType> GetAllProductType(int CompanyIndex);
        //Task<Entities.ProductType> GetProductTypeByName(string ProductTypeName);
        //Task<Entities.ProductType> GetProductTypeByCode(Guid ProductTypeCode);
        //Task<int> CreateProductType(ProductType productType);
        //Task<int> UpdateProductType(ProductType entity);
        //Task<int> DeleteProductType(Guid ProductTypeCode);

        //Task<int> CreateCountry(Country country);
        //Task<Entities.Country> GetCountryByName(string CountryName);
        //Task<Entities.Country> GetCountryByCode(string CountryCode);
        //Task<int> UpdateCountry(string CountryCode, Country entity);
        //List<Country> GetAllCountry();
        //Task<int> DeleteCountry(string Id);

    }
}
