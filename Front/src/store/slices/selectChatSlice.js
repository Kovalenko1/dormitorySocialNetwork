import { createSlice } from '@reduxjs/toolkit';

const selectChatSlice = createSlice({
    name: 'chat',
    initialState: {
        selectedChatId: null,
        bodyName: null,
    },
    reducers: {
        setSelectedChat: (state, action) => {
            state.selectedChatId = action.payload['chatId'];
            state.bodyName = action.payload['name'];
        },
        clearSelectedChat: (state) => {
            state.selectedChatId = null;
            state.bodyName = null;
        }
    },
});

export const { setSelectedChat, clearSelectedChat } = selectChatSlice.actions;

export default selectChatSlice.reducer;
