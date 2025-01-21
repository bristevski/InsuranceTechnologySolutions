using Claims.Core.Audit.Entities.Base;

namespace Core.Audit.Entities
{
    public class CoverAudit : BaseAudit
    {
        public string CoverId { get; set; }
    }
}
