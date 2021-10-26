using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product,ProductToReturnDto>()
                .ForMember(d=> d.ProductBrand, o=>o.MapFrom(s=>s.ProductBrand.Name)) //ForMember para evitar  "productType": "Core.Entities.ProductType" y especificar la propiedad que deseamos afectar
                .ForMember(d=> d.ProductType, o=>o.MapFrom(s=>s.ProductType.Name)) // ForMember recibe dos parametros (el parametro a afectar/destino, el source de donde lo quiero afectar)
                .ForMember(d=> d.PictureUrl, o=>o.MapFrom<ProductUrlResolver>());
        }
    }
}