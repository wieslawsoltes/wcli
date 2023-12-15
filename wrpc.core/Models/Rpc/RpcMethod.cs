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
using System.Text.Json.Serialization;
using WasabiRpc.Models.Rpc.Methods;

namespace WasabiRpc.Models.Rpc;

[JsonDerivedType(typeof(BroadcastRpcMethod), "broadcast")]
[JsonDerivedType(typeof(BuildRpcMethod), "build")]
[JsonDerivedType(typeof(CancelTransactionRpcMethod), "canceltransaction")]
[JsonDerivedType(typeof(CreateWalletRpcMethod), "createwallet")]
[JsonDerivedType(typeof(ExcludeFromCoinJoinRpcMethod), "excludefromcoinjoin")]
[JsonDerivedType(typeof(GetFeeRatesRpcMethod), "getfeerates")]
[JsonDerivedType(typeof(GetHistoryRpcMethod), "gethistory")]
[JsonDerivedType(typeof(GetNewAddressRpcMethod), "getnewaddress")]
[JsonDerivedType(typeof(GetStatusRpcMethod), "getstatus")]
[JsonDerivedType(typeof(GetWalletInfoRpcMethod), "getwalletinfo")]
[JsonDerivedType(typeof(ListCoinsRpcMethod), "listcoins")]
[JsonDerivedType(typeof(ListKeysRpcMethod), "listkeys")]
[JsonDerivedType(typeof(ListUnspentCoinsRpcMethod), "listunspentcoins")]
[JsonDerivedType(typeof(ListWalletsRpcMethod), "listwallets")]
[JsonDerivedType(typeof(LoadWalletRpcMethod), "loadwallet")]
[JsonDerivedType(typeof(PayInCoinjoinRpcMethod), "payincoinjoin")]
[JsonDerivedType(typeof(RecoverWalletRpcMethod), "recoverwallet")]
[JsonDerivedType(typeof(SendRpcMethod), "send")]
[JsonDerivedType(typeof(SpeedUpTransactionRpcMethod), "speeduptransaction")]
[JsonDerivedType(typeof(StartCoinJoinSweepRpcMethod), "startcoinjoinsweep")]
[JsonDerivedType(typeof(StartCoinJoinRpcMethod), "startcoinjoin")]
[JsonDerivedType(typeof(StopCoinJoinRpcMethod), "stopcoinjoin")]
[JsonDerivedType(typeof(StopRpcMethod), "stop")]
public abstract class RpcMethod : Rpc
{
    [JsonPropertyName("method")]
    [JsonRequired]
    public string? Method { get; set; }
}
