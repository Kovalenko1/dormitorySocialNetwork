import { configureStore } from '@reduxjs/toolkit';
import authReducer from './slices/authSlice';
import searchReducer from './slices/searchSlice';
import selectChatReducer from "./slices/selectChatSlice";
import chatReducer from './slices/chatSlice';

const store = configureStore({
    reducer: {
        auth: authReducer,
        search: searchReducer,
        selectChat: selectChatReducer,
        chat: chatReducer,
    },
});

export default store;