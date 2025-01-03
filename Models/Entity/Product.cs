﻿using System.ComponentModel.DataAnnotations;

public class Product
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public float Price { get; set; }
    public string Description { get; set; }
    public int Stock { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }
}
