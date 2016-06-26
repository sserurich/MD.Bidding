using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using MD.Bidding.BAL.Concrete;
using MD.Bidding.BAL.Interfaces;
using MD.Bidding.DAL.Interfaces;
using MD.Bidding.DAL.Concrete;


namespace MD.Bidding.DependencyResolver
{
   public class ServiceDependencyResolver: NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IUserService)).To(typeof(UserService));
            Bind(typeof(IProductService)).To(typeof(ProductService));
            Bind(typeof(ICategoryService)).To(typeof(CategoryService));
            Bind(typeof(IMediaService)).To(typeof(MediaService));  
            //DAL
            Bind(typeof(IUserDataService)).To(typeof(UserDataService));
            Bind(typeof(IProductDataService)).To(typeof(ProductDataService));
            Bind(typeof(ICategoryDataService)).To(typeof(CategoryDataService));
            Bind(typeof(IMediaDataService)).To(typeof(MediaDataService));
        }
    }
}
