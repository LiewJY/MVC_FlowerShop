using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MVC_FlowerShop.Areas.Identity.Data;

// Add profile data for application users by adding properties to the MVC_FlowerShopUser class
public class MVC_FlowerShopUser : IdentityUser
{
    [PersonalData]
    public string CustomerName { get; set; }

    [PersonalData]
    public string CustomerAddresss { get; set; }

    [PersonalData]
    public int CustomerAge { get; set; }

    [PersonalData]
    public DateTime CustomerDOB { get; set; }

}

