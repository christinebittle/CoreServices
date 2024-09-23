using CoreEntityFramework.Models;

namespace CoreEntityFramework.Interfaces
{
    public interface ICategoryService
    {


        // base CRUD
        Task<IEnumerable<CategoryDto>> ListCategories();


        Task<CategoryDto?> FindCategory(int id);


        Task<ServiceResponse> UpdateCategory(CategoryDto categoryDto);

        Task<ServiceResponse> AddCategory(CategoryDto categoryDto);

        Task<ServiceResponse> DeleteCategory(int id);

        // related methods

        Task<IEnumerable<CategoryDto>> ListCategoriesForProduct(int id);


        Task<ServiceResponse> LinkCategoryToProduct(int categoryId, int productId);

        Task<ServiceResponse> UnlinkCategoryFromProduct(int categoryId, int productId);
    }
}
