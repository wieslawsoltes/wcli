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
using System;
using System.Text.Json;
using System.Threading.Tasks;
using WasabiRpc.ViewModels.Factories;
using WasabiRpc.Models;
using WasabiRpc.Models.App;
using WasabiRpc.Models.BatchMode;
using WasabiRpc.Models.Services;
using WasabiRpc.ViewModels.App;
using WasabiRpc.ViewModels.BatchMode;
using WasabiRpc.ViewModels.Info;

namespace WasabiRpc.ViewModels;

public abstract partial class RoutableMethodViewModel : RoutableViewModel
{
    protected RoutableMethodViewModel(
        IRpcServiceViewModel rpcService, 
        INavigationService navigationService,
        INavigationService detailsNavigationService, 
        IBatchManager batchManager)
        : base(rpcService, navigationService, detailsNavigationService)
    {
        BatchManager = batchManager;
    }

    protected IBatchManager BatchManager { get; }

    protected virtual async Task RunCommand()
    {
        var job = CreateJob();

        if (RpcService.BatchMode)
        {
            OnBatch(job);
            return;
        }

        var routable = await Execute(job);
        if (routable is not null)
        {
            if (routable is ErrorInfoViewModel errorInfoViewModel)
            {
                OnRpcError(errorInfoViewModel);
            }
            else if (routable is ErrorViewModel errorViewModel)
            {
                OnError(errorViewModel);
            }
            else
            {
                OnRpcSuccess(routable);
            }
        }
    }

    protected virtual void OnRpcSuccess(IRoutable routable)
    {
        DetailsNavigationService.Clear();
        NavigationService.ClearAndNavigateTo(routable);
    }

    protected virtual void OnRpcError(IRoutable routable)
    {
        DetailsNavigationService.Clear();
        NavigationService.NavigateTo(routable);
    }

    protected virtual void OnError(IRoutable routable)
    {
        DetailsNavigationService.Clear();
        NavigationService.NavigateTo(routable);
    }

    protected virtual void OnBatch(Job job)
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(job, typeof(Job), new ModelsJsonContext(options));
            var addJobViewModel = new AddJobViewModel(RpcService, NavigationService, DetailsNavigationService, BatchManager, job, json);
            DetailsNavigationService.Clear();
            NavigationService.ClearAndNavigateTo(addJobViewModel);
        }
        catch (Exception e)
        {
            var errorViewModel = new Error { Message = e.Message }.ToViewModel(RpcService, NavigationService, DetailsNavigationService);
            DetailsNavigationService.Clear();
            NavigationService.NavigateTo(errorViewModel);
        }
    }

    public abstract IRoutable? ToJobResult(object? result);

    public abstract Job CreateJob();

    public abstract Task<IRoutable?> Execute(Job job);
}
