using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CH.Domain.Handlers.DataStatus;
using CH.Domain.Models.Status;

namespace CH.Domain.Contracts
{
    public interface IDataStatusHandler
    {
        Task<DataStatusModel> GetModelStatusAsync(StatusContext statusContext, IStorageSource source, IStorageSource target);
        Task<DataStatusModel> SyncModelAsync(StatusContext statusContext, IStorageSource source, IStorageSource target);
    }
}
