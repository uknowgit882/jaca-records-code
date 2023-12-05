import axios from 'axios';

export default {

  login(user) {
    return axios.post('/login', user)
  },

  register(user) {
    return axios.post('/register', user)
  },
  search(Search){
    return axios.get(`Test/Search?q=${Search.General}&artist=${Search.Artist}&title=${Search.Title}&genre=${Search.Genre}&year=${Search.Year}&country=${Search.Country}&label=${Search.Label}&barcode=${Search.Barcode}`)
  },


}
