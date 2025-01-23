using Claims.Core.Audit.Entities.Base;

namespace Claims.Core.Audit.Entities;

public class CoverAudit : BaseAudit
{
    public string CoverId { get; set; }
}
