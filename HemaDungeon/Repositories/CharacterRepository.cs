using HemaDungeon.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace HemaDungeon.Repositories;

public sealed class CharacterRepository(Context context)
{
    public async Task<ICollection<Character>> GetAllCharacters()
    {
        var characters = context.Users.Include(x => x.Visits).Include(x => x.Cataclysms).Include(x => x.Pages);
        return await characters.ToListAsync(); 
    }
}