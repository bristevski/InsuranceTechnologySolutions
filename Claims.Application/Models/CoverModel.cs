using Claims.Application.Models.Enums;
using Claims.Core.Claims.Entities.Enums;
using Core.Claims.Entities;

namespace Claims.Application.Models
{
    public class CoverModel
    {
        public CoverModel() { }
        public CoverModel(Cover cover) 
        {
            Id = cover.Id;
            StartDate = cover.StartDate;
            EndDate = cover.EndDate;
            Type = (CoverModelType)cover.Type;
            Premium = cover.Premium;
        }

        public string Id { get; set; }
        public DateTime StartDate { get; set; }    
        public DateTime EndDate { get; set; }
        public CoverModelType Type { get; set; }
        public decimal Premium { get; set; }

        public Cover ToDomainModel()
        {
            return new Cover()
            {
                Id = Id,
                StartDate = StartDate,
                EndDate = EndDate,
                Type = (CoverType)Type,
                Premium = Premium
            };
        }
    }
}
