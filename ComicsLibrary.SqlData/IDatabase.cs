using System;
using System.Collections.Generic;
using System.Text;

namespace ComicsLibrary.SqlData
{
    public interface IDatabase
    {
        void Execute(string storedProcedure, object parameters = null);
        List<T> Query<T>(string storedProcedure, object parameters = null);
        T QuerySingle<T>(string storedProcedure, object parameters = null);
        void Populate<T>(T model, string storedProcedure, object parameters = null);
    }
}
