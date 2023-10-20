using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WasabiRpc.Models.App;
using WasabiRpc.Models.Results;
using WasabiRpc.Models.Services;
using WasabiRpc.ViewModels.Factories;
using WasabiRpc.ViewModels.Info;

namespace WasabiRpc.ViewModels.Methods;

public partial class CoinViewModel : RoutableViewModel
{
    [ObservableProperty] private string _walletName;
    [ObservableProperty] private bool _isSelected;

    public CoinViewModel(IRpcServiceViewModel rpcService, INavigationService navigationService, string walletName, CoinInfoViewModel coinInfo)
        : base(rpcService, navigationService)
    {
        WalletName = walletName;
        CoinInfo = coinInfo;
        IsSelected = false;
    }

    public CoinInfoViewModel CoinInfo { get; }

    [RelayCommand]
    private async Task ExcludeFromCoinJoin()
    {
        await Exclude(true);
    }

    [RelayCommand]
    private async Task RemoveExclusionFromCoinJoin()
    {
        await Exclude(false);
    }

    private async Task Exclude(bool exclude)
    {
        var excludeFromCoinJoinViewModel = new ExcludeFromCoinJoinViewModel(RpcService, NavigationService, WalletName)
        {
            TransactionId = CoinInfo.TxId,
            N = 0,
            Exclude = exclude
        };
        var job = excludeFromCoinJoinViewModel.CreateJob();
        var result = await RpcService.Send<RpcExcludeFromCoinJoinResult>(job, NavigationService);
        if (result is RpcExcludeFromCoinJoinResult)
        {
            NavigationService.NavigateTo(new Success
            {
                Message = $"{(excludeFromCoinJoinViewModel.Exclude ? "Excluded" : "Removed the exclusion")} from coinjoin"
            }.ToViewModel(RpcService, NavigationService));
        }
        else if (result is RpcErrorResult { Error: not null } rpcErrorResult)
        {
            NavigationService.NavigateTo(rpcErrorResult.Error.ToViewModel(RpcService, NavigationService));
        }
        else if (result is Error error)
        {
            NavigationService.NavigateTo(error.ToViewModel(RpcService, NavigationService));
        }
    }
}
