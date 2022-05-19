using System;
using System.Collections.Generic;

namespace HeijunkaAppLibrary.Databases
{
    public interface IHerokuDataAccess
    {
        void SaveData<T>(string sqlStatement,
                         T parameters,
                         string connectionStringName);

        List<T> LoadData<T, U>(string sqlStatement,
                               U parameters,
                               string connectionStringName);
    }
}
