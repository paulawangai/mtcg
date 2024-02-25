using System;
using System.Threading.Tasks;
using mtcg.Models;

namespace mtcg.Services
{
    public interface IBattleService
    {
        Task<BattleResult> RequestBattle(Guid player1Id, Guid player2Id);
    }
}
