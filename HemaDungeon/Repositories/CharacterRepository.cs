using HemaDungeon.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace HemaDungeon.Repositories;

public sealed class CharacterRepository(Context context)
{
    public async Task<ICollection<Character>> GetAllCharacters(string region)
    {
        var characters = context.Users.Include(x => x.Region).Include(x => x.Visits).Include(x => x.Cataclysms).Include(x => x.Pages).Where(x => x.Region!.Id == region);
        return await characters.ToListAsync(); 
    }
}