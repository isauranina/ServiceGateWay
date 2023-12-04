using Newtonsoft.Json;
using Ocelot.Middleware;
using Ocelot.Multiplexer;
using Nur.Gateway.Web.Dto;
using System.Net;

namespace Nur.Gateway.Web.Aggregator
{
     public class MenuItemDetailAggregator : IDefinedAggregator
     {
	   public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
	   {

		 if (responses.Count == 0)
		 {
		      return new DownstreamResponse(new HttpResponseMessage(HttpStatusCode.NotFound));
		 }

		 var response = responses[0];
		 if (response.Items.Errors().Count > 0)
		 {
		      return new DownstreamResponse(new HttpResponseMessage(HttpStatusCode.NotFound));
		 }

		 var menuItemResponse = await
			 responses[0].Items.DownstreamResponse().Content.ReadAsStringAsync();

		 var objMenuItem = JsonConvert.DeserializeObject<MenuItem>(menuItemResponse);

		 if (objMenuItem.EsInventariable)
		 {
		      var inventarioResponse = await
			      responses[1].Items.DownstreamResponse().Content.ReadAsStringAsync();

		      var objInventario = JsonConvert.DeserializeObject<InventarioItem>(inventarioResponse);

		      objMenuItem.Stock = objInventario.Stock;
		 }


		 return new DownstreamResponse(new StringContent(JsonConvert.SerializeObject(objMenuItem)),
			 HttpStatusCode.OK, new List<Header>(), "OK");
	   }
     }
}
