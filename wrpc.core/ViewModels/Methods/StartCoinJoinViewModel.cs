using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WasabiRpc.ViewModels.Factories;
using WasabiRpc.Models.App;
using WasabiRpc.Models.BatchMode;
using WasabiRpc.Models.Results;
using WasabiRpc.Models.Services;

namespace WasabiRpc.ViewModels.Methods;

public partial class StartCoinJoinViewModel : RoutableMethodViewModel
{
    [ObservableProperty] private string? _walletName;
    [ObservableProperty] private string? _walletPassword;
    [ObservableProperty] private bool _stopWhenAllMixed;
    [ObservableProperty] private bool _overridePlebStop;

    public StartCoinJoinViewModel(IRpcServiceViewModel rpcService, INavigationService navigationService, IBatchManager batchManager, string walletName)
        : base(rpcService, navigationService, batchManager)
    {
        WalletName = walletName;
        WalletPassword = "";
        StopWhenAllMixed = true;
        OverridePlebStop = true;
    }

    [RelayCommand]
    private async Task StartCoinJoin()
    {
        await RunCommand();
    }

    public override async Task<IRoutable?> Execute(Job job)
    {
        var result = await RpcService.Send<RpcStartCoinJoinResult>(job.RpcMethod, job.RpcServerUri, NavigationService);
        if (result is RpcStartCoinJoinResult rpcStartCoinJoinResult)
        {
            return new Success { Message = $"Started coinjoin for wallet {WalletName}" }.ToViewModel(RpcService, NavigationService);
        }

        if (result is RpcErrorResult { Error: not null } rpcErrorResult)
        {
            return rpcErrorResult.Error?.ToViewModel(RpcService, NavigationService);
        }

        if (result is Error error)
        {
            return error.ToViewModel(RpcService, NavigationService);
        }

        return null;
    }

    public override Job CreateJob()
    {
        var requestBody = new RpcMethod
        {
            Method = "startcoinjoin",
            Params = new []
            {
                WalletPassword,
                $"{StopWhenAllMixed}",
                $"{OverridePlebStop}"
            }
        };

        var rpcServerUri = $"{RpcService.ServerPrefix}/{WalletName}";

        return new Job("startcoinjoin", requestBody, rpcServerUri, typeof(RpcStartCoinJoinResult));
    }
}
