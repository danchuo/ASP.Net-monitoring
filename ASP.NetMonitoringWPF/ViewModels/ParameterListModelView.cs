using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ASP.NetMonitoringWPF.Commands;
using ASP.NetMonitoringWPF.Models;

namespace ASP.NetMonitoringWPF.ViewModels;

public class ParameterListModelView : ViewModelBase {

    private readonly ParameterList _parameterList;
    public ReadOnlyObservableCollection<WmiProperty> AllParametersList => _parameterList.AllParametersList;

    public ICommand AddDeletePropertyCommand { get; }
    public ICommand AddAllPropertiesCommand { get; }
    public ICommand DeleteAllPropertiesCommand { get; }

    public ParameterListModelView(ParameterList parameterList) {
        _parameterList = parameterList;

        

        AddDeletePropertyCommand = new AddDeletePropertyCommand(parameterList.DataCenter);
        AddAllPropertiesCommand = new AddAllPropertiesCommand(parameterList.DataCenter);
        DeleteAllPropertiesCommand = new DeleteAllPropertiesCommand(parameterList.DataCenter);
    }

}