using Microservices.Web.Exceptions;
using Microservices.Web.Services.Abstract;
using Microservices.Web.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Web.Handler
{
    //Uygulamalara özel token taleplerini yerine getirecek olan sınıfımız.
    //Mikroservislerimizin bazıları üyelik bilgisinden ziyade uygulamaların kimlik bilgisine ihtiyaç duyarak çalışıyor (Catalog ve Photo gibi)
    public class ClientCredentialTokenHandler : DelegatingHandler //HTTP İsteklerinde araya girmemizi sağlayacak
    {
        //Client Credential Token dönecek servisimiz
        private readonly IClientCredentialTokenService _clientCredentialTokenService;

        public ClientCredentialTokenHandler(IClientCredentialTokenService clientCredentialTokenService)
        {
            _clientCredentialTokenService = clientCredentialTokenService;
        }

        //Her istek başlatıldığında araya girecek
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var clientToken = await _clientCredentialTokenService.GetToken();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", clientToken);

            var response = await base.SendAsync(request, cancellationToken);

            //401 ya da 403 aldıysak eğer
            if (response.StatusCode==System.Net.HttpStatusCode.Unauthorized || response.StatusCode==System.Net.HttpStatusCode.Forbidden)
            {
                throw new UnAuthorizeException();
            }
            return response;
        }

    }
}
