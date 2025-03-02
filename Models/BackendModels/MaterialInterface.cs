using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.BackendModels;

public class MaterialInterface
{
    public string? Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string? Picture { get; set; }
    public string Unit { get; set; }
    public DateTime Updated { get; set; }
    public bool IsUsedInAi { get; set; }
}
