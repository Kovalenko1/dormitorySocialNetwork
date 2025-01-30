import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import { connectToChatHub as connectService} from '../../services/chatService';

const loadMessagesFromLocalStorage = () => {
    const messages = {};
    for (let key in localStorage) {
        if (localStorage.hasOwnProperty(key)) {
            try {
                if (key === 'user'){
                    const tempMessages = JSON.parse(localStorage.getItem(key)).chats;
                    for (let key2 in tempMessages) {
                        messages[tempMessages[key2].roomName] = tempMessages[key2].messages;
                    }
                }
            } catch (e) {
                console.error(`Ошибка при парсинге ключа ${key}:`, e);
            }
        }
    }
    console.log("mess", messages);
    return messages;
};

const initialState = {
    connectionStatus: 'disconnected',
    messages: {},
    currentChatId: null,
    status: 'idle',
    error: null,
};

export const connectToChatHub = createAsyncThunk(
    'chat/connectToChatHub',
    async (_, { dispatch, rejectWithValue }) => {
        try {
            return await connectService(dispatch, receiveMessage);
        } catch (error) {
            return rejectWithValue('Connection failed');
        }
    }
);

export const setChats = createAsyncThunk(
    'chat/setChats',
    async (chats, { rejectWithValue }) => {
        try {
            return loadMessagesFromLocalStorage();
        } catch (error) {
            return rejectWithValue(error.message);
        }
    }
);

const chatSlice = createSlice({
    name: 'chat',
    initialState,
    reducers: {
        receiveMessage: (state, action) => {
            const { userName, message, chatRoom } = action.payload;
            const chatRoomName = chatRoom || state.currentChatRoom;

            if (chatRoomName) {
                if (!state.messages[chatRoomName]) {
                    state.messages[chatRoomName] = [];
                }
                state.messages[chatRoomName].push({ userName, message });
            }
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(connectToChatHub.fulfilled, (state, action) => {
                state.connectionStatus = action.payload.connectionStatus;
            })
            .addCase(connectToChatHub.rejected, (state, action) => {
                state.connectionStatus = 'disconnected';
                state.error = action.payload;
            })
            .addCase(setChats.fulfilled, (state, action) => {
                state.messages = action.payload;
            })
            .addCase(setChats.rejected, (state, action) => {
                state.error = action.payload;
            });
    },
});

export const { receiveMessage } = chatSlice.actions;

export default chatSlice.reducer;
