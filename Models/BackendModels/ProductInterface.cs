using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.BackendModels;

public class ProductInterface
{
    public string? Id { get; set; }
    public string Name { get; set; }
    public double Workmanship { get; set; }
    public List<(double, string)> Materials { get; set; }
    public double MaterialsPrice { get; set; }
    public double TimeTaken { get; set; }
    public string Type { get; set; }
    public bool IsUsedInAi { get; set; }
}
