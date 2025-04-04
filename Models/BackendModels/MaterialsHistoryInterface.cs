﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.BackendModels;

public class MaterialsHistoryInterface
{
    public string? Id { get; set; }
    public string MaterialId { get; set; }
    public double Price { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsLatest { get; set; }
}
