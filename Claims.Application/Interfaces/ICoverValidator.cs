using Claims.Application.Models;

namespace Claims.Application.Interfaces
{
    public interface ICoverValidator
    {
        List<string> ValidateModel(CoverModel coverModel);
    }
}
