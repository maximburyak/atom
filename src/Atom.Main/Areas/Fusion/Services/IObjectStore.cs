using System.Collections.Generic;

namespace Atom.Main.Areas.Fusion.Services
{
    public interface IObjectStore
    {
        void Delete(string key);
        T Get<T>(string key);
        void Store<T>(string key, T value);
        IList<T> GetList<T>(string key);
    }
}