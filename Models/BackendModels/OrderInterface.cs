using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.BackendModels;

public class OrderInterface
{
    public string? _id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string? Product_Id { get; set; }
    public string? Image_Reference { get; set; }
    public string? Final_Image { get; set; }
    public string Status { get; set; }
    public string Pickup_Time { get; set; }
    public string UserEmail { get; set; }
    public string? Finished_Order_Time { get; set; }
}
