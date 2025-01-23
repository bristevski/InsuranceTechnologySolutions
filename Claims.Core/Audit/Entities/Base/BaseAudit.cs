namespace Claims.Core.Audit.Entities.Base;

public class BaseAudit
{
    public int Id { get; set; }

    public DateTime Created { get; set; }

    public string HttpRequestType { get; set; }
}
