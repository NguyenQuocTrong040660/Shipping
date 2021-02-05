using Models = ShippingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShippingApp.Domain.Interfaces
{
    public interface IShippingAppRepository
    {
        List<Models.ProductModel> GetAllProducts();
        //List<ProductOverview> GetAllProductsHightLight();
        Task<Models.ProductModel> GetProductsbyID(Guid id);
        //List<ProductType> GetAllProductType(int CompanyIndex);
        //ProductGroup GetAllProductGroup();

        //List<ProductType> GetAllCategory();
        //List<Country> GetProductCountry();
        //List<Brand> GetProductBrand();

        //List<ProductOverview> GetProductByCountries(string CountryCode);
        //List<ProductOverview> GetProductByBrand(string BrandCode);
        //Task<Entities.ProductType> GetProductTypeByName(string ProductTypeName);
        //Task<Entities.ProductType> GetProductTypeByCode(Guid ProductTypeCode);
        //Task<int> CreateProductType(ProductType productType);
        //Task<int> UpdateProductType(ProductType entity);
        //Task<int> DeleteProductType(Guid ProductTypeCode);

        //Task<int> CreateProductOverView(ProductOverview productOverview); //not completed
        Task<int> DeleteProductByID(Guid Id);

        //Task<int> UpdateProductOverView(Entities.ProductOverview entity);

        //Task<int> CreateCountry(Country country);
        //Task<Entities.Country> GetCountryByName(string CountryName);
        //Task<Entities.Country> GetCountryByCode(string CountryCode);
        //Task<int> UpdateCountry(string CountryCode, Country entity);
        //List<Country> GetAllCountry();

        //Task<int> CreateBrand(Brand brand);
        //Task<int> UpdateBrand(Brand entity);

        //List<Brand> GetAllBrand(int CompanyIndex);
        //Task<Entities.Brand> GetBrandByCode(Guid Id);
        //Task<int> DeleteBrand(Guid Id);
        //Task<int> DeleteCountry(string Id);

        //Task<Entities.Customer> CreatCustomer(Customer customer);
        //Task<int> CreatOder(Order order);
        Task<int> CreateNewProduct(Models.ProductModel productModel);
    }
}
