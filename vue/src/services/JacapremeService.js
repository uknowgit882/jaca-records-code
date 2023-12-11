import axios from "axios";

export default {
    reactiveUser(username){
        return axios.put(`Jacapreme/reactivate/${username}`);
    },
    upgradeUserToAdmin(username){
        return axios.put(`Jacapreme/upgradetoadmin/${username}`);
    },
    downgradeUserFromAdmin(username){
        return axios.put(`Jacapreme/downgradefromadmin/${username}`);
    }
}