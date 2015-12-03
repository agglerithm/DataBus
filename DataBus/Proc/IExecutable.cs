using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DataBus.Data;

namespace DataBus.Proc
{
    public interface IExecutable {
        bool ReturnsRecords { get; }
        Expression Definition { get; } 
        Type ReturnType { get; } 
    }

    public interface IExecutable<in P, out E> : IExecutable where E:new()
    {

        int Execute(P parameters, IDataContext session);
        E ExecuteScalar(P parameters, IDataContext session);
        IEnumerable<E> ExecuteQuery(P parameters, IDataContext session); 
    }
}