using System;
using System.Collections.Generic;

namespace LoanlyBot.Models;

public partial class Loaner
{
    public long UserId { get; set; }

    public string Name { get; set; } = null!;

    public decimal? MyLoan { get; set; }

    public decimal? HisLoan { get; set; }

    public virtual ICollection<Payment> PaymentRecipients { get; set; } = new List<Payment>();

    public virtual ICollection<Payment> PaymentSenders { get; set; } = new List<Payment>();
}
