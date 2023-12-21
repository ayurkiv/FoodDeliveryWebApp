using FoodDelivery.Models.Common;
using FoodDelivery.Models.Interfaces;
using FoodDelivery.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Data
{
    public partial class ApplicationDbContext
    {
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplySoftDelete();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            ApplySoftDelete();
            return base.SaveChanges();
        }

        private void ApplySoftDelete()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is ISoftDeletable softDeletable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Deleted:
                            entry.State = EntityState.Modified;
                            softDeletable.IsDelete = true;
                            break;
                    }
                }
            }
        }
    }
}