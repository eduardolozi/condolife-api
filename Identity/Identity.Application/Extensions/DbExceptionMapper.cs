using Identity.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Identity.Application.Extensions;

public static class DbExceptionMapper
{
    private const string UniqueViolation = "23505";

    public static bool TryMap(Exception exception, out Exception mapped)
    {
        mapped = null!;

        if (exception is not DbUpdateException dbEx)
            return false;

        if (dbEx.InnerException is not PostgresException pgEx)
            return false;

        return TryMapPostgres(pgEx, out mapped);
    }

    private static bool TryMapPostgres(PostgresException pgEx, out Exception mapped)
    {
        mapped = null!;

        switch (pgEx.SqlState)
        {
            case UniqueViolation:
                return TryMapUniqueViolation(pgEx, out mapped);

            default:
                return false;
        }
    }

    private static bool TryMapUniqueViolation(PostgresException pgEx, out Exception mapped)
    {
        mapped = pgEx.ConstraintName switch
        {
            "UX_users_email" => new ConflictException("Email já cadastrado."),
            "UX_users_external_id" => new ConflictException("ExternalId já existente. Contate o administrador do sistema."),

            // fallback
            _ => new ConflictException("Violação de unicidade. Contate o administrador.")
        };

        return true;
    }
}
