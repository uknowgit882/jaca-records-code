import axios from "axios";

export default{
    GetLibrary(){
        return axios.get(`Library`);
    },
    AddToLibrary(record){
        return axios.post(`Library`, record);
    },
    GetRecordInLibrary(id){
        return axios.get(`Library/${id}`);
    },
    DeleteRecordInLibrary(id){
        return axios.delete(`Library/${id}`);
    },
    ChangeNoteForRecordInLibrary(record){
        return axios.put(`Library/${record.discogs_Id}/notes`, record);
    },
    ChangeQuantityForRecordInLibrary(record){
        return axios.put(`Library/${record.discogs_Id}/quantity`, record);
    }
}