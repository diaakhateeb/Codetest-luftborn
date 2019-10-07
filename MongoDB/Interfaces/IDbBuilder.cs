namespace MongoDB.Interfaces
{
    public interface IDbBuilder
    {
        string GetDatabaseName();

        string GetConnectionString();
    }
}
