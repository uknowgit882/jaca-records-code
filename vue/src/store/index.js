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
