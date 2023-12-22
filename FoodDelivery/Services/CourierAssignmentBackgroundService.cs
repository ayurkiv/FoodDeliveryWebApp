using FoodDelivery.Repositories;

namespace FoodDelivery.Services
{
    //Служба CourierAssignmentBackgroundService - це IHostedService,
    //призначена для роботи у фоновому режимі в додатку ASP.NET Core.
    //Вона використовує IServiceScopeFactory для створення нової
    //області видимості для кожного виконання.
    //Це дозволяє уникнути проблеми ін'єкції
    //масштабованого сервісу (IOrderRepository) в синглтонний сервіс,
    //забезпечуючи правильну ін'єкцію залежностей.
    //Сервіс періодично перевіряє наявність замовлень
    //без призначеного кур'єра, використовуючи просту
    //затримку для демонстрації, і намагається призначити
    //кур'єра для кожного відповідного замовлення в межах
    //виділеної області видимості. Такий дизайн запобігає
    //конфліктам між одиночним та обмеженим часом життя
    //сервісу в контексті додатку ASP.NET Core.

    public class CourierAssignmentBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public CourierAssignmentBackgroundService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var orderRepository = scope.ServiceProvider.GetRequiredService<OrderRepository>();

                    // Your logic to periodically check for orders and assign couriers
                    // For simplicity, let's assume you check every 5 seconds
                    await Task.Delay(TimeSpan.FromSeconds(5));

                    // Get a list of orders that need a courier
                    var ordersToAssign = orderRepository.GetOrdersWithoutCourier();

                    foreach (var order in ordersToAssign)
                    {
                        // Attempt to assign a courier
                        await orderRepository.AssignCourierToOrderAsync(order.Id);
                    }
                }
            }
        }
    }
}
