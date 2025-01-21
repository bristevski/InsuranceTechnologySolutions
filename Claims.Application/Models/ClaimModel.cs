using Claims.Application.Models.Enums;
using Claims.Core.Claims.Entities.Enums;
using Core.Claims.Entities;

namespace Claims.Application.Models
{
    public class ClaimModel
    {
        public ClaimModel() { }
        public ClaimModel(Claim claim) 
        {
            Id = claim.Id;
            CoverId = claim.CoverId;
            Created = claim.Created;
            Name = claim.Name;
            Type = (ClaimModelType)claim.Type;
            DamageCost = claim.DamageCost;
        }

        public string Id { get; set; }
        public string CoverId { get; set; }
        public DateTime Created { get; set; }
        public string Name { get; set; }
        public ClaimModelType Type { get; set; }
        public decimal DamageCost { get; set; }

        public Claim ToDomainModel()
        {
            return new Claim()
            {
                Id = Id,
                CoverId = CoverId,
                Created = Created,
                Name = Name,
                Type = (ClaimType)Type,
                DamageCost = DamageCost
            };
        }
    }
}
