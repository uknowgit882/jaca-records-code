import { createStore as _createStore } from 'vuex';
import axios from 'axios';

export function createStore(currentToken, currentUser) {
  let store = _createStore({
    state: {
      token: currentToken || '',
      user: currentUser || {},
      searchResults: [
     
      ],
      recordsInDB: [

      ],

      records: [
        // { 
        //   id: 1,
        //   album_cover:"https://m.media-amazon.com/images/I/812H+OFXlCL._UF1000,1000_QL80_.jpg",
        //   title:"No String Attached",
        //   artist:"*Nsync",
        // },
        // {
        //   id: 2,
        //   album_cover:"https://upload.wikimedia.org/wikipedia/en/1/13/This_Is_Us_%28Backstreet_Boys_album_-_cover_art%29.jpg",
        //   title:"This is us",
        //   artist: "Backstreet Boys"
        // },
        // {
        //   id: 3,
        //   album_cover: "https://m.media-amazon.com/images/I/61nmDvBE7TL._UF1000,1000_QL80_.jpg",
        //   title: "Weezer(Red)",
        //   artist: "Weezer"
        // },
        // {
        //   id: 4,
        //   album_cover:"https://i.pinimg.com/originals/cb/6b/e3/cb6be339c675430b6f8eb4b824816396.jpg",
        //   title: "In Utero",
        //   artist: "Nirvana"
        // },
        // {
        //   id: 5,
        //   album_cover:"https://upload.wikimedia.org/wikipedia/en/d/df/RedHotChiliPeppersCalifornication.jpg",
        //   title: "Californication",
        //   artist: "Red Hot Chili Peppers"
        // },
        // {
        //   id: 6,
        //   album_cover: "https://poprescue.files.wordpress.com/2014/10/o-town-2001-o-town-album.jpg",
        //   title: "We fit together",
        //   artist:"O-town"
        // },
        // {
        //   id: 7,
        //   album_cover:"https://upload.wikimedia.org/wikipedia/en/b/b8/Limp_Bizkit_Significant_Other.jpg",
        //   title: "Significant Other",
        //   artist: "Limp Bizkit"
        // },
        // {
        //   id: 8,
        //   album_cover:"https://m.media-amazon.com/images/I/718c9L+Ti1L._SX522_.jpg",
        //   title: "Wave of Mutilation: Best of Pixies",
        //   artist: "Pixies"
        // },
        // {
        //   id: 9,
        //   album_cover:"https://upload.wikimedia.org/wikipedia/en/e/e0/Nine_Inch_Nails_-_With_Teeth.png",
        //   title: "With Teeth",
        //   artist: "Nine Inch Nails"
        // },
        // {
        //   id: 10,
        //   album_cover:"https://upload.wikimedia.org/wikipedia/en/2/27/The_Offspring-Conspiracy_of_One.jpg",
        //   title: "Conspiracy of one",
        //   artist: "The Offspring"
        // },
        // {
        //   id: 11,
        //   album_cover:"https://i.ebayimg.com/00/s/NzQ0WDc1MA==/z/OGsAAOSw2DFgEPPD/$_12.JPG?set_id=880000500F",
        //   title: "40oz. to Freedom",
        //   artist: "Sublime"
        // }

      ]
    },

    mutations: {
      SET_AUTH_TOKEN(state, token) {
        state.token = token;
        localStorage.setItem('token', token);
        axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
      },
      SET_USER(state, user) {
        state.user = user;
        localStorage.setItem('user', JSON.stringify(user));
      },
      LOGOUT(state) {
        localStorage.removeItem('token');
        localStorage.removeItem('user');
        state.token = '';
        state.user = {};
        axios.defaults.headers.common = {};
      },
      ADD_SEARCH_RESULT(state, result){
        state.searchResults = result;
      },
      ADD_RECORDS_TO_DB(state, bibub){
        state.recordsInDB = bibub;
      },
      ADD_RECORDS_TO_LIBRARY(state,adding){
        state.records = adding;
      },
      SHOW_RECORDS_IN_LIBRARY(state,result){
        state.records = result;
      }
    },



  });
  return store;
}
