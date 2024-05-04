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
    public class EmpiricMessage : ValueChangedMessage<EmpiricCoefficientMathModel>
    {
        public EmpiricMessage(EmpiricCoefficientMathModel value) : base(value)
        {

        }
    }
}
