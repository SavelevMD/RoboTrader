using AutoMapper;
using Models.Business;
using Robo.Database.Models;

namespace Models.Mappers
{
    public class CandleModelToCandleProfile : Profile
    {
        public CandleModelToCandleProfile()
        {
            CreateMap<CandleModel, Candles>()
                .ForMember(r => r.Id, u => u.MapFrom(r => r.Id))
                .ForMember(r => r.Close, u => u.MapFrom(r => r.Close))
                .ForMember(r => r.Open, u => u.MapFrom(r => r.Open))
                .ForMember(r => r.High, u => u.MapFrom(r => r.High))
                .ForMember(r => r.Low, u => u.MapFrom(r => r.Low))
                .ForMember(r => r.Volume, u => u.MapFrom(r => r.Volume))
                .ForMember(r => r.ReceiptTime, u => u.MapFrom(r => r.ReceiptTime))
                .ForMember(r => r.Mtc, u => u.MapFrom(r => r.Mtc))
                .ForMember(r => r.TimeFrame, u => u.MapFrom(r => r.TimeFrame));
        }
    }
}
