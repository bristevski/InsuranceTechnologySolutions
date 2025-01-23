using Claims.Core.Audit.Entities.Base;

namespace Claims.Core.Audit.Entities;

public class ClaimAudit : BaseAudit
{
    public string ClaimId { get; set; }
}
