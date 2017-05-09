using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoseShoutFormsPrism.Services;

namespace WhoseShoutFormsPrism.ViewModels
{
    public class MainViewModel
    {
        public ServiceApi ServiceApi { get; private set; }

        public MainViewModel()
        {
            ServiceApi = new ServiceApi();
        }
    }
}
