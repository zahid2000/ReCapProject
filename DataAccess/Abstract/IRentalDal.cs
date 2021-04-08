﻿using Core.DataAccess;
using Entities.Concrete;
using Entities.Concrete.Dto;
using Entities.Dto;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Abstract
{
   public interface IRentalDal:IEntityRepository<Rental>
    {
        List<RentalDetailDto> GetRentalDetail(Expression<Func<CarDetailDto, bool>> filter = null);
    }
}
