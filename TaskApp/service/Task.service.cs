using TaskService;
using TaskModels;
using Npgsql;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

public class TaskControllerService : ControllerBase, ITaskService
{
    private readonly string _connection;
    public TaskControllerService(IConfiguration configuration)
    {
        _connection = configuration.GetConnectionString("DefaultConnection");
        Console.WriteLine("Connected successfully");
    }

    public async Task<IEnumerable<TaskModel>> getAll()
    {
        var connect = new NpgsqlConnection(_connection);
        await connect.OpenAsync();

        var sql = """ SELECT * FROM task; """;

        using var command = new NpgsqlCommand(sql, connect);

        var _list = new List<TaskModel>();

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {

            int id = reader.GetInt32(reader.GetOrdinal("id"));
            string title = reader.GetString(reader.GetOrdinal("title"));
            string description = reader.GetString(reader.GetOrdinal("description"));

            _list.Add(new TaskModel { id = id, title = title, description = description });

        }

        return _list;

    }

    public async Task<TaskModel> getBy(int id)
    {
        var connect = new NpgsqlConnection(_connection);
        await connect.OpenAsync();

        var sql = """ SELECT * FROM task WHERE id=@id; """;

        using var command = new NpgsqlCommand(sql, connect);

        command.Parameters.AddWithValue("id", id);

        await using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var task = new TaskModel
            {
                id = reader.GetInt32(reader.GetOrdinal("id")),
                title = reader.GetString(reader.GetOrdinal("title")),
                description = reader.GetString(reader.GetOrdinal("description"))
            };

            return task;
        }

        return null;


    }
    public async Task<TaskModel> create(TaskCreate dto)
    {

        var connect = new NpgsqlConnection(_connection);
        await connect.OpenAsync();

        var sql = """ INSERT INTO task(title,description) VALUES(@title,@description); """;

        using var command = new NpgsqlCommand(sql, connect);

        command.Parameters.AddWithValue("title", dto.title);
        command.Parameters.AddWithValue("description", dto.description);

        var result = await command.ExecuteScalarAsync();

        int ids = Convert.ToInt32(result);

        return new TaskModel
        {
            id = ids,
            title = dto.title,
            description = dto.description
        };
    }
    public async Task<TaskModel> update(TaskUpdate dto)
    {

        var connect = new NpgsqlConnection(_connection);
        await connect.OpenAsync();

        var sql = """ UPDATE task SET title=@title,description=@description WHERE id=@id;""";

        using var command = new NpgsqlCommand(sql, connect);

        command.Parameters.AddWithValue("id", dto.id);
        command.Parameters.AddWithValue("title", dto.title);
        command.Parameters.AddWithValue("description", dto.description);

        await command.ExecuteNonQueryAsync();


        return new TaskModel { id = dto.id, title = dto.title, description = dto.description };
    }
    public async Task<bool> delete(int id)
    {

        var connect = new NpgsqlConnection(_connection);
        await connect.OpenAsync();

        var sql = """ DELETE FROM task WHERE id=@id; """;

        using var command = new NpgsqlCommand(sql, connect);

        command.Parameters.AddWithValue("id", id);

       int row =await  command.ExecuteNonQueryAsync();

        return row > 0;

    }
}