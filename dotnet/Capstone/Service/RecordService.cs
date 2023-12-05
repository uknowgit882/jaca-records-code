namespace Capstone.Service;
using Capstone.Models;
using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Authenticators.OAuth;
using System.Net.Http;



public class RecordService : IRecordService
{
    // Taking in external URL from the API 
    // should I update this so it works with a dependency injection?
    //private readonly string BaseURL = "https://api.discogs.com/releases/";
    private readonly string BaseURL = "https://api.discogs.com/";
    private readonly OAuth1Authenticator oAuth1 = OAuth1Authenticator.ForAccessToken(
            consumerKey: "wZrDHJlTdpkgyYiwrGVM",
            consumerSecret: "dbpFfprjUhyGYcGNzxwRFPDMmPQCynTg",
            token: "VMEflxvRfgFRGpIxvlkzghtvmjzUMUqZzzeAaOLZ",
            tokenSecret: "LdfHmiuMYrhgDiXtSkJuBVAqiNjtVPKxvPpOdUfe",
            OAuthSignatureMethod.PlainText);

    private static RestClient client = null;

    public RecordService()
    {

        // checking if client is null 
        if (client == null)
        {
            client = new RestClient();
        }
    }
    public RecordClient GetRecord(int release_id)
    {
        client.Authenticator = oAuth1;
        RecordClient getRecord = new RecordClient();

        //api.discogs.com/releases/relase_id (249504) worked okay - returned 200 
        RestRequest request = new RestRequest(BaseURL + $"releases/{release_id}");

        IRestResponse<RecordClient> response = client.Get<RecordClient>(request);
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


    public SearchResult SearchForRecordsDiscogs(SearchRequest searchObject)
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

        if(searchObject.Barcode != null)
        {
            searchObject.Barcode = searchObject.Barcode.Replace(' ', '+');
        }
        else
        {
            searchObject.Barcode = "";
        }

        RestRequest request = new RestRequest(BaseURL + "/database/search");
        request.AddParameter("q", searchObject.Query);
        request.AddParameter("artist", searchObject.Artist);
        request.AddParameter("release_title", searchObject.Title);
        request.AddParameter("genre", searchObject.Genre);
        request.AddParameter("year", searchObject.Year);
        request.AddParameter("country", searchObject.Country);
        request.AddParameter("label", searchObject.Label);
        request.AddParameter("barcode", searchObject.Barcode);
        request.AddParameter("format", "vinyl");

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

    //public SearchResult SearchForRecordsInLibrary(SearchRequest searchObject)
    //{
    //    SearchResult searchedRecord = new SearchResult();



    //    return searchedRecord;
    //}
}

