using Microsoft.EntityFrameworkCore.Metadata;

namespace FoodDelivery.Models.Interfaces
{
    public interface ISoftDeletable : IModel
    {
        bool IsDelete { get; set; }
    }
}