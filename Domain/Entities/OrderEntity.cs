﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Identity;
using Domain.Entities.Delivery;

namespace Domain.Entities;

[Table("tblOrders")]
public class OrderEntity : BaseEntity<long>
{
    [ForeignKey(nameof(OrderStatus))]
    public long OrderStatusId { get; set; }
    [ForeignKey(nameof(User))]   
    public long UserId { get; set; }
    public OrderStatusEntity? OrderStatus { get; set; }
    public UserEntity? User { get; set; }
    
    public ICollection<OrderItemEntity>? OrderItems { get; set; }
    public DeliveryInfoEntity? DeliveryInfo {get; set; }
}
