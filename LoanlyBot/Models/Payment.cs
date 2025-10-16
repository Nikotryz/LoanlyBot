using System;
using System.Collections.Generic;

namespace LoanlyBot.Models;

public partial class Payment
{
    public long PaymentId { get; set; }

    public long SenderId { get; set; }

    public long RecipientId { get; set; }

    public DateTime Date { get; set; }

    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public virtual Loaner Recipient { get; set; } = null!;

    public virtual Loaner Sender { get; set; } = null!;
}
