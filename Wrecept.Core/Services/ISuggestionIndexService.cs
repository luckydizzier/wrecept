using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wrecept.Core.Services;

public interface ISuggestionIndexService
{
    Task AddHistoryEntryAsync(string term);
    Task<IReadOnlyList<string>> GetPredictionsAsync(string prefix);
}
