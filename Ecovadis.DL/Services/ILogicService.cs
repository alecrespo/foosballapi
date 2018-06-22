using Ecovadis.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ecovadis.DL.Services
{
    public interface ILogicService
    {
        Task<bool> IsGameOver(Match match);
        Task CompleteGame(Match match);
        Task<bool> IsPeriodOver(Period period);
        Task CompletePeriod(Period period);

    }
}
