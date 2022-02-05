using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Prism.Commands;
using RevitAPITrainingLibrary_6_2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITraining_6_2
{
    public class MainViewViewModel
    {

        private ExternalCommandData _commandData;
        public List<Level> Levels { get; } = new List<Level>();
        public List<XYZ> Points { get; } = new List<XYZ>();
        public DelegateCommand SaveCommand { get; }
        public List<FamilySymbol> FamilyTypes { get; }
        public FamilySymbol SelectedFurniture { get; set; }
        public Level SelectedLevel { get; set; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;

            Points = SelectionsUtils.GetPoints(_commandData, "Выберите точки", Autodesk.Revit.UI.Selection.ObjectSnapTypes.Points);
            
            FamilyTypes = FamilySymbolUtils.GetFamilySymbols(_commandData);
            
            Levels = LevelsUtils.GetLevels(commandData);

            SaveCommand = new DelegateCommand(OnSaveCommand);
        }
        private void OnSaveCommand()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            if (Points.Count <1)
            {
                return;
            }
            var olevel = SelectedLevel;
            var opoint = Points[0];

            FamilyInstanceUtils.CreateFamilyInstance(_commandData, SelectedFurniture, opoint, olevel);

            RaiseCloseRequest();
        }

        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }

}