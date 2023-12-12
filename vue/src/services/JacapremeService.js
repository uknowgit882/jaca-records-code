import axios from "axios";

export default {
    reactivateUser(username){
        return axios.put(`Jacapreme/reactivate/${username}`);
    },
    upgradeUserToAdmin(username){
        return axios.put(`Jacapreme/upgradetoadmin/${username}`);
    },
    downgradeUserFromAdmin(username){
        return axios.put(`Jacapreme/downgradefromadmin/${username}`);
    }
}