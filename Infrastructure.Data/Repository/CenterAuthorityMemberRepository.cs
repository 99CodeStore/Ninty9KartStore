using NsdcTraingPartnerHub.Core.Entities;
using NsdcTraingPartnerHub.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NsdcTraingPartnerHub.Data.Repository
{
    public class CenterAuthorityMemberRepository : GenericRepository<CenterAuthorityMember>, ICenterAuthorityMemberRepository
    {
        private NsdcTpDbContext _db;

        public CenterAuthorityMemberRepository(NsdcTpDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(CenterAuthorityMember centerAuthority)
        {
            _db.CenterAuthorityMembers.Update(centerAuthority);
        }
    }
}
