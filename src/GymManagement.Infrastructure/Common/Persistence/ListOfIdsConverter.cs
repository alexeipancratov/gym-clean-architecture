using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GymManagement.Infrastructure.Common.Persistence;

public class ListOfIdsConverter(ConverterMappingHints? mappingHints = null) : ValueConverter<List<Guid>, string>(
    v => string.Join(',', v),
    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList(),
    mappingHints);