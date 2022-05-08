﻿using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace NextGenSoftware.OASIS.API.Providers.EthereumOASIS.ContractDefinition
{
    [Function("DeleteAvatarDetail", "bool")]
    public class DeleteAvatarDetailFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "entityId", 1)]
        public virtual BigInteger EntityId { get; set; }
    }
}