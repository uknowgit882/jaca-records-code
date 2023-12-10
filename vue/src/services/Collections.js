import axios from "axios";

export default{
    getInfoForCard(){
        return axios.get('/Collections/')
    }
}