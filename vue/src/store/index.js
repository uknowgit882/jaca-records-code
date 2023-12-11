import { createStore as _createStore } from 'vuex';
import axios from 'axios';

export function createStore(currentToken, currentUser) {
  let store = _createStore({
    state: {
      token: currentToken || '',
      user: currentUser || {},
      library: [], // aseel added
      searchResults: [],
      searchLibraryResults: [],
      searchCollectionsResults: [],
      //recordsInDB: [],
      //records: [],
      Collections:
      {
        "name": "Backstreet Boys",
        "records": [
          {
            "id": 1,
            "title": "DNA",
            "released": " 2019-01-29",
            "country": "Orlando,FL",
            "notes": "none",
            "uri": "https://www.discogs.com/artist/11002-Backstreet-Boys",
            "artists": [
              {
                "name": "blablabla "
              }
            ],
            "extraArtists": [
              {
                "name": "no"
              }
            ],
            "formats": [
              {
                "name": "no"
              }
            ],
            "genres": [
              "unicorn"
            ],
            "identifiers": [
              {
                "type": "they",
                "value": "bad"
              }
            ],
            "images": [
              {
                "uri": "https://i.ebayimg.com/images/g/CFwAAOSwTW1dgjwJ/s-l500.jpg",
                "height": 250,
                "width": 250
              }
            ],
            "labels": [
              {
                "name": "nbbmb",
                "resource_Url": "https://i.ebayimg.com/images/g/CFwAAOSwTW1dgjwJ/s-l500.jpg"
              }
            ],
            "tracklist": [
              {
                "title": "string",
                "position": "string",
                "duration": "string"
              }
            ],
            recordsInDB: [], // AT: what is this?
            records: [], // AT: what is this?
            Collections:
            {
              "name": "Backstreet Boys",
              "records": [
                {
                  "id": 1,
                  "title": "DNA",
                  "released": " 2019-01-29",
                  "country": "Orlando,FL",
                  "notes": "none",
                  "uri": "https://www.discogs.com/artist/11002-Backstreet-Boys",
                  "artists": [
                    {
                      "name": "blablabla "
                    }
                  ],
                  "extraArtists": [
                    {
                      "name": "no"
                    }
                  ],
                  "formats": [
                    {
                      "name": "no"
                    }
                  ],
                  "genres": [
                    "unicorn"
                  ],
                  "identifiers": [
                    {
                      "type": "they",
                      "value": "bad"
                    }
                  ],
                  "images": [
                    {
                      "uri": "https://i.ebayimg.com/images/g/CFwAAOSwTW1dgjwJ/s-l500.jpg",
                      "height": 250,
                      "width": 250
                    }
                  ],
                  "labels": [
                    {
                      "name": "nbbmb",
                      "resource_Url": "https://i.ebayimg.com/images/g/CFwAAOSwTW1dgjwJ/s-l500.jpg"
                    }
                  ],
                  "tracklist": [
                    {
                      "title": "string",
                      "position": "string",
                      "duration": "string"
                    }
                  ]

                }
              ],
            }
          }
        ]

      },
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
      ADD_RECORDS_TO_DB(state, bibub) {
        state.recordsInDB = bibub;
      },
      ADD_RECORDS_TO_LIBRARY(state, adding) {
        state.records = adding;
        state.library = adding;
      },
      SHOW_RECORDS_IN_LIBRARY(state, result) {
        state.records = result;
      },
      SHOW_INFO_FROM_COLLECTIONS(state, info) {
        state.Collections = info;
      },
      ADD_AGGREGATE_STATS(state, stats) {
        state.StatsAggregate = stats;
      },
      ADD_USER_STATS(state, stats) {
        state.StatsUser = stats;
      },

    },
  });
  return store;
}