using System;

namespace lab1_pws.ViewModels
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
                public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
