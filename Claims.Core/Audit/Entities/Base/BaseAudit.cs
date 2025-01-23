namespace Claims.Core.Audit.Entities.Base;

public class BaseAudit
{
    public DateTime Created { get; set; }

    public string HttpRequestType { get; set; }
}
