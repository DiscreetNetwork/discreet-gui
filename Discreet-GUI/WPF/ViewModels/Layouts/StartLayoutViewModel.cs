using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WPF.Attributes;
using WPF.ViewModels.Common;

namespace WPF.ViewModels.Layouts
{
    [AssemblyScanIgnore("This is a LayoutViewModel and is used by other ViewModels, but never alone and thus it shouldnt be registered")]
    public class StartLayoutViewModel : ViewModelBase
    {
        public ViewModelBase ContentViewModel { get; set; }


        private int _carouselIndex = 0;
        public int CarouselIndex { get => _carouselIndex; set { _carouselIndex = value; OnPropertyChanged(nameof(CarouselIndex)); } }

        public StartLayoutViewModel(ViewModelBase contentViewModel)
        {
            ContentViewModel = contentViewModel;

            //_ = InitializeCarousel();
        }


        async Task InitializeCarousel()
        {
            while(true)
            {
                await Task.Delay(2000);

                if(CarouselIndex == 2)
                {
                    CarouselIndex = 0;
                    continue;
                }

                CarouselIndex++;
            }
        }
    }
}
