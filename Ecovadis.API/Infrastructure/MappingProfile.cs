using AutoMapper;
using Ecovadis.DAL.Models;
using Ecovadis.DL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecovadis.API.Infrastructure
{
    /// <summary>
    /// Automapper intermediate DTO
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Goal, GoalDto>();
            CreateMap<GoalDto, Goal>();

            CreateMap<Period, PeriodDto>();
            CreateMap<PeriodDto, Period>();

            CreateMap<Match, MatchDto>();
            CreateMap<MatchDto, Match>();

            CreateMap<Match, MatchDtoDetail>();
            CreateMap<MatchDtoDetail, Match>();
        }
    }
}
