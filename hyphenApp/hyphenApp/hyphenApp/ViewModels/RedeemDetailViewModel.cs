using System;

using hyphenApp.Models;

namespace hyphenApp.ViewModels
{
    public class RedeemDetailViewModel : BaseViewModel
    {
        public Redemption Item { get; set; }
        public RedeemDetailViewModel(Redemption item = null)
        {
            Title = item?.Name;
            Item = item;
        }
    }
}
