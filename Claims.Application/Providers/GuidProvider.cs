using Claims.Application.Interfaces;

namespace Claims.Application.Providers
{
    public class GuidProvider : IGuidProvider
    {
        public string NewStringGuid() => Convert.ToString(Guid.NewGuid());
    }
}
