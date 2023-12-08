import axios from "axios";

export default {
    addToLibrary(id){
       return axios.get(`Test/AddRecordToDb/${id}`) 
    }
}