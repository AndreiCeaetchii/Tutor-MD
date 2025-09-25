// import {
//   getFavouriteTutors,
//   addToFavourites,
//   removeFromFavourites,
//   getTutorById
// } from '../services/tutorService';

// // State interface
// interface FavoriteTutorsState {
//   favouriteTutors: any[];
//   loading: boolean;
//   error: string | null;
// }

// const state: FavoriteTutorsState = {
//   favouriteTutors: [],
//   loading: false,
//   error: null
// };

// const getters = {
//   isFavourite: (state) => (tutorId) => {
//     return state.favouriteTutors.some(tutor => tutor.id === tutorId);
//   }
// };

// const actions = {
//   async fetchFavouriteTutors({ commit }) {
//     commit('SET_LOADING', true);
//     try {
//       const response = await getFavouriteTutors();
//       commit('SET_FAVOURITE_TUTORS', response.data || []);
//     } catch (error) {
//       commit('SET_ERROR', 'Failed to load favourite tutors');
//       console.error('Error fetching favourite tutors:', error);
//     } finally {
//       commit('SET_LOADING', false);
//     }
//   },
  
//   async addToFavourites({ commit }, tutorId) {
//     try {
//       await addToFavourites(tutorId);
//       // If we have tutor details, we can add directly to state
//       // Otherwise we'll need to fetch the tutor details first
//       const tutorDetails = await getTutorById(tutorId);
//       commit('ADD_FAVOURITE', tutorDetails);
//     } catch (error) {
//       commit('SET_ERROR', 'Failed to add tutor to favourites');
//       console.error('Error adding to favourites:', error);
//     }
//   },
  
//   async removeFromFavourites({ commit }, tutorId) {
//     try {
//       await removeFromFavourites(tutorId);
//       commit('REMOVE_FAVOURITE', tutorId);
//     } catch (error) {
//       commit('SET_ERROR', 'Failed to remove tutor from favourites');
//       console.error('Error removing from favourites:', error);
//     }
//   }
// };

// export default {
//   namespaced: true,
//   state,
//   getters,
//   actions,
// };