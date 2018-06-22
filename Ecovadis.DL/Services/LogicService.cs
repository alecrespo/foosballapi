using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecovadis.DAL.Contexts;
using Ecovadis.DAL.Models;

namespace Ecovadis.DL.Services
{
    public class LogicService : ILogicService
    {
        public Task CompleteGame(Match match)
        {
            var redPlayer = match.Periods.Where(p => p.Winner == TableSide.Red).Count();
            var bluePlayer = match.Periods.Where(p => p.Winner == TableSide.Blue).Count();

            if ((redPlayer == 2 && bluePlayer == 0) || (bluePlayer == 2 && redPlayer == 0))
            {
                match.WasOneSided = true;
            }

            if (redPlayer > bluePlayer)
            {
                match.Winner = TableSide.Red;
            }
            else
            {
                match.Winner = TableSide.Blue;
            }
            match.IsFinished = true;
            match.EndTime = DateTime.UtcNow;

            return Task.CompletedTask;


        }

        public Task CompletePeriod(Period period)
        {
            var redPlayer = period.Goals.Where(p => p.Player == TableSide.Red).Count();
            var bluePlayer = period.Goals.Where(p => p.Player == TableSide.Blue).Count();
            if (redPlayer > bluePlayer)
            {
                period.Winner = TableSide.Red;
            }
            else
            {
                period.Winner = TableSide.Blue;
            }
            period.IsFinished = true;
            period.EndTime = DateTime.UtcNow;
            return Task.CompletedTask;
        }
        
        public Task<bool> IsGameOver(Match match)
        {
            if (match.IsFinished)
            {
                return Task.FromResult(true);
            }
            else
            {
                var redPlayer = match.Periods.Where(p => p.Winner == TableSide.Red).Count();
                var bluePlayer = match.Periods.Where(p => p.Winner == TableSide.Blue).Count();
                if (redPlayer == 2 || bluePlayer == 2)
                {
                    return Task.FromResult(true);
                }
                else
                {
                    return Task.FromResult(false);
                }
            }
        }

        public Task<bool> IsPeriodOver(Period period)
        {
            var redPlayer = period.Goals.Where(p => p.Player == TableSide.Red).ToList();
            var bluePlayer = period.Goals.Where(p => p.Player == TableSide.Blue).ToList();
            if (bluePlayer.Count == 10|| redPlayer.Count == 10)
            {
                return Task.FromResult(true);
            }
            else
            {
                return Task.FromResult(false);
            }
        }
    }
}
