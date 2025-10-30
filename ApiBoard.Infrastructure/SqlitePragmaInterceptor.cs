using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ApiBoard.Infrastructure;

public sealed class SqlitePragmaInterceptor : DbConnectionInterceptor
{
    private static readonly string[] Pragmas =
    {
        "PRAGMA journal_mode=WAL;",
        "PRAGMA foreign_keys=ON;"
    };

    public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
    {
        base.ConnectionOpened(connection, eventData);
        ApplyPragmas(connection);
    }

    public override async Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData, CancellationToken cancellationToken = default)
    {
        await base.ConnectionOpenedAsync(connection, eventData, cancellationToken).ConfigureAwait(false);
        await ApplyPragmasAsync(connection, cancellationToken).ConfigureAwait(false);
    }

    private static void ApplyPragmas(DbConnection connection)
    {
        if (connection is not SqliteConnection sqliteConnection)
        {
            return;
        }

        using var command = sqliteConnection.CreateCommand();
        foreach (var pragma in Pragmas)
        {
            command.CommandText = pragma;
            command.ExecuteNonQuery();
        }
    }

    private static async Task ApplyPragmasAsync(DbConnection connection, CancellationToken cancellationToken)
    {
        if (connection is not SqliteConnection sqliteConnection)
        {
            return;
        }

        foreach (var pragma in Pragmas)
        {
            await using var command = sqliteConnection.CreateCommand();
            command.CommandText = pragma;
            await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}

