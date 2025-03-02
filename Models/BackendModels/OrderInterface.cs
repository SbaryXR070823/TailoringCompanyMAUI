using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.BackendModels;

public class OrderInterface
{
    public string? Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string? ProductId { get; set; }
    public string? ImageReference { get; set; }
    public string? FinalImage { get; set; }
    public string Status { get; set; }
    public string PickupTime { get; set; }
    public string UserEmail { get; set; }
    public string? FinishedOrderTime { get; set; }
}
