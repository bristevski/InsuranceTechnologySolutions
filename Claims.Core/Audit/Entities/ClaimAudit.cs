using Claims.Core.Audit.Entities.Base;

namespace Core.Audit.Entities
{
    public class ClaimAudit : BaseAudit
    {
        public string ClaimId { get; set; }
    }
}
