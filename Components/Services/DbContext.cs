using System.Data.SQLite;
using Dapper;
using PainelKanbanDesenvolvimento.Components.Models.Enum;
using PainelKanbanDesenvolvimento.Components.Models.Kanban;

namespace PainelKanbanDesenvolvimento.Components.Services;

public static class DbContext
{
    private static readonly string DbPath = Path.Combine(AppContext.BaseDirectory, "Data/PainelKanbanDev.db");

    private static SQLiteConnection Connection()
    {
        var conn = new SQLiteConnection($"Data Source={DbPath};Version=3;");
        conn.Open();
        return conn;
    }
    public static List<T> GetAll<T>(string table)
    {
        using var conn = Connection();

        return conn.Query<T>($"SELECT * FROM {table}").ToList();
    }
    public static T? GetById<T>(string table, string id)
    {
        using var conn = Connection();
        return conn.QueryFirstOrDefault<T>($"SELECT * FROM {table} WHERE Id=@Id", new{ Id = id});
    }
    public static T? GetByIndex<T>(string table, int index)
    {
        using var conn = Connection();
        return conn.QueryFirstOrDefault<T>($"SELECT * FROM {table} WHERE Idx=@Idx", new{ Idx = index});
    }
    public static async Task InsertEtapaAsync(Etapa etapa)
    {
        using var conn = Connection();
        var sql = @"INSERT INTO Etapas (Id, IdProjeto, Nome, Descricao, ResponsavelIndex, Status, Prazo, DataCriacao, DataConclusao)
        VALUES (@Id, @IdProjeto, @Nome, @Descricao, @ResponsavelIndex, @Status, @Prazo, @DataCriacao, @DataConclusao)
        ";
        await conn.ExecuteAsync(sql, etapa);
    }
    public static async Task InsertCardAsync(Card etapa)
    {
        using var conn = Connection();
        var sql = @"INSERT INTO Cards (Id, CurrentStatus, Prioridade, Prazo, DataCriacao, Nome, Descricao, Observacao, IndexSetor, IndexResponsavel, DataComecoProjeto)
        VALUES (@Id, @CurrentStatus, @Prioridade, @Prazo, @DataCriacao, @Nome, @Descricao, @Observacao, @IndexSetor, @IndexResponsavel, @DataComecoProjeto)
        ";
        await conn.ExecuteAsync(sql, etapa);
    }
    public static async Task UpdateCardStatusAsync(Card card)
    {
        using var connection = Connection();

        var sql = @"
            UPDATE Cards
            SET
                Nome = @Nome,
                Descricao = @Descricao,
                CurrentStatus = @CurrentStatus,
                Prioridade = @Prioridade,
                Prazo = @Prazo,
                IndexResponsavel = @IndexResponsavel,
                IndexSetor = @IndexSetor,
                Observacao = @Observacao,
                DataComecoProjeto = @DataComecoProjeto,
                DataConclusao = @DataConclusao,
                DataArquivamento = @DataArquivamento,
                MotivoArquivamento = @MotivoArquivamento
            WHERE Id = @Id";

        await connection.ExecuteAsync(sql, card);
    }
    public static async Task UpdateEtapaAsync(Etapa etapa)
    {
        using var connection = Connection();

        var sql = @"
            UPDATE Etapas
            SET
                Nome = @Nome,
                Descricao = @Descricao,
                ResponsavelIndex = @ResponsavelIndex,
                Status = @Status,
                Prazo = @Prazo,
                DataCriacao = @DataCriacao,
                DataConclusao = @DataConclusao
            WHERE Id = @Id";

        await connection.ExecuteAsync(sql, etapa);
    }
    public static async Task DeleteCardByIdAsync(string id)
    {
        using var connection = Connection();

        var sql = @"DELETE FROM Cards WHERE Id = @Id";

        await connection.ExecuteAsync(sql, new { Id = id });
    }
    public static async Task DeleteEtapa(Etapa etapa)
    {
        using var connection = Connection();

        var sql = @"DELETE FROM Etapas WHERE Id = @Id";

        await connection.ExecuteAsync(sql, new { Id = etapa.Id });
    }
}