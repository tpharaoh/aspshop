using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using aspshop.Models;

namespace aspshop.Pages.Account;

[Authorize]
public class ProfileModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfileModel(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();
    public string? Email { get; set; }
    public string? SuccessMessage { get; set; }

    public class InputModel
    {
        [Required] public string FirstName { get; set; } = string.Empty;
        [Required] public string LastName { get; set; } = string.Empty;
    }

    public async Task OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            Email = user.Email;
            Input.FirstName = user.FirstName;
            Input.LastName = user.LastName;
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToPage("/Account/Login");

        user.FirstName = Input.FirstName;
        user.LastName = Input.LastName;
        await _userManager.UpdateAsync(user);

        SuccessMessage = "Profile updated successfully.";
        Email = user.Email;
        return Page();
    }
}
