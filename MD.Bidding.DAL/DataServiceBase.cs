using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MD.Bidding.EF.Context;
using MD.Bidding.EF.Models;
using MD.Bidding.EF.UnitOfWork;

namespace MD.Bidding.DAL
{
    public class DataServiceBase
    {
        private IUnitOfWork<BiddingEntities> _unitOfwork;

        protected IUnitOfWork<BiddingEntities> UnitOfWork { get { return this._unitOfwork; } }

        public DataServiceBase(IUnitOfWork<BiddingEntities> unitOfWork)
        {
            this._unitOfwork = unitOfWork;
        }
    }
}
