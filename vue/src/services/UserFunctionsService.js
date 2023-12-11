import axios from "axios";

export default {
    deactivateUser(){
        return axios.put(`UserFunctions/deactivate`);
    },
    upgradeUser(){
        return axios.put(`UserFunctions/upgrade`);
    },
    downgradeUser(){
        return axios.put(`UserFunctions/downgrade`);
    }
}