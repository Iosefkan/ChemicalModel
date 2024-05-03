using ChemModel.Data.DbTables;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemModel.ViewModels.ViewModelsData
{
    public class DataExcel
    {
        public double Length { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double Velocity { get; set; }
        public double TempCr {  get; set; }
        public double Step {  get; set; }
        public Material Material { get; set; }
        public MathModel MathModel { get; set; }
        public List<MathModelEmpiricBind> Coefs { get; set; }
        public List<MaterialPropertyBind> Properties { get; set; }
        public List<MaterialMathModelPropertyBind> MatMathProps { get; set; }

    }
}
