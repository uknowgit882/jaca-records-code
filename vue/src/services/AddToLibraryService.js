import axios from "axios";

export default {
    addToDB(id){
       return axios.get(`Test/AddRecordToDb/${id}`) 
    },
    addToLibrary(id){
        return axios.post('Library/', {
            "discogsId": id,
            "notes": "please work",
            "quantity": 1
        })
    },
    displayRecordsInLibrary(){
        return axios.get('Library/')
    }
}