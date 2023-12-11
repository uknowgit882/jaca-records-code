import axios from "axios";

export default {
  searchDiscogs(Search) {
    return axios.get(`Search/search?q=${Search.General}&artist=${Search.Artist}&title=${Search.Title}&genre=${Search.Genre}&year=${Search.Year}&country=${Search.Country}&label=${Search.Label}&barcode=${Search.Barcode}`)
  },
  searchLibrary(Search) {
    return axios.get(`Search/searchLibrary?q=${Search.General}&artist=${Search.Artist}&title=${Search.Title}&genre=${Search.Genre}&year=${Search.Year}&country=${Search.Country}&label=${Search.Label}&barcode=${Search.Barcode}`)
  },
  searchCollections(Search) {
    return axios.get(`Search/searchCollections?q=${Search.General}&artist=${Search.Artist}&title=${Search.Title}&genre=${Search.Genre}&year=${Search.Year}&country=${Search.Country}&label=${Search.Label}&barcode=${Search.Barcode}`)
  }
}
