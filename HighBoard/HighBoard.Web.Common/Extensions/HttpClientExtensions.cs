
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;

namespace HighBoard.Web.Common.Extensions;

public static class HttpClientExtensions
{
    public static void AddCustomHttpClients(this IServiceCollection services)
    {
        services.AddScoped<JwtHandler>(); // JwtHandler'ı DI'ya ekle

        services.AddHttpClient("BaseHttpClient")
            .AddHttpMessageHandler<JwtHandler>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri("https://fetvaapitest.diyanet.gov.tr/api/");
            });

        services.AddHttpClient("AnotherHttpClient")
            .AddHttpMessageHandler<JwtHandler>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri("https://another-api.example.com/");
            });
    }
   
    //public static void AddCustomHttpClientV1(this IServiceCollection services)
    //{
    //    services.AddHttpClient("FatwaBaseHttpClient")
    //        .ConfigureHttpClient(client =>
    //        {
    //            client.BaseAddress = new Uri("https://fetvaapitest.diyanet.gov.tr/api/");
    //        });

    //    services.AddScoped(sp =>
    //    {
    //        var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    //        var authExtensions = sp.GetRequiredService<IAuthExtensions>();
    //        var httpClient = httpClientFactory.CreateClient("FatwaBaseHttpClient");

    //        // JWT token'ı ekle
    //        var jwtToken = authExtensions.GenerateJwt(new User { Id = Guid.NewGuid(), UserName = "default.user" });
    //        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

    //        return httpClient;
    //    });
    //}

    // JwtHandler sınıfı
    public class JwtHandler : DelegatingHandler
    {
        private readonly IAuthExtensions _authExtensions;

        public JwtHandler(IAuthExtensions authExtensions)
        {
            _authExtensions = authExtensions;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var jwtToken = _authExtensions.GenerateJwt(new User { Id = Guid.NewGuid(), UserName = "default.user" });
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}