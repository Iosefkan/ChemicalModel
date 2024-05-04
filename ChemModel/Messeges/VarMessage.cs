using ChemModel.Data.DbTables;
using ChemModel.ViewModels;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemModel.Messeges
{
    public class VarMessage : ValueChangedMessage<VarCoefficientMathModel>
    {
        public VarMessage(VarCoefficientMathModel value) : base(value)
        {

        }
    }
}
