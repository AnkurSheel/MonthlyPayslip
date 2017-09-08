using System.Collections.Generic;

namespace MonthlyPayslip.DataLayer.Interfaces
{
    public interface IRepository<T>
        where T : new()
    {
        void Add(T entity);

        IEnumerable<T> FindAll();
    }
}
