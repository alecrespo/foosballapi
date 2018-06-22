using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ecovadis.DAL.Contexts;
using Ecovadis.DAL.Models;
using Ecovadis.DL.Models;
using Ecovadis.DL.Services;
using Microsoft.EntityFrameworkCore;

namespace Ecovadis.API.Services
{
    /// <summary>
    /// Whole game flow
    /// </summary>
    public class GameService : IGameService
    {
        private readonly EcovadisContext context;
        private readonly ILogicService logic;
        private readonly IMapper mapper;
        public GameService(ILogicService logic, IMapper mapper, EcovadisContext context)
        {
            this.logic = logic;
            this.context = context;
            this.mapper = mapper;
        }

        public MatchDto Create()
        {
            Match match = new Match();
            match.Periods.Add(new Period());
            context.Match.Add(match);
            context.SaveChanges();
            return mapper.Map<Match, MatchDto>(match);
        }

        public MatchDtoDetail Detail(int matchId)
        {
            var match = context.Match.Include(m => m.Periods)
                .FirstOrDefault(o => o.Id == matchId);
            return mapper.Map<Match, MatchDtoDetail>(match);
        }

        public IEnumerable<MatchDto> List()
        {
            var matches = context.Match.OrderByDescending(o => o.StartTime).ToList();
            return mapper.Map<IEnumerable<Match>, IEnumerable<MatchDto>>(matches);
        }

        public void Remove(int matchId)
        {
            var match = context.Match.FirstOrDefault(o => o.Id == matchId);
            if (match == null) return;
            context.Match.Remove(match);
            context.SaveChanges();
        }

        public MatchDtoDetail Score(int matchId, TableSide side)
        {
            var match = context.Match.Include(o=>o.Periods).ThenInclude(j=>j.Goals).FirstOrDefault(o => o.Id == matchId);
            if (match == null)
            {
                throw new ArgumentNullException();
            }

            if (logic.IsGameOver(match).Result)
            {
                return mapper.Map<Match, MatchDtoDetail>(match);
            }

            var period = match.Periods.FirstOrDefault(o => o.MatchId == match.Id && o.IsFinished == false);
            if (period == null)
            {
                throw new ArgumentNullException();
            }
            period.Goals.Add(new Goal()
            {
                Player = side
            });
            if (logic.IsPeriodOver(period).Result)
            {
                logic.CompletePeriod(period);
                if (logic.IsGameOver(match).Result)
                {
                    logic.CompleteGame(match);
                }
                else
                {
                    match.Periods.Add(new Period());
                }
            }
            context.SaveChanges();
            return mapper.Map<Match, MatchDtoDetail>(match);
        }
    }
}
