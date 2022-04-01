public static class ServiceRegistration
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        // ASP.NET DI 容器提供三種選項：

        // Singleton
        // 整個 Process 只建立一個 Instance，任何時候都共用它。
        // Scoped
        // 在網頁 Request 處理過程(指接到瀏覽器請求到回傳結果前的執行期間)共用一個 Instance。
        // Transient
        // 每次要求元件時就建立一個新的，永不共用。

        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        //services.AddTransient<IProductRepository, ProductRepository>();
    }
}