﻿using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WasabiRpc.ViewModels.Factories;
using WasabiRpc.Models.App;
using WasabiRpc.Models.BatchMode;
using WasabiRpc.Models.Info;
using WasabiRpc.Models.Results;
using WasabiRpc.Models.Services;

namespace WasabiRpc.ViewModels.Methods;

public partial class ListUnspentCoinsViewModel : RoutableMethodViewModel
{
    [ObservableProperty] private string? _walletName;

    public ListUnspentCoinsViewModel(IRpcServiceViewModel rpcService, INavigationService navigationService, IBatchManager batchManager, string walletName)
        : base(rpcService, navigationService, batchManager)
    {
        WalletName = walletName;
    }

    [RelayCommand]
    private async Task ListUnspentCoins()
    {
        var job = CreateJob();

        if (RpcService.BatchMode)
        {
            OnBatch(job);
            return;
        }

        await Execute(job);
    }

    public override async Task Execute(Job job)
    {
        var result = await RpcService.Send<RpcListUnspentCoinsResult>(job.RpcMethod, job.RpcServerUri, NavigationService);
        if (result is RpcListUnspentCoinsResult { Result: not null } rpcListUnspentCoinsResult)
        {
            OnRpcSuccess(rpcListUnspentCoinsResult);
        }
        else if (result is RpcErrorResult { Error: not null } rpcErrorResult)
        {
            OnRpcError(rpcErrorResult);
        }
        else if (result is Error error)
        {
            OnError(error);
        }
    }

    protected override void OnRpcSuccess(Rpc rpcResult)
    {
        if (rpcResult is RpcListUnspentCoinsResult rpcListUnspentCoinsResult && WalletName is not null)
        {
            var listUnspentCoinsInfoViewModel = new ListUnspentCoinsInfo { Coins = rpcListUnspentCoinsResult.Result }.ToViewModelAdapter(RpcService, NavigationService, BatchManager, WalletName);
            NavigationService.NavigateTo(listUnspentCoinsInfoViewModel);
        }
    }

    public override Job CreateJob()
    {
        var requestBody = new RpcMethod
        {
            Method = "listunspentcoins"
        };

        var rpcServerUri = $"{RpcService.ServerPrefix}/{WalletName}";

        return new Job("listunspentcoins", requestBody, rpcServerUri, typeof(RpcListUnspentCoinsResult));
    }
}
