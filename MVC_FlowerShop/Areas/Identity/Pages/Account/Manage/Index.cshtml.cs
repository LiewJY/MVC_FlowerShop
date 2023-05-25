// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MVC_FlowerShop.Areas.Identity.Data;

namespace MVC_FlowerShop.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<MVC_FlowerShopUser> _userManager;
        private readonly SignInManager<MVC_FlowerShopUser> _signInManager;

        public IndexModel(
            UserManager<MVC_FlowerShopUser> userManager,
            SignInManager<MVC_FlowerShopUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "Please enter your name first before register!")]
            [Display(Name = "Customer Full Name")]
            [StringLength(100, ErrorMessage = "Only 5 to 100 chars allowed in this column!", MinimumLength = 5)]
            public string CustomerName { get; set; }



            [Display(Name = "Customer Age")]
            [Range(12, 100, ErrorMessage = "This website only allowwed children that at least 12 years old!")]
            public int CustomerAge { get; set; }



            [Display(Name = "Customer Address")]
            public string CustomerAddress { get; set; }



            [Display(Name = "Customer Birth Date")]
            [DataType(DataType.Date)]
            public DateTime CustomerDate { get; set; }
        }

        private async Task LoadAsync(MVC_FlowerShopUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

          Input = new InputModel
          {
              // left related to input form, right side related to database table
              PhoneNumber = phoneNumber,
              CustomerName = user.CustomerName,
              CustomerAge = user.CustomerAge,
              CustomerDate = user.CustomerDOB,
              CustomerAddress = user.CustomerAddresss,
          };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync() //form submission funciton
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }


            if (Input.CustomerDate != user.CustomerDOB)
            {
                user.CustomerDOB = Input.CustomerDate;
            }



            if (Input.CustomerAge != user.CustomerAge)
            {
                user.CustomerAge = Input.CustomerAge;
            }



            if (Input.CustomerAddress != user.CustomerAddresss)
            {
                user.CustomerAddresss = Input.CustomerAddress;
            }



            // to update the database using the new user info
            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
