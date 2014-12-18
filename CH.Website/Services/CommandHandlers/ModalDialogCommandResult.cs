using System;
using System.Collections.Generic;
using CH.Domain.Services.Commands;
using CH.Website.Models.Modal;
using TOTD.Utility.ExceptionHelpers;

namespace CH.Website.Services.CommandHandlers
{
    public class ModalDialogCommandResult : BaseCommandResult<ModalDialogCommandResult>
    {
        public static ModalDialogCommandResult Success(ModalDialogModel model)
        {
            ThrowIf.Argument.IsNull(model, "model");

            ModalDialogCommandResult result = ModalDialogCommandResult.Success();
            result.ModalDialog = model;
            return result;
        }

        public ModalDialogModel ModalDialog
        {
            get;
            set;
        }
    }
}