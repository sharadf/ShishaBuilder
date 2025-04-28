using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShishaBuilder.Core.Models;

public class OrderTobacco
{
    public int Id { get; set; }
    public  int OrderId  { get; set; }
    public required int TobaccoId  { get; set; }

    public required int Percentage { get; set; }

    [ForeignKey("OrderId")]
    public Order Order { get; set; }
    
    [ForeignKey("TobaccoId")]
    public Tobacco Tobacco { get; set;}
}
