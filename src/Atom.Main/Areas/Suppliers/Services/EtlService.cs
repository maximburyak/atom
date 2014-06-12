using System.Collections.Generic;
using Atom.Areas.Suppliers.Data.Query;
using Atom.Areas.Suppliers.Domain.Models;
using BeValued.Data.NHibernate;
using NHibernate;

namespace Atom.Main.Areas.Suppliers.Services
{
	public class EtlService
	{
		private readonly ISession _session;

		private readonly NHibernateRepository<EtlFilesProcessLogEntry> _supplierRepo;

		public EtlService(ISession session)
		{
			_session = session;
			_supplierRepo = new NHibernateRepository<EtlFilesProcessLogEntry>(_session);
		}

		public IList<EtlFilesProcessLogEntry> ViewModel()
		{
			var model = new ListEtlFilesProcessLogEntries().GetQuery(_session).List<EtlFilesProcessLogEntry>();
			return model;
		}

		public void Disable(int id)
		{
			var etlprocess = _supplierRepo.Get(id);
			if (!etlprocess.Enabled)
				return;

			etlprocess.Enabled = false;
			_supplierRepo.Save(etlprocess);

		}

		public void Enable(int id)
		{
			var etlprocess = _supplierRepo.Get(id);
			if (etlprocess.Enabled)
				return;

			etlprocess.Enabled = true;
			_supplierRepo.Save(etlprocess);
		}
	}
}