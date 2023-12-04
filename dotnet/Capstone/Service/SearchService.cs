
using Microsoft.AspNetCore.Diagnostics;
using RestSharp;
using Capstone.Models;
using RestSharp.Authenticators.OAuth;
using RestSharp.Authenticators;

namespace Capstone.Service
{
    public class SearchService
    {
        protected readonly string BaseURL = "https://api.discogs.com/database/search?";
        
        private readonly OAuth1Authenticator oAuth1 = OAuth1Authenticator.ForAccessToken(
        consumerKey: "wZrDHJlTdpkgyYiwrGVM",
        consumerSecret: "dbpFfprjUhyGYcGNzxwRFPDMmPQCynTg",
        token: "VMEflxvRfgFRGpIxvlkzghtvmjzUMUqZzzeAaOLZ",
        tokenSecret: "LdfHmiuMYrhgDiXtSkJuBVAqiNjtVPKxvPpOdUfe",
        OAuthSignatureMethod.PlainText);


        protected static RestClient client = null;

        public SearchService()
        {
            if (client == null)
            {
                client = new RestClient();
            }
        }

        public SearchRequest GetSearch(string searchRequest)
        {

            // client.Authenticator = oAuth1;
            SearchRequest search = new SearchRequest();
            //string searchQueries = searchRequest.Substring(searchRequest.LastIndexOf("search?"));
            //string[] queriesSplit = searchQueries.Split('&');
            //string[] queries;
            //foreach(string query in queriesSplit)
            //{

            //}
            return search;
        }



    }
}
