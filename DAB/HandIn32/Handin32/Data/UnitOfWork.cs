using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Handin32.Data
{
	public class UnitOfWork<T> where T : Entity
	{
		private readonly AddressModel _context;
		public IRepository<T> Repository { get; }


		public UnitOfWork(AddressModel context)
		{
			_context = context;
			Repository = new Repository<T>(context);
		}

		

		public int Commit()
		{
			return _context.SaveChanges();
		}

	}
}