using NHibernate;

namespace Atom.Areas.Fusion.Events
{
	public abstract class EventHandler
	{
		public ISession Session { get; set; }
		public string Host
		{
			get { return System.Configuration.ConfigurationManager.AppSettings["url"]; }
		}
	}
}
