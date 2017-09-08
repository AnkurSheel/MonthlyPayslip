using System;
using System.Collections.Generic;
using System.IO;
using MonthlyPayslip.DataLayer.Interfaces;

namespace MonthlyPayslip.DataLayer.Repositories
{
    public abstract class BaseCsvRepository<T> : IRepository<T>
        where T : new()
    {
        private readonly string _inputFileName;

        protected BaseCsvRepository(string inputFileName)
        {
            _inputFileName = inputFileName;
        }

        public void Add(T entity)
        {
            if (string.IsNullOrEmpty(_inputFileName))
            {
                throw new ArgumentException("No Filename specified");
            }

            var fileStream = new FileStream(_inputFileName, FileMode.Append);
            using (var streamWriter = new StreamWriter(fileStream))
            {
                streamWriter.WriteLine(ToCsv(entity));
            }
        }

        public IEnumerable<T> FindAll()
        {
            var entities = new List<T>();
            if (string.IsNullOrEmpty(_inputFileName))
            {
                return entities;
            }

            var fileStream = new FileStream(_inputFileName, FileMode.Open);
            using (var streamReader = new StreamReader(fileStream))
            {
                string line = streamReader.ReadLine();
                while (line != null)
                {
                    var data = line.Split(',');
                    entities.Add(Map(data));
                    line = streamReader.ReadLine();
                }
            }

            return entities;
        }

        protected abstract T Map(string[] data);

        protected abstract string ToCsv(T entity);
    }
}
