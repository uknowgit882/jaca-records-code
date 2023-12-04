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
    private readonly string BaseURL = "https://api.discogs.com/releases/";
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
        RestRequest request = new RestRequest(BaseURL + release_id);

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
}

