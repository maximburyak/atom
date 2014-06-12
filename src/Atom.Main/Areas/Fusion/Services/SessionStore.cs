using System;
using System.Collections.Generic;
using System.Web;

namespace Atom.Main.Areas.Fusion.Services
{
    public class SessionStore : IObjectStore
    {
        public void Delete(string key)
        {
            HttpContext.Current.Session.Remove(key);
        }

        public T Get<T>(string key)
        {
            return (T)HttpContext.Current.Session[key];
        }

        public void Store<T>(string key, T value)
        {
            HttpContext.Current.Session[key] = value;
        }

        public IList<T> GetList<T>(string key)
        {
            throw new NotImplementedException();
        }
    }
}