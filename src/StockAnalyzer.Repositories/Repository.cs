using StockAnalyzer.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalyzer.Repositories
{
    
        public abstract class Repository<T> : IRepository<T> where T : class
    {
        private MyDBContext dataContext;
        private readonly DbSet<T> dbset;

        protected Repository(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            dbset = DataContext.Set<T>();
        }

        protected IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }

        protected MyDBContext DataContext
        {
            get { return dataContext ?? (dataContext = DatabaseFactory.Get()); }
        }


        public virtual void Add(T entity)
        {
            dbset.Add(entity);
        }
        public virtual void Update(T entity)
        {
            dbset.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
        }
        public virtual void Delete(T entity)
        {
            dbset.Remove(entity);
        }
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbset.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                dbset.Remove(obj);
        }
        public virtual T GetById(long id)
        {
            return dbset.Find(id);
        }
        public virtual T GetById(int id)
        {
            return dbset.Find(id);
        }
        public virtual T GetById(string id)
        {
            return dbset.Find(id);
        }
        public virtual IEnumerable<T> GetAll()
        {
            return dbset.ToList();
        }
        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbset.Where(where).ToList();
        }
        public T Get(Expression<Func<T, bool>> where)
        {
            return dbset.Where(where).FirstOrDefault<T>();
        }

        protected DataTable ExecuteSqlQuery(string sqlQuery, CommandType commandType, SqlParameter[] parameters = null)
        {
            if (commandType == CommandType.Text)
            {
                return SqlQuery(sqlQuery, parameters);
            }
            else if (commandType == CommandType.StoredProcedure)
            {
                return StoredProcedure(sqlQuery, parameters);
            }

            return null;
        }
        protected void ExecuteNonQuery(string commandText, CommandType commandType, SqlParameter[] parameters = null)
        {
            if (DataContext.Database.Connection.State == ConnectionState.Closed)
            {
                DataContext.Database.Connection.Open();
            }

            var command = DataContext.Database.Connection.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = commandType;

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }

            command.ExecuteNonQuery();
        }
        protected ICollection ExecuteReader(string commandText, CommandType commandType, SqlParameter[] parameters = null)
        {
            if (DataContext.Database.Connection.State == ConnectionState.Closed)
            {
                DataContext.Database.Connection.Open();
            }

            var command = DataContext.Database.Connection.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = commandType;

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }

            using (var reader = command.ExecuteReader())
            {
                var mapper = new DataReaderMapper();
                //return mapper.MapToList(reader);
                return new List<string>();
            }
        }

        private DataTable SqlQuery(string sqlQuery, SqlParameter[] parameters = null)
        {

            var dataTable = new DataTable();
            if (DataContext.Database.Connection.State == ConnectionState.Closed)
            {
                DataContext.Database.Connection.Open();
            }

            var command = DataContext.Database.Connection.CreateCommand();
            command.CommandText = sqlQuery;
            command.CommandType = CommandType.Text;

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }

            using (var reader = command.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            return dataTable;
        }

        private DataTable StoredProcedure(string storedProcedureName, SqlParameter[] parameters = null)
        {
            var dataTable = new DataTable();
            if (DataContext.Database.Connection.State == ConnectionState.Closed)
            {
                DataContext.Database.Connection.Open();
            }

            var command = DataContext.Database.Connection.CreateCommand();
            command.CommandText = storedProcedureName;
            command.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }

            using (var reader = command.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            return dataTable;
        }
    }
}
