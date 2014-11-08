using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Models.Status;

namespace CH.Domain.Contracts
{
    public interface IDataStatusHandler
    {
        Task<DataStatusModel> GetModelStatusAsync(params IStorageSource[] storageSources);
        Task<DataStatusModel> SyncModelAsync(IStorageSource source, IStorageSource target);
    }
}
