/*
 * wrpc A Graphical User Interface for using the Wasabi Wallet RPC.
 * Copyright (C) 2023  Wiesław Šoltés
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WasabiRpc.ViewModels.Factories;
using WasabiRpc.Models.App;
using WasabiRpc.Models.BatchMode;
using WasabiRpc.Models.Info;
using WasabiRpc.Models.Params.Build;
using WasabiRpc.Models.Params.Send;
using WasabiRpc.Models.Rpc;
using WasabiRpc.Models.Rpc.Methods;
using WasabiRpc.Models.Rpc.Results;
using WasabiRpc.Models.Services;
using WasabiRpc.ViewModels.App;
using WasabiRpc.ViewModels.Info;
using WasabiRpc.ViewModels.Methods.Adapters;

namespace WasabiRpc.ViewModels.Methods;

public partial class BuildViewModel : RoutableMethodViewModel
{
    [NotifyCanExecuteChangedFor(nameof(BuildCommand))]
    [NotifyCanExecuteChangedFor(nameof(CoinsSelectorCommand))]
    [ObservableProperty] 
    private string? _walletName;

    [NotifyCanExecuteChangedFor(nameof(BuildCommand))]
    [ObservableProperty]
    private string? _walletPassword;

    [NotifyCanExecuteChangedFor(nameof(BuildCommand))]
    [ObservableProperty]
    private string? _sendTo;

    [NotifyCanExecuteChangedFor(nameof(BuildCommand))]
    [ObservableProperty]
    private long _amount;

    [NotifyCanExecuteChangedFor(nameof(BuildCommand))]
    [ObservableProperty]
    private string? _label;

    [NotifyCanExecuteChangedFor(nameof(BuildCommand))]
    [ObservableProperty]
    private bool _subtractFee;

    [NotifyCanExecuteChangedFor(nameof(BuildCommand))]
    [ObservableProperty]
    private int? _feeTarget;

    [NotifyCanExecuteChangedFor(nameof(BuildCommand))]
    [ObservableProperty]
    private decimal? _feeRate;

    [NotifyCanExecuteChangedFor(nameof(CoinsSelectorCommand))]
    [ObservableProperty]
    private ObservableCollection<CoinAdapterViewModel> _coins;

    public BuildViewModel(
        IRpcServiceViewModel rpcService, 
        INavigationService navigationService,
        INavigationService detailsNavigationService, 
        IBatchManager batchManager, 
        string? walletName)
        : base(rpcService, navigationService, detailsNavigationService, batchManager)
    {
        WalletName = walletName;
        WalletPassword = "";
        SendTo = "";
        Amount = 0;
        Label = "Label";
        SubtractFee = false;
        FeeTarget = 2;
        FeeTarget = null;
        Coins = new ObservableCollection<CoinAdapterViewModel>();
    }

    private bool CanBuild()
    {
        return WalletName is not null 
               && WalletName.Length > 0
               && WalletPassword is not null
               && SendTo is not null 
               && SendTo.Length > 0
               && Amount > 0
               && Label is not null
               && Label.Length > 0
               && FeeTarget > 0;
    }

    [RelayCommand(CanExecute = nameof(CanBuild))]
    private async Task Build()
    {
        await RunCommand();
    }

    public override async Task<IRoutable?> Execute(Job job)
    {
        var result = await RpcService.Send<RpcBuildResult>(job.RpcMethod, job.RpcServerUri);
        return ToJobResult(result);
    }

    public override IRoutable? ToJobResult(object? result)
    {
        if (result is RpcBuildResult { Result: not null } rpcBuildResult)
        {
            return new BuildInfo { Tx = rpcBuildResult.Result }.ToViewModel(RpcService, NavigationService, DetailsNavigationService);
        }

        if (result is RpcErrorResult { Error: not null } rpcErrorResult)
        {
            return rpcErrorResult.Error?.ToViewModel(RpcService, NavigationService, DetailsNavigationService);
        }

        if (result is Error error)
        {
            return error.ToViewModel(RpcService, NavigationService, DetailsNavigationService);
        }

        return null;
    }

    public override Job CreateJob()
    {
        var requestBody = new BuildRpcMethod
        {
            Method = "build",
            Params = new Build
            {
                Payments = new List<Payment>
                {
                    new ()
                    {
                        SendTo = SendTo,
                        Amount = Amount,
                        Label = Label,
                        SubtractFee = SubtractFee
                    }
                },
                Coins = Coins
                    .Where(x => x.IsSelected)
                    .Select(x => new Coin { TransactionId = x.CoinInfo.TxId, Index = x.CoinInfo.Index })
                    .ToList(),
                FeeTarget = FeeTarget,
                FeeRate = FeeRate,
                Password = WalletPassword
            }
        };

        var rpcServerUri = $"{RpcService.ServerPrefix}/{WalletName}";
        
        return new Job("build", requestBody, rpcServerUri);
    }

    private bool CanCoinsSelector()
    {
        return WalletName is not null 
               && WalletName.Length > 0;
    }

    [RelayCommand(CanExecute = nameof(CanCoinsSelector))]
    private void CoinsSelector()
    {
        if (WalletName is null)
        {
            return;
        }

        var coinsSelectorViewModel = new UnspentCoinsSelectorViewModel(
            RpcService,
            NavigationService,
            DetailsNavigationService,
            BatchManager,
            WalletName,
            Coins);

        DetailsNavigationService.NavigateTo(coinsSelectorViewModel);
    }
}
