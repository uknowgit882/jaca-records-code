namespace Capstone.Service;
using Capstone.Models;
using RestSharp;
using System.Net.Http;



public class RecordService : IRecordService
{
    // Taking in external URL from the API 

    protected static RestClient client = null;

    public RecordService()
    {

        // checking if client is null 
        if (client == null)
        {
            client = new RestClient();
        }
    }
    public Record GetRecord(int release_id)
    {
        Record getRecord = new Record();

        //api.discogs.com/releases/relase_id (249504) worked okay - returned 200 
        RestRequest request = new RestRequest("discogs.com/releases/" + release_id);
        IRestResponse<Record> response = client.Get<Record>(request);
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

