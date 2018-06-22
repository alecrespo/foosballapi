using Ecovadis.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecovadis.DL.Repositories
{
    public static class MatchRepositoryExtension
    {
        public static IEnumerable<Match> GetMatchesDescending(this IRepository<Match> repository)
        {
            return repository.GetAll().Result;
        }

    }
}
