using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MD.Bidding.DTO;
using MD.Bidding.EF.Models;


namespace MD.Bidding.DAL.Interfaces
{
    public interface ICategoryDataService 
    {
        Category GetCategory(int categoryId);       
        IEnumerable<Category> GetAllCategories();
        int SaveAsDraft(CategoryDTO categoryDTO, string userId);
        void MarkAsDeleted(int categoryId, string userId);
    }
}
