namespace Claims.Core.Audit.Entities.Base;

public class BaseAudit
{
    // Unused but required for the migrations...
    public int Id { get; set; }

    public DateTime Created { get; set; }

    public string HttpRequestType { get; set; }
}
