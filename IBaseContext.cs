
public interface IBaseContext
{
    IRepository<T> ResolveRepository<T>()
        where T : User;
}