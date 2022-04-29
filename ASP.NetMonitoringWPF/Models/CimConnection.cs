using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Management.Infrastructure;

namespace ASP.NetMonitoringWPF.Models;

public class CimConnection {

    private readonly CimSession _cimSession;

    public string ComputerName => _cimSession.ComputerName;

    public CimConnection() {
        _cimSession = CimSession.Create("localhost");
    }

    public CimConnection(string computerName) {
        _cimSession = CimSession.Create(computerName);
    }

    // try / catch ?
    public IEnumerable<CimInstance> MakeQuery(string queryExpression) =>
        _cimSession.QueryInstances(@"root\cimv2", "WQL", queryExpression);

    private static readonly Random Rnd = new();

    public double GetPropertyValue(string propertyName, string queryExpression) {
        return //Rnd.Next(100) +
               MakeQuery(queryExpression).FirstOrDefault()!.CimInstanceProperties
                   .Where(x => x.Name == propertyName)
                   .Select(x => int.Parse(x.Value.ToString() ?? "0")).FirstOrDefault();
    }

}