using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MD.Bidding.EF.Models;
using MD.Bidding.DTO;
using MD.Bidding.EF.UnitOfWork;
using MD.Bidding.DAL.Interfaces;

namespace MD.Bidding.DAL.Concrete
{
    public class CategoryDataService : DataServiceBase, ICategoryDataService
    {
        public CategoryDataService(IUnitOfWork<BiddingEntities> unitOfWork)
            : base(unitOfWork)
        {

        }

        /// <summary>
        /// Gets category with the specified categoryId, and 
        /// returns that category if found. 
        /// </summary>
        /// <param name="categoryId">CategoryId of the category to get.</param>
        /// <returns>Category with the specified categoryId.</returns>
        public Category GetCategory(int categoryId)
        {
            return this.UnitOfWork.Get<Category>().AsQueryable()
                .FirstOrDefault(m =>
                    m.CategoryId == categoryId &&
                    m.Deleted == false
                );
        }

       

        public IEnumerable<Category> GetAllCategories()
        {
            return this.UnitOfWork.Get<Category>().AsQueryable()
                .Where(m => m.Deleted == false);
        }

        public int SaveAsDraft(CategoryDTO categoryDTO, string userId)
        {
            int categoryId = 0;

            if (categoryDTO.CategoryId == 0)
            {

                var n = new Category()
                {
                    Name = categoryDTO.Name,
                    Description = categoryDTO.Description,
                    TimeStamp = DateTime.Now,
                    CreatedOn = DateTime.Now,
                    CreatedBy = userId,
                    Deleted = false
                };

                this.UnitOfWork.Get<Category>().AddNew(n);
                this.UnitOfWork.SaveChanges();
                categoryId = n.CategoryId;

            }
            else
            {
                var category = this.UnitOfWork.Get<Category>().AsQueryable()
                    .FirstOrDefault(m => m.CategoryId == categoryDTO.CategoryId);
                if (category != null)
                {
                    category.Name = categoryDTO.Name;
                    category.Description = categoryDTO.Description;                   
                    category.UpdatedBy = userId;
                    category.TimeStamp = DateTime.Now;

                    this.UnitOfWork.Get<Category>().Update(category);
                    this.UnitOfWork.SaveChanges();
                }
                return categoryDTO.CategoryId;
            }

            return categoryId;
        }


        public void MarkAsDeleted(int categoryId, string userId)
        {

            var category = (from n in this.UnitOfWork.Get<Category>().AsQueryable()
                            where n.CategoryId == categoryId
                            select n
                            ).FirstOrDefault();
            if (category != null)
            {
                category.DeletedBy = userId;
                category.DeletedOn = DateTime.Now;
                category.Deleted = true;
                this.UnitOfWork.Get<Category>().Update(category);
                this.UnitOfWork.SaveChanges();
            }
        }
    }
}
