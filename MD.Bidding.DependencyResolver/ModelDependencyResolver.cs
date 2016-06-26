using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using MD.Bidding.EF.UnitOfWork;


namespace MD.Bidding.DependencyResolver
{
    public class ModelDependencyResolver: NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IUnitOfWork<>)).To(typeof(UnitOfWork<>));
        }
    }
}

