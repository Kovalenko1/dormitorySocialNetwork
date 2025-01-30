import { configureStore, combineReducers  } from '@reduxjs/toolkit';
import authReducer from './slices/authSlice';
import searchReducer from './slices/searchSlice';
import selectChatReducer from "./slices/selectChatSlice";
import chatReducer from './slices/chatSlice';

const appReducer = combineReducers({
    auth: authReducer,
    search: searchReducer,
    selectChat: selectChatReducer,
    chat: chatReducer,
});

const rootReducer = (state, action) => {
    console.log("action", action, action.type);
    if (action.type === "auth/logout") {
        state = undefined;
    }
    console.log(action);
    return appReducer(state, action);
};

const store = configureStore({
    reducer: rootReducer,
});

export default store;