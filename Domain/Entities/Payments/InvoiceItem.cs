using System;
using Domain.Common;
using Domain.Entities.Reservations;

namespace Domain.Entities.Payments;

public sealed class InvoiceItem : BaseEntity<int>
{
    public int InvoiceId { get; private set; }
    public int InvoiceItemTypeId { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Subtotal { get; private set; }
    public int? ReservationPassengerId { get; private set; }

    public Invoice Invoice { get; set; } = null!;
    public InvoiceItemType InvoiceItemType { get; set; } = null!;
    public ReservationPassenger? ReservationPassenger { get; set; }

    private InvoiceItem() { }

    public InvoiceItem(
        int invoiceId, 
        int invoiceItemTypeId, 
        string description, 
        int quantity, 
        decimal unitPrice, 
        decimal subtotal, 
        int? reservationPassengerId)
    {
        ValidateAndSet(invoiceId, invoiceItemTypeId, description, quantity, unitPrice, subtotal, reservationPassengerId);
    }

    public void Update(
        int invoiceId, 
        int invoiceItemTypeId, 
        string description, 
        int quantity, 
        decimal unitPrice, 
        decimal subtotal, 
        int? reservationPassengerId)
    {
        ValidateAndSet(invoiceId, invoiceItemTypeId, description, quantity, unitPrice, subtotal, reservationPassengerId);
        UpdatedAt = DateTime.UtcNow;
    }

    private void ValidateAndSet(
        int invoiceId, 
        int invoiceItemTypeId, 
        string description, 
        int quantity, 
        decimal unitPrice, 
        decimal subtotal, 
        int? reservationPassengerId)
    {
        if (invoiceId <= 0) throw new ArgumentException("InvoiceId must be greater than 0.");
        if (invoiceItemTypeId <= 0) throw new ArgumentException("InvoiceItemTypeId must be greater than 0.");
        
        if (string.IsNullOrWhiteSpace(description)) 
            throw new ArgumentException("Description cannot be null or empty.");
        if (description.Length > 200) 
            throw new ArgumentException("Description cannot exceed 200 characters.");
            
        if (quantity < 1) throw new ArgumentException("Quantity must be at least 1.");
        if (unitPrice < 0) throw new ArgumentException("UnitPrice cannot be negative.");
        if (subtotal < 0) throw new ArgumentException("Subtotal cannot be negative.");

        if (subtotal != quantity * unitPrice)
            throw new ArgumentException("Subtotal must be exactly Quantity * UnitPrice.");

        if (reservationPassengerId.HasValue && reservationPassengerId.Value <= 0)
            throw new ArgumentException("ReservationPassengerId must be greater than 0 if provided.");

        InvoiceId = invoiceId;
        InvoiceItemTypeId = invoiceItemTypeId;
        Description = description.Trim();
        Quantity = quantity;
        UnitPrice = unitPrice;
        Subtotal = subtotal;
        ReservationPassengerId = reservationPassengerId;
    }
}
