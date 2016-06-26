using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MD.Bidding.Models;

namespace MD.Bidding.BAL.Interfaces
{
    public interface ICategoryService
    {
        Category GetCategory(int categoryId);    
        int SaveAsDraft(Category category, string userId);
        void MarkAsDeleted(int categoryId, string userId);
        IEnumerable<Category> GetAllCategories();
        IEnumerable<Models.Category> GetAllMainCategories();
    }
}
