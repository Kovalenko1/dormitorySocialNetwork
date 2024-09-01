import { configureStore } from '@reduxjs/toolkit';
import authReducer from './slices/authSlice';
import searchReducer from './slices/searchSlice';

const store = configureStore({
    reducer: {
        auth: authReducer,
        search: searchReducer,
    },
});

export default store;