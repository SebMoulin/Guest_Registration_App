using System;
using AutoMapper;
using Company.Welcome.Commons;
using Company.Welcome.Entities;

namespace Company.Welcome.Ral
{
    public class ApplicationMapping : IApplicationMapping
    {
        private MapperConfiguration _mappingConfiguration;
        private IMapper _mapper;

        public void Configure()
        {
            _mappingConfiguration = new MapperConfiguration(ConfigureMapping());
            _mapper = _mappingConfiguration.CreateMapper();
        }

        private static Action<IMapperConfiguration> ConfigureMapping()
        {
            return cfg =>
            {
                //cfg.CreateMap<EntityBase, EntityBaseDto>().ReverseMap();
                //cfg.CreateMap<Tweet, TweetDto>().ReverseMap();
                //cfg.CreateMap<TechnicalSkillCategory, TechnicalSkillCategoryDto>().ReverseMap();
                //cfg.CreateMap<Entities.Profile, ProfileDto>().ReverseMap();
            };
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TDestination>(source);
        }
    }
}