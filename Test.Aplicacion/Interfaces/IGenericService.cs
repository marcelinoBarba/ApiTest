using Test.Dominio.Interfaces;

namespace Test.Aplicacion.Interfaces
{
    public interface IGenericService<T> where T : class, IEntity
    {
        T? GetById(int id);
        List<T> GetAll();
        bool Add(T entity);
        bool Update(T entity);
        bool Delete(int id);
    }
}
