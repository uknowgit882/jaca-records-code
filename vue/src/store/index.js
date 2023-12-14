import { createStore as _createStore } from 'vuex';
import axios from 'axios';

export function createStore(currentToken, currentUser) {
  let store = _createStore({
    state: {
      token: currentToken || '',
      user: currentUser || {},
      library: [], // aseel added
      collections: [],
      publicCollections: [],
      gotResults: false,
      searchResults: [],
      searchLibraryResults: [],
      searchCollectionsResults: [],
      StatsAggregate: [],
      StatsUser: [],
      searchRequest: [],
      StatsRecords: [],
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
      ADD_SEARCH_RESULT(state, result) {
        state.searchResults = result;
        localStorage.setItem('discogsResults', JSON.stringify(result));
      },
      ADD_SEARCH_LIBRARY_RESULT(state, result) {
        state.searchLibraryResults = result;
        localStorage.setItem('libraryResults', JSON.stringify(result));
      },
      ADD_SEARCH_COLLECTIONS_RESULT(state, result) {
        state.searchCollectionsResults = result;
        localStorage.setItem('collectionsResults', JSON.stringify(result));
      },
      // ADD_RECORDS_TO_DB(state, bibub) {
      //   state.recordsInDB = bibub;
      // },
      ADD_RECORDS_TO_LIBRARY(state, adding) {
        state.library = adding;
      },
      PUSH_RECORDS_TO_LIBRARY(state, adding) {
        state.library.unshift(adding);
      },
      ADD_COLLECTIONS_TO_LIBRARY(state, collections){
        state.collections = collections;
      },
      ADD_PUBLIC_COLLECTIONS_TO_LIBRARY(state, collections){
        state.publicCollections = collections;
      },
      // SHOW_RECORDS_IN_LIBRARY(state, result) {
      //   state.records = result;
      // },
      // SHOW_INFO_FROM_COLLECTIONS(state, info) {
      //   state.Collections = info;
      // },
      ADD_AGGREGATE_STATS(state, stats) {
        state.StatsAggregate = stats;
      },
      ADD_USER_STATS(state, stats) {
        state.StatsUser = stats;
      },
      CHANGE_USER(state, user){
        state.user = user;
      },
      SET_SEARCH_REQUEST(state, search){
        state.searchRequest = search;
      },
      GET_TOTAL_RECORDS(state,result){
        state.StatsRecords = result;
      },
      SET_GOT_RESULTS_TRUE(state,result){
        state.gotResults = true;
      },
      SET_GOT_RESULTS_FALSE(state,setting){
        if(!this.searchResults){
          state.gotResults = false;
        }
      }
    },
  });
  return store;
}