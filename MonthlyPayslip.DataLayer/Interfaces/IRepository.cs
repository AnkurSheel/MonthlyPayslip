using System;
using System.Collections.Generic;

namespace MonthlyPayslip.DataLayer.Interfaces
{
    public interface IRepository<T>
        where T : new()
    {
        IEnumerable<T> FindAll();
    }
}