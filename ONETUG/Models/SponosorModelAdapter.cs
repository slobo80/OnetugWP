using System.Collections.Generic;
using OnetugModel;
namespace Onetug
{
    public class SponosorModelAdapter
    {
        List<SponsorModel> _source = null;
        public SponosorModelAdapter(List<SponsorModel> source)
        {
            _source = source;
        }
        public List<SponsorModelWrapper> Sponsors()
        {
            List<SponsorModelWrapper> result = new List<SponsorModelWrapper>();
            foreach (var sponsors in _source)
            {
                SponsorModelWrapper adapted = new SponsorModelWrapper()
                {
                    Active = sponsors.Active,
                    ImageUrl = sponsors.ImageUrl,
                    ImageName = sponsors.ImageName,
                    Name = sponsors.Name,
                    Website = sponsors.Website
                };
                result.Add(adapted);
            }
            return result;
        }
    }
}
