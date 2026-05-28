using System.Data.SQLite;
using Dapper;
using PainelKanbanDesenvolvimento.Components.Models;
using PainelKanbanDesenvolvimento.Components.Models.Kanban;

namespace PainelKanbanDesenvolvimento.Components.Services;

public static class DbContext
{
    private static readonly string DbPath =
        @"P:\Fiscal\Arquivos de Apoio\APLICATIVOS\X - Dados Compartilhados\PainelKanban\PainelKanbanDev.db";

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
}