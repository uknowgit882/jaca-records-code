import axios from "axios";

export default{
    getPublicCollections(){
        return axios.get(`Anonymous/collections`);
    }
}