using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataBus.Proc
{
    public interface IExecutable {
        bool ReturnsRecords { get; }
        Expression Definition { get; } 
        Type ReturnType { get; } 
    }

    public interface IExecutable<in P, out E> : IExecutable where E:new()
    {

        int Execute(P parameters);
        E ExecuteScalar(P parameters);
        IEnumerable<E> ExecuteQuery(P parameters); 
    }
}