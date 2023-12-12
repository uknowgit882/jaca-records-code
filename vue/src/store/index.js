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
      searchResults: [],
      searchLibraryResults: [],
      searchCollectionsResults: [],
      StatsAggregate: [],
      StatsUser: [],

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
      },
      ADD_SEARCH_LIBRARY_RESULT(state, result) {
        state.searchLibraryResults = result;
      },
      ADD_SEARCH_COLLECTIONS_RESULT(state, result) {
        state.searchCollectionsResults = result;
      },
      // ADD_RECORDS_TO_DB(state, bibub) {
      //   state.recordsInDB = bibub;
      // },
      ADD_RECORDS_TO_LIBRARY(state, adding) {
        state.library = adding;
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
    },
  });
  return store;
}