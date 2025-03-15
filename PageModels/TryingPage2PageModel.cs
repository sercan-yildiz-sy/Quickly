using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Quicky.Models;

namespace Quicky.PageModels
{
    [QueryProperty(nameof(Inventory), "Inventory")]
    public partial class TryingPage2PageModel : ObservableObject, IBaseClass
    {
        [ObservableProperty]
        public bool _isBusy;

        public TryingPage2PageModel()
        {
        }

        [ObservableProperty]
        public Inventory inventory;


    }
}
