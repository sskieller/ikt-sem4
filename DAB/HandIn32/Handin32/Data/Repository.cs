using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Handin32.Data
{
	public interface IRepository<T> where T : Entity
	{
		void Create(T t);
		T Read(int id);
		DbSet<T> ReadAll();
		void Delete(T t);
	}
	public class Repository<T> : IRepository<T> where T : Entity
	{
		private AddressModel _model;
		public Repository(AddressModel model)
		{
			_model = model;
		}
		public void Create(T t)
		{
			_model.Set<T>().Add(t);
		}

		public T Read(int id)
		{
			return _model.Set<T>().Find(id);
		}

		public DbSet<T> ReadAll()
		{
			return _model.Set<T>();
		}
		

		public void Delete(T t)
		{
			_model.Set<T>().Remove(t);
		}
	}
}