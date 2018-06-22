using Ecovadis.DAL.Models;
using Ecovadis.DL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecovadis.API.Services
{
    public interface IGameService
    {
        MatchDto Create();
        MatchDtoDetail Score(int matchId,TableSide side);
        MatchDtoDetail Detail(int matchId);
        IEnumerable<MatchDto> List();
        void Remove(int matchId);
    }
}
