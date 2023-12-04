using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Nur.Gateway.Web.Dto;
using Ocelot.Middleware;
using Ocelot.Multiplexer;
namespace Nur.Gateway.Web.Aggregator
{
	public class UsersPostsAggregator : IDefinedAggregator
	{
		public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
		{
			if (responses.Any(r => r.Items.Errors().Count > 0))
			{
				return new DownstreamResponse(null, HttpStatusCode.BadRequest, (List<Header>)null, null);
			}

			var userResponseContent = await
			 responses[0].Items.DownstreamResponse().Content.ReadAsStringAsync();

			var postResponseContent = await
			 responses[1].Items.DownstreamResponse().Content.ReadAsStringAsync();

			var users = JsonConvert.DeserializeObject<List<User>>(userResponseContent);
			var posts = JsonConvert.DeserializeObject<List<Post>>(postResponseContent);

			foreach (var user in users)
			{
				//var userPosts =new List<Post_>( posts.Where(p => p.UserId == user.Id).ToList());
				var userPosts = posts.Where(p => p.UserId == user.Id).ToList();
				user.Posts.AddRange(userPosts);
			}

			var postByUserString = JsonConvert.SerializeObject(users);

			var stringContent = new StringContent(postByUserString)
			{
				Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
			};

			return new DownstreamResponse(stringContent, HttpStatusCode.OK, new List<KeyValuePair<string, IEnumerable<string>>>(), "OK");
		}
	}
}
