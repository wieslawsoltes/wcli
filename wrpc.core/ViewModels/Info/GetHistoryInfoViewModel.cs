using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using WasabiRpc.Models.Services;
using WasabiRpc.ViewModels.Methods.Adapters;

namespace WasabiRpc.ViewModels.Info;

public partial class GetHistoryInfoViewModel : RoutableViewModel
{
    [ObservableProperty] 
    private List<TransactionAdapterViewModel>? _transactions;

    public GetHistoryInfoViewModel(IRpcServiceViewModel rpcService, INavigationService navigationService)
        : base(rpcService, navigationService)
    {
    }
}
