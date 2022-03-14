﻿using NsdcTraingPartnerHub.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NsdcTraingPartnerHub.Core.Interfaces
{
    public interface ITrainingPartnerRepository : IGenericRepository<TrainingPartner>
    {
        void Update(TrainingPartner obj);
    }
}
