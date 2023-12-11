import axios from "axios";

export default{
    GetAllCollections(){
        return axios.get(`Collections`);
    },
    AddNewCollection(collection){
        return axios.post(`Collections`, collection);
    },
    GetNamedCollection(name){
        return axios.get(`Collections/${name}`);
    },
    DeleteNamedCollection(name){
        return axios.delete(`Collections/${name}`);
    },
    ChangeNameForNamedCollection(name, newInfo){
        return axios.put(`Collections/${name}/name`, newInfo);
    },
    ChangePrivacyForNamedCollection(name, newInfo){
        return axios.put(`Collections/${name}/privacy`, newInfo);
    },
    GetRecordInCollection(name, discogs_id){
        return axios.get(`Collections/${name}/record/${discogs_id}`);
    },
    AddRecordToCollection(name, discogs_id){
        return axios.post(`Collections/${name}/record`, discogs_id);
    },
    DeleteRecordInCollection(name, discogs_id){
        return axios.post(`Collections/${name}/record/${discogs_id}`);
    }
}