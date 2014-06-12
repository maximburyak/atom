using System;

namespace Atom.Main.Areas.Fusion.Services
{
	public interface ICacheService
	{
		T Get<T>(string cacheid) where T : class;
		T Get<T>(string cacheid, Func<T> getItemCallback) where T : class;
		T Get<T>(string cacheid, Func<T> getItemCallback, int minutesToExpiry) where T : class;
		void Add(string cacheid, object item, int minutesToExpiry);
		void Remove(string cacheid);

	}
}