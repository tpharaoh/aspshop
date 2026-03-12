using Microsoft.AspNetCore.Mvc.RazorPages;

namespace aspshop.Pages.Checkout;

public class ConfirmationModel : PageModel
{
    public int OrderId { get; set; }

    public void OnGet(int orderId)
    {
        OrderId = orderId;
    }
}
