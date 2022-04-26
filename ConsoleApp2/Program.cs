using System;
using System.Linq;
using System.Threading;
using ConsoleApp2.Models;
using Microsoft.Management.Infrastructure;

class Program {

    private static void main2() {
        var connection = new CimConnection("localhost");
        var model = new MonitoringModel(connection);
        model.Init();
        foreach (var data in model.DataChangesList) {
            Console.WriteLine(data.PropertyName + " " + data.Data.Sum());
        }

        Console.WriteLine();
        ObservedDataChanges.StartUpdating();
        Thread.Sleep(10000);

        foreach (var data in model.DataChangesList) {
            Console.WriteLine(data.PropertyName + " " + data.Data.Sum());
        }

        Console.WriteLine();
        ObservedDataChanges.StopUpdating();
        Thread.Sleep(10000);

        foreach (var data in model.DataChangesList) {
            Console.WriteLine(data.PropertyName + " " + data.Data.Sum());
        }
    }


    static void Main(string[] args) {
        //main2();

        var connection = new CimConnection("localhost");

        // var obj = new ObservedDataChanges(connection,
        //     "Select PercentProcessorTime From Win32_PerfFormattedData_PerfOS_Processor Where Name = \"_Total\"");
        //
        // //obj.StartUpdating();
        //
        // Thread.Sleep(5000);
        // for (int i = 0; i < 85; i++) {
        //     for (int j = 0; j < obj.Data.Count; j++) {
        //         Console.Write(ObservedDataChanges.Times[j] + " ");
        //     }
        //
        //     Console.WriteLine();
        //     Thread.Sleep(1100);
        // }

        while (true) {
            // Select PercentProcessorTime From Win32_PerfFormattedData_PerfOS_Processor Where Name = "_Total" // CPU load

            // Get-WMIObject -List| Where{$_.name -match "^Win32_.*_ASP.*"} | Sort Name | Format-Table name
            
            // Select * from Win32_PerfRawData_ASPNET4030319_ASPNETAppsv4030319
            // Select ManagedMemoryUsedestimated from Win32_PerfRawData_ASPNET4030319_ASPNETAppsv4030319

            // Select * from Win32_PerfFormattedData_ASPNET_ASPNET
            // Select ApplicationsRunning from Win32_PerfFormattedData_ASPNET_ASPNET
            // Select StateServerSessionsActive from Win32_PerfFormattedData_ASPNET_ASPNET
            // Select WorkerProcessesRunning from Win32_PerfFormattedData_ASPNET_ASPNET
            
            // Select ErrorEventsRaised from Win32_PerfFormattedData_ASPNET_ASPNET // errors ??
            // Select RequestsQueued from Win32_PerfFormattedData_ASPNET_ASPNET // requests queued ??
            // Select RequestsCurrent from Win32_PerfFormattedData_ASPNET_ASPNET // requests current ??

            // Select RequestsPerSec from Win32_PerfFormattedData_ASP_ActiveServerPages // requests per sec ??
            // Select RequestsTotal from Win32_PerfFormattedData_ASP_ActiveServerPages // requests total ??
            // Select ErrorsPerSec from Win32_PerfFormattedData_ASP_ActiveServerPages // errors per sec ??
            // Select RequestsQueued from Win32_PerfFormattedData_ASP_ActiveServerPages // requests queued ??


            // Select PercentManagedProcessorTimeestimated from Win32_PerfFormattedData_ASPNET_ASPNETApplications // cpu load? ??


            // загрузка CPU, запросов/сек, ошибки, длина очереди, etc
            Console.Write("Enter WQL (x = Quit): ");
            var query = Console.ReadLine().ToUpper();
            if (string.Compare(query, "X", StringComparison.Ordinal) == 0) break;


            // var dic = getDictionary(query);
            // foreach (var VARIABLE in dic) {
            //     Console.WriteLine(VARIABLE.Key + "   " + VARIABLE.Value);
            // }

            var enumerable = connection.MakeQuery(query);
            
            foreach (var cimInstance in enumerable) {
                PrintCimInstance(cimInstance);
                // Console.WriteLine(GetPropertyValue(cimInstance, "PercentProcessorTime"));
            }
        }
    }


    private static Dictionary<string, string> getDictionary(string query) {
        var enumerable = new CimConnection().MakeQuery(query);

        return (from cimInstance in enumerable
                from enumeratedProperty in cimInstance.CimInstanceProperties
                where enumeratedProperty.Value != null
                select enumeratedProperty)
            .ToDictionary(enumeratedProperty => enumeratedProperty.Name, enumeratedProperty => query);
    }


    private static void PrintCimInstance(CimInstance cimInstance) {
        Console.WriteLine("{0} properties", cimInstance.CimSystemProperties.ClassName);

        Console.WriteLine($"{"Key?",-5}{"Property",-30}{"Type",-15}{"Value",-10}");

        foreach (var enumeratedProperty in cimInstance.CimInstanceProperties) {
            var isKey = (enumeratedProperty.Flags & CimFlags.Key) == CimFlags.Key;

            if (enumeratedProperty.Value != null) {
                Console.WriteLine(
                    "{0,-5}{1,-30}{2,-15}{3,-10}",
                    isKey ? "Y" : string.Empty,
                    enumeratedProperty.Name,
                    enumeratedProperty.CimType,
                    enumeratedProperty.Value);
            }
        }

        Console.WriteLine();
    }

    private static int GetPropertyValue(CimInstance cimInstance, string propertyName) {
        return cimInstance.CimInstanceProperties.Where(x => x.Name == propertyName)
            .Select(x => int.Parse(x.Value.ToString() ?? string.Empty)).FirstOrDefault();
    }

}