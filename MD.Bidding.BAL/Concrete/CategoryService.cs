using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MD.Bidding.DTO;
using MD.Bidding.BAL.Interfaces;
using MD.Bidding.DAL.Interfaces;
using MD.Bidding.Models;
using System.Configuration;
using MD.Bidding.Helpers;


namespace MD.Bidding.BAL.Concrete
{
    public class CategoryService: ICategoryService
    {
        private ICategoryDataService _dataService;
        private IProductService _productService;

        public CategoryService(ICategoryDataService dataService, IProductService productService)
        {
            this._dataService = dataService;
            this._productService = productService;
        }

  
        public Category GetCategory(int categoryId)
        {
            var result = this._dataService.GetCategory(categoryId);
            if(result!=null)
                return MapEFToModel(result);

            return null;
        }

        public IEnumerable<Models.Category> GetAllMainCategories()
        {
            var results = _dataService.GetAllCategories().Where(c => c.ParentId== null).Distinct();
            return MapEFToModel(results);
        }

        public IEnumerable<Models.Category> GetAllCategories()
        {
               var results = _dataService.GetAllCategories();
               return MapEFToModel(results);        
        }

        public int SaveAsDraft(Category category, string userId)
        {
            var c = new DTO.CategoryDTO()
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                //   Description = category.Description,               
                CreatedBy = userId,
                UpdatedBy = userId,
            };

            return this._dataService.SaveAsDraft(c, userId);
        }


        public void MarkAsDeleted(int categoryId, string userId)
        {
            _dataService.MarkAsDeleted(categoryId, userId);
        }   

        private IEnumerable<Category> MapEFToModel(IEnumerable<MD.Bidding.EF.Models.Category> data)
        {
            var list = new List<Category>();
            if (data != null)
            {
                foreach (var result in data)
                {
                    list.Add(MapEFToModel(result));
                }
            }           
            return list;
        }

        public Category MapEFToModel(EF.Models.Category data)
        {
            if(data!=null)
            { 
                string createdBy = string.Empty;
                string updatedBy = string.Empty;          
                 var listSubCategories = new List<Category>();
                 var listCategoryProducts = new List<Product>();
                var parentCategory = string.Empty;
                if(data.Category2 != null){
                    parentCategory = data.Category2.Name;
                }
                if (data.AspNetUser != null)
                {
                    createdBy = data.AspNetUser.FirstName + " " + data.AspNetUser.LastName;
                }
                if (data.AspNetUser2 != null)
                {
                    updatedBy = data.AspNetUser2.FirstName + " " + data.AspNetUser2.LastName;
                }

               

                if (data.Category1 != null)
                {
                    if (data.Category1.Any())
                    {
                        foreach (var result in data.Category1)
                        {
                            listSubCategories.Add(new Category()
                            {
                                CategoryId = result.CategoryId,
                                Name = result.Name,
                                Description = result.Description,
                                //UpdatedBy = updatedBy,
                                //TimeStamp = data.TimeStamp,
                                //CreatedOn = data.CreatedOn,
                                //CreatedBy = createdBy,
                            });
                        }
                    }
                    
                }

                if (data.Products != null)
                {
                    if (data.Products.Any())
                    {
                        listCategoryProducts = _productService.MapEFToModel(data.Products).ToList();                        
                    }
                    
                }
          


              var category = new Category()
                {
                    CategoryId = data.CategoryId,
                    Name = data.Name,
                    Description = data.Description,                   
                    UpdatedBy = updatedBy,
                    TimeStamp = data.TimeStamp,
                    CreatedOn  = data.CreatedOn,
                    CreatedBy = createdBy,
                    ParentId = data.ParentId,
                    ChildCategories = listSubCategories,
                    Products = listCategoryProducts,
                    ParentCategory = parentCategory
                };
                return category;

            }

            return null;
        }

       
           
   
    }
}
