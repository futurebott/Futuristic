using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Futuristic.Models;
using Xamarin.Forms;

namespace Futuristic.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        public ItemDetailViewModel(Item item = null)
        {
            Title = item?.Text;
            Item = item;
        }

    }
}
