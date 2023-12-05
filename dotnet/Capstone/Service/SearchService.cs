using Capstone.Models;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Authenticators.OAuth;
using System.Net.Http;

namespace Capstone.Service
{
    public class SearchService : ISearchService
    {
        protected readonly string BaseURL = "https://api.discogs.com/database/search";

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

        public SearchResult SearchForRecord(SearchRequest searchObject)
        {
            client.Authenticator = oAuth1;
            SearchResult searchedRecord = new SearchResult();
            if (searchObject.Query != null)
            {
                searchObject.Query = searchObject.Query.Replace(' ', '+');
            }
            else
            {
                searchObject.Query = "";
            }

            if (searchObject.Artist != null)
            {
                searchObject.Artist = searchObject.Artist.Replace(' ', '+');
            }
            else
            {
                searchObject.Artist = "";
            }

            if (searchObject.Title != null)
            {
                searchObject.Title = searchObject.Title.Replace(' ', '+');
            }
            else
            {
                searchObject.Title = "";
            }

            if (searchObject.Genre != null)
            {
                searchObject.Genre = searchObject.Genre.Replace(' ', '+');
            }
            else
            {
                searchObject.Genre = "";
            }

            if (searchObject.Year != null)
            {
                searchObject.Year = searchObject.Year.Replace(' ', '+');
            }
            else
            {
                searchObject.Year = "";
            }

            if (searchObject.Country != null)
            {
                searchObject.Country = searchObject.Country.Replace(' ', '+');
            }
            else
            {
                searchObject.Country = "";
            }

            if (searchObject.Label != null)
            {
                searchObject.Label = searchObject.Label.Replace(' ', '+');
            }
            else
            {
                searchObject.Label = "";
            }


            string searchParameterString =
                $"q={searchObject.Query}&artist={searchObject.Artist}&release_title={searchObject.Title}&genre={searchObject.Genre}&year={searchObject.Year}&country={searchObject.Country}&label={searchObject.Label}";

            RestRequest request = new RestRequest(BaseURL + "?" + searchParameterString);

            IRestResponse<SearchResult> response = client.Get<SearchResult>(request);
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new HttpRequestException("Error occured - unable to reach server,", response.ErrorException);
            }
            else if (!response.IsSuccessful)
            {
                throw new HttpRequestException("Error occured - received non-success response.", response.ErrorException);
            }

            return response.Data;
        }



    }
}
