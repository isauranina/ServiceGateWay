using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nur.Gateway.Web.Handlers
{
    public class RemoveEncodingHandler : DelegatingHandler
    {
	 protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken CancellationToken)
	 {
	     request.Headers.AcceptEncoding.Clear();
	     return await base.SendAsync(request, CancellationToken);
	 }
    }
}