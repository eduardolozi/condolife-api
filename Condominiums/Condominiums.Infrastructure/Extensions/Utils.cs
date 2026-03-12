namespace Condominiums.Infrastructure.Extensions;

public static class Utils
{
    public static string ReadSqlSeed(string sqlName)
    {
        var assembly = typeof(CondominiumDbContext).Assembly;

        var manifestResourceName = $"Condominiums.Infrastructure.Seeds.{sqlName}";
        using var stream = assembly.GetManifestResourceStream(manifestResourceName);

        using var reader = new StreamReader(stream!);
        var sql = reader.ReadToEnd();
        
        return sql;
    }
}