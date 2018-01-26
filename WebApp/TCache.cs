using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp
{
    public class TCache<T>
    {
        public T Get(string cacheKeyName, int cacheTimeOutSeconds, 
            Func<T> func)
        {
            return new TCacheInternal<T>().Get(
                cacheKeyName,cacheTimeOutSeconds,func);
        }

        //public T Get(string cacheKeyName, int cacheTimeOutSeconds,
        //    Func<T> func)
        //{
        //    return new TCacheInternal<T>().Get(
        //        cacheKeyName, cacheTimeOutSeconds, func);
        //}
    }
}