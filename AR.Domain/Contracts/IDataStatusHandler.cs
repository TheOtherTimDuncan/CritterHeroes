using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AR.Domain.Models.Status;

namespace AR.Domain.Contracts
{
    public interface IDataStatusHandler
    {
        Task<DataStatusModel> GetModelStatusAsync(params IStorageSource[] storageSources);
        Task<DataStatusModel> SyncModelAsync(IStorageSource source, IStorageSource target);
    }
}
