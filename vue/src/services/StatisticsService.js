import axios from "axios";

export default {
    getAggregateStats(){
        return axios.get(`Statistics/aggregate`);
    },
    getUserStats(){
        return axios.get(`Statistics/user`);
    }
}