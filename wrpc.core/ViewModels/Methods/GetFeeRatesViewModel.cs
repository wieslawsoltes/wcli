﻿using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using WasabiRpc.ViewModels.Factories;
using WasabiRpc.Models.App;
using WasabiRpc.Models.BatchMode;
using WasabiRpc.Models.Info;
using WasabiRpc.Models.Results;
using WasabiRpc.Models.Services;

namespace WasabiRpc.ViewModels.Methods;

public partial class GetFeeRatesViewModel : RoutableMethodViewModel
{
    public GetFeeRatesViewModel(IRpcServiceViewModel rpcService, INavigationService navigationService, IBatchManager batchManager)
        : base(rpcService, navigationService, batchManager)
    {
    }

    [RelayCommand]
    private async Task GetFeeRates()
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
        var result = await RpcService.Send<RpcGetFeeRatesResult>(job.RpcMethod, job.RpcServerUri, NavigationService);
        if (result is RpcGetFeeRatesResult { Result: not null } rpcGetFeeRatesResult)
        {
            OnRpcSuccess(rpcGetFeeRatesResult);
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
        if (rpcResult is RpcGetFeeRatesResult rpcGetFeeRatesResult)
        {
            var getFeeRatesInfoViewModel = new GetFeeRatesInfo { FeeRates = rpcGetFeeRatesResult.Result }.ToViewModel(RpcService, NavigationService);
            NavigationService.NavigateTo(getFeeRatesInfoViewModel);
        }
    }

    public override Job CreateJob()
    {
        var requestBody = new RpcMethod
        {
            Method = "getfeerates"
        };

        var rpcServerUri = $"{RpcService.ServerPrefix}";

        return new Job("getfeerates", requestBody, rpcServerUri, typeof(RpcGetFeeRatesResult));
    }
}
